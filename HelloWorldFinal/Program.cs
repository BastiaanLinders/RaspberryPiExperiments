using System;
using System.Device.Gpio;
using System.Threading;
using DeviceServices;
using Services;
using Services.PrimaNova;

namespace HelloWorldFinal
{
	internal class Program
	{
		private static readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

		private static void Main(string[] args)
		{
			Console.WriteLine("Starting");

			Console.WriteLine("Setting up Cancel");
			Console.CancelKeyPress += OnCancel;
			var cancellationToken = _cancellationTokenSource.Token;

			Console.WriteLine("Start monitoring");
			MonitorSituation(cancellationToken);

			Console.WriteLine("Finished");
		}

		private static void MonitorSituation(CancellationToken cancellationToken)
		{
			Console.WriteLine("Creating controllers and services");
			using var controller = new GpioController(PinNumberingScheme.Board);
			using var situationProvider = new SituationProvider(controller);
			using var feedbackService = new FeedbackService(controller);
			using var lcdFeedbackService = new LcdFeedbackService();


			var lastState = AudienceState.Unknown;
			while (!cancellationToken.IsCancellationRequested)
			{
				var state = situationProvider.GetSituation();
				if (state != lastState)
				{
					Console.WriteLine($"State change: {state}");
					lcdFeedbackService.VisualizeAudienceState(state);
				}

				feedbackService.VisualizeAudienceState(state);

				lastState = state;
				Thread.Sleep(1000);
			}
		}

		private static void OnCancel(object sender, ConsoleCancelEventArgs e)
		{
			Console.WriteLine("OnCancel");
			_cancellationTokenSource.Cancel();

			e.Cancel = true;
		}
	}
}