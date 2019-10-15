using Curly.PrimaNova.Abstractions;
using Curly.PrimaNova.Abstractions.Services;

namespace HelloWorldFinal.Stubs
{
	public class ShowStateProviderStub : IShowStateProvider
	{
		public ShowState GetState()
		{
			return ShowState.Stopped;
		}
	}
}