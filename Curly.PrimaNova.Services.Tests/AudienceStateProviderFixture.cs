using Curly.PrimaNova.Abstractions;
using Curly.PrimaNova.Abstractions.Services;
using Shouldly;
using Xunit;

namespace Curly.PrimaNova.Services.Tests
{
    public class AudienceStateProviderFixture : TestFixtureBase<AudienceStateProvider>
    {
        public AudienceStateProviderFixture()
        {
            GetMock<ITheaterStateProvider>().Setup(tsp => tsp.GetState())
                                            .Returns(TheaterState.Opened);
        }

        [Fact]
        public void GetState_WhenTheaterClosed_ReturnsUnknown()
        {
            GetMock<ITheaterStateProvider>().Setup(tsp => tsp.GetState())
                                            .Returns(TheaterState.Closed);

            var subject = GetSubject();
            var state = subject.GetState();

            state.ShouldBe(AudienceState.Unknown);
        }

        [Fact]
        public void GetState_WithoutSensorInput_ReturnsEmpty()
        {
            GetMock<IActivityStateProvider>().Setup(ts => ts.GetState()).Returns(new ActivityState
            {
                MovementDetected = false,
                Butt1Detected = false,
                Butt2Detected = false
            });

            var subject = GetSubject();
            var state = subject.GetState();

            state.ShouldBe(AudienceState.Empty);
        }

        [Fact]
        public void GetState_MovementDetected_ReturnsEnterLeaving()
        {
            GetMock<IActivityStateProvider>().Setup(ts => ts.GetState()).Returns(new ActivityState
            {
                MovementDetected = true,
                Butt1Detected = false,
                Butt2Detected = false
            });

            var subject = GetSubject();
            var state = subject.GetState();

            state.ShouldBe(AudienceState.EnterLeaving);
        }

        [Theory]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void GetState_AnyChairActivate_ReturnsSitting(bool chair1Activated, bool chair2Activated)
        {
            GetMock<IActivityStateProvider>().Setup(ts => ts.GetState()).Returns(new ActivityState
            {
                MovementDetected = true,
                Butt1Detected = chair1Activated,
                Butt2Detected = chair2Activated
            });

            var subject = GetSubject();
            var state = subject.GetState();

            state.ShouldBe(AudienceState.Sitting);
        }

        [Theory]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void GetState_ShowRunningAndAnyChairActivated_ReturnsWatching(bool chair1Activated, bool chair2Activated)
        {
            GetMock<IShowStateProvider>().Setup(ssp => ssp.GetState()).Returns(ShowState.Running);
            GetMock<IActivityStateProvider>().Setup(ts => ts.GetState()).Returns(new ActivityState
            {
                MovementDetected = false,
                Butt1Detected = chair1Activated,
                Butt2Detected = chair2Activated
            });

            var subject = GetSubject();
            var state = subject.GetState();

            state.ShouldBe(AudienceState.Watching);
        }

        [Fact]
        public void GetState_ShowRunningAndNoChairActivatedAndMovement_ReturnsFleeing()
        {
            GetMock<IShowStateProvider>().Setup(ssp => ssp.GetState()).Returns(ShowState.Running);
            GetMock<IActivityStateProvider>().Setup(ts => ts.GetState()).Returns(new ActivityState
            {
                MovementDetected = true,
                Butt1Detected = false,
                Butt2Detected = false
            });

            var subject = GetSubject();
            var state = subject.GetState();

            state.ShouldBe(AudienceState.Fleeing);
        }

        [Fact]
        public void GetState_ShowRunningAndNoChairActivatedAndNoMovement_ReturnsEmpty()
        {
            GetMock<IShowStateProvider>().Setup(ssp => ssp.GetState()).Returns(ShowState.Running);
            GetMock<IActivityStateProvider>().Setup(ts => ts.GetState()).Returns(new ActivityState
            {
                MovementDetected = false,
                Butt1Detected = false,
                Butt2Detected = false
            });

            var subject = GetSubject();
            var state = subject.GetState();

            state.ShouldBe(AudienceState.Empty);
        }
    }
}