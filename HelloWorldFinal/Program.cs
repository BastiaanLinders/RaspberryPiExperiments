using System;
using System.Threading;
using System.Threading.Tasks;
using Curly.PrimaNova.Abstractions.Services;
using Curly.PrimaNova.Services;
using DeviceServices;
using HelloWorldFinal.Stubs;
using Microsoft.Extensions.DependencyInjection;

namespace HelloWorldFinal
{
	internal class Program
	{
		private static readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

		private static async Task Main(string[] args)
		{
			Console.WriteLine("Starting");

			Console.WriteLine("Setting up Cancel");
			Console.CancelKeyPress += OnCancel;
			var cancellationToken = _cancellationTokenSource.Token;

			Console.WriteLine("Bootstrapping service provider");
			var serviceProvider = Bootstrap();
			using (var applicationScope = serviceProvider.CreateScope())
			{
				var monitor = applicationScope.ServiceProvider.GetRequiredService<IAudienceStateMonitor>();
				Console.WriteLine("Start monitoring");
				await monitor.Run(cancellationToken);
			}

			((IDisposable) serviceProvider).Dispose();

			Console.WriteLine("Finished");
		}

		private static IServiceProvider Bootstrap()
		{
			var serviceCollection = new ServiceCollection();

			StubServicesModule.RegisterServices(serviceCollection);
			DeviceServicesModule.RegisterServices(serviceCollection);
			PrimaNovaServicesModule.RegisterServices(serviceCollection);

			MediatRModule.RegisterServices(serviceCollection);

			return serviceCollection.BuildServiceProvider();
		}

		private static void OnCancel(object sender, ConsoleCancelEventArgs e)
		{
			Console.WriteLine("OnCancel");
			_cancellationTokenSource.Cancel();

			e.Cancel = true;
		}
	}
}