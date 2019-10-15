using Curly.PrimaNova.Abstractions;
using Curly.PrimaNova.Abstractions.Services;

namespace Curly.PrimaNova.Services
{
	public class AudienceStateProvider : IAudienceStateProvider
	{
		private readonly ITheaterStateProvider _theaterStateProvider;
		private readonly IShowStateProvider _showStateProvider;
		private readonly IActivityStateProvider _activityStateProvider;

		public AudienceStateProvider(ITheaterStateProvider theaterStateProvider,
		                             IShowStateProvider showStateProvider,
		                             IActivityStateProvider activityStateProvider)
		{
			_theaterStateProvider = theaterStateProvider;
			_showStateProvider = showStateProvider;
			_activityStateProvider = activityStateProvider;
		}

		public AudienceState GetState()
		{
			var theaterState = _theaterStateProvider.GetState();
			var showState = _showStateProvider.GetState();
			var activityState = _activityStateProvider.GetState();


			if (theaterState != TheaterState.Opened)
			{
				return AudienceState.Unknown;
			}

			if (!activityState.MovementDetected && !activityState.Butt1Detected && !activityState.Butt2Detected)
			{
				return AudienceState.Empty;
			}

			if (showState == ShowState.Running && (activityState.Butt1Detected || activityState.Butt2Detected))
			{
				return AudienceState.Watching;
			}

			if (showState == ShowState.Running && !activityState.Butt1Detected && !activityState.Butt2Detected)
			{
				return AudienceState.Fleeing;
			}

			if (activityState.Butt1Detected || activityState.Butt2Detected)
			{
				return AudienceState.Sitting;
			}

			if (activityState.MovementDetected)
			{
				return AudienceState.EnterLeaving;
			}

			return AudienceState.Unknown;
		}
	}
}