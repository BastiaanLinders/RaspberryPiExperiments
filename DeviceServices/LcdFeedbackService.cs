using System;
using Curly.PrimaNova.Abstractions;
using Curly.PrimaNova.Abstractions.Services;
using DeviceServices.Drivers;

namespace DeviceServices
{
	public class LcdFeedbackService : IAudienceStateVisualizer, IDisposable
	{
		private const int DisplayI2CBus = 1;
		private const int DisplayI2CAddress = 0x27;
		private LcdDisplay _lcdDisplay;

		public LcdFeedbackService()
		{
			Init();
		}

		public void VisualizeState(AudienceState audienceState)
		{
			WriteText(audienceState.ToString());
		}

		private void WriteText(string text)
		{
			_lcdDisplay.Clear();
			_lcdDisplay.DisplayText(text, 1);
		}

		private void Init()
		{
			_lcdDisplay = new LcdDisplay(DisplayI2CBus, DisplayI2CAddress);
			_lcdDisplay.Init();
		}

		public void Dispose()
		{
			_lcdDisplay?.Dispose();
		}
	}
}