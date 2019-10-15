using Curly.PrimaNova.Abstractions.Events;
using Curly.PrimaNova.Abstractions.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Curly.PrimaNova.Services
{
	public static class PrimaNovaServicesModule
	{
		public static void RegisterServices(IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<IAudienceStateProvider, AudienceStateProvider>();
			serviceCollection.AddSingleton<IAudienceStateMonitor, AudienceStateMonitor>();

			// Event Handlers
			serviceCollection.AddSingleton<INotificationHandler<AudienceStateChangedEvent>, FeedbackHandler>();
		}
	}
}