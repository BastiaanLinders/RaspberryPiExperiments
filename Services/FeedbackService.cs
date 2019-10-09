using System;
using System.Device.Gpio;
using Services.Gpio;
using Services.PrimaNova;

namespace Services
{
	public class FeedbackService : IDisposable
	{
		private const int GreenLed = (int) GpioPins.Gpio05;
		private const int RedLed = (int) GpioPins.Gpio06;

		private readonly GpioController _controller;

		public FeedbackService(GpioController controller)
		{
			_controller = controller;

			Init();
		}

		public void VisualizeAudienceState(AudienceState audienceState)
		{
			switch (audienceState)
			{
				case AudienceState.Empty:
					_controller.Write(GreenLed, PinValue.Low);
					_controller.Write(RedLed, PinValue.Low);
					break;
				case AudienceState.EnterLeaving:
					_controller.Write(GreenLed, PinValue.High);
					_controller.Write(RedLed, PinValue.Low);
					break;
				case AudienceState.Sitting:
					_controller.Write(GreenLed, PinValue.Low);
					_controller.Write(RedLed, PinValue.High);
					break;
				case AudienceState.Unknown:
				default:
					_controller.Write(GreenLed, PinValue.High);
					_controller.Write(RedLed, PinValue.High);
					break;
			}
		}

		private void Init()
		{
			_controller.OpenPin(GreenLed, PinMode.Output);
			_controller.OpenPin(RedLed, PinMode.Output);
		}

		public void Dispose()
		{
			_controller?.ClosePin(GreenLed);
			_controller?.ClosePin(RedLed);
		}
	}
}