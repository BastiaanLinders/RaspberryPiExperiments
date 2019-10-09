using System;
using System.Device.Gpio;
using System.Threading;
using Services;

namespace HelloWorldFinal
{
	internal class Program
	{
		private static readonly CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();

		private static void Main(string[] args)
		{
			Console.WriteLine("Starting");

			if (args.Length > 0 && int.TryParse(args[0], out var blinkerDuration))
			{
				Console.WriteLine($"Duration passed: {blinkerDuration}ms");
			}
			else
			{
				blinkerDuration = 500;
				Console.WriteLine("No duration passed. Setting default (500ms).");
			}

			Console.WriteLine("Setting up Cancel");
			Console.CancelKeyPress += OnCancel;

			Console.WriteLine("Creating LedController");

			using (var controller = new GpioController(PinNumberingScheme.Board))
            using (var situationProvider = new SituationProvider(controller))
            using (var feedbackService = new FeedbackService(controller))
            {
                Console.WriteLine("Calling Blinker");
                ledController.Blinker(blinkerDuration, CancellationTokenSource.Token);
            }

			Console.WriteLine("Finished");
		}

		private static void OnCancel(object sender, ConsoleCancelEventArgs e)
		{
			Console.WriteLine("OnCancel");
			CancellationTokenSource.Cancel();

			e.Cancel = true;
		}
	}
}