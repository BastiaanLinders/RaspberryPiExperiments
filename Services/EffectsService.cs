using System.Device.Gpio;
using System.Threading;
using System.Threading.Tasks;
using Services.Gpio;

namespace Services
{
	public class EffectsService
	{
		private const int GreenPin = (int) GpioPins.Gpio05;
		private const int RedPin = (int) GpioPins.Gpio06;

		private readonly GpioController _controller;

		public EffectsService(GpioController controller)
		{
			_controller = controller;
		}

		public void Blink(int interval, CancellationToken cancellationToken)
		{
			Task.Run(() =>
			{
				_controller.OpenPin(GreenPin, PinMode.Output);
				_controller.OpenPin(RedPin, PinMode.Output);
				try
				{
					while (!cancellationToken.IsCancellationRequested)
					{
						_controller.Write(RedPin, PinValue.High);
						_controller.Write(GreenPin, PinValue.Low);
						Thread.Sleep(interval);
						_controller.Write(RedPin, PinValue.Low);
						_controller.Write(GreenPin, PinValue.High);
						Thread.Sleep(interval);
					}
				}
				finally
				{
					_controller.ClosePin(GreenPin);
					_controller.ClosePin(RedPin);
				}
			}, cancellationToken);
		}
	}
}