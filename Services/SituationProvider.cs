using System;
using System.Device.Gpio;
using System.Diagnostics.CodeAnalysis;
using Services.Gpio;
using Services.PrimaNova;

namespace Services
{
	public class SituationProvider : IDisposable
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
		public AudienceState GetSituation()
		{
			bool movementDetected = DetectMovement();
			bool butt1Detected = DetectButt1();
			bool butt2Detected = DetectButt2();

			if (!movementDetected && !butt1Detected && !butt2Detected)
			{
				return AudienceState.Empty;
			}

			if (butt1Detected || butt2Detected)
			{
				return AudienceState.Sitting;
			}

			if (movementDetected)
			{
				return AudienceState.EnterLeaving;
			}

			return AudienceState.Unknown;
		}

		private bool DetectMovement()
		{
			var roomSensorValue = _controller.Read(RoomSensor);
			return roomSensorValue == PinValue.Low;
		}

		private bool DetectButt1()
		{
			var roomSensorValue = _controller.Read(Chair1Sensor);
			return roomSensorValue == PinValue.Low;
		}

		private bool DetectButt2()
		{
			var roomSensorValue = _controller.Read(Chair2Sensor);
			return roomSensorValue == PinValue.Low;
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