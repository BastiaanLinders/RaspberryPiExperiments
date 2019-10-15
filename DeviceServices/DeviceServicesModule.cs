using System.Device.Gpio;
using Curly.PrimaNova.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DeviceServices
{
	public static class DeviceServicesModule
	{
		public static void RegisterServices(IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton(new GpioController(PinNumberingScheme.Board));

			serviceCollection.AddSingleton<IActivityStateProvider, SituationProvider>();
			serviceCollection.AddSingleton<IAudienceStateVisualizer, LcdFeedbackService>();
		}
	}
}