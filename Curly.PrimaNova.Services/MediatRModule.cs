using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Curly.PrimaNova.Services
{
	public static class MediatRModule
	{
		public static void RegisterServices(IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<IMediator, Mediator>();
			serviceCollection.AddTransient<ServiceFactory>(serviceProvider => serviceProvider.GetRequiredService);
		}
	}
}