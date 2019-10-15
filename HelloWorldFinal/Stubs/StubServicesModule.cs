using Curly.PrimaNova.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HelloWorldFinal.Stubs
{
	public static class StubServicesModule
	{
		public static void RegisterServices(IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<ITheaterStateProvider, TheaterStateProviderStub>();
			serviceCollection.AddSingleton<IShowStateProvider, ShowStateProviderStub>();
		}
	}
}