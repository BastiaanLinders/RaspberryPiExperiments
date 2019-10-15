using System;
using System.Device.Gpio;
using System.Diagnostics.CodeAnalysis;
using Curly.PrimaNova.Abstractions;
using Curly.PrimaNova.Abstractions.Services;
using Services.Gpio;

namespace DeviceServices
{
	public class SituationProvider : IActivityStateProvider, IDisposable
	{
		private const int RoomSensor = (int) GpioPins.Gpio19;
		private const int Chair1Sensor = (int) GpioPins.Gpio20;
		private const int Chair2Sensor = (int) GpioPins.Gpio21;

		private readonly GpioController _controller;

		public SituationProvider(GpioController controller)
		{
			_controller = controller;

			Init();
		}

		[SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
		[SuppressMessage("ReSharper", "ConvertIfStatementToReturnStatement")]
		public ActivityState GetState()
		{
			return new ActivityState
			{
				MovementDetected = DetectMovement(),
				Butt1Detected = DetectButt1(),
				Butt2Detected = DetectButt2()
			};
		}

		private bool DetectMovement()
		{
			var pinValue = _controller.Read(RoomSensor);
			return pinValue == PinValue.Low;
		}

		private bool DetectButt1()
		{
			var pinValue = _controller.Read(Chair1Sensor);
			return pinValue == PinValue.Low;
		}

		private bool DetectButt2()
		{
			var pinValue = _controller.Read(Chair2Sensor);
			return pinValue == PinValue.Low;
		}

		private void Init()
		{
			_controller.OpenPin(RoomSensor, PinMode.InputPullUp);
			_controller.OpenPin(Chair1Sensor, PinMode.InputPullUp);
			_controller.OpenPin(Chair2Sensor, PinMode.InputPullUp);
		}

		public void Dispose()
		{
			_controller?.ClosePin(RoomSensor);
			_controller?.ClosePin(Chair1Sensor);
			_controller?.ClosePin(Chair2Sensor);
		}
	}
}