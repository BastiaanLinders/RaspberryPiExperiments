using Curly.PrimaNova.Abstractions;
using Curly.PrimaNova.Abstractions.Services;

namespace HelloWorldFinal.Stubs
{
	public class TheaterStateProviderStub : ITheaterStateProvider
	{
		public TheaterState GetState()
		{
			return TheaterState.Opened;
		}
	}
}