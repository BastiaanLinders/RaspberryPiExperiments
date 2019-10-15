using System.Threading;
using System.Threading.Tasks;

namespace Curly.PrimaNova.Abstractions.Services
{
	public interface IAudienceStateMonitor
	{
		Task Run(CancellationToken cancellationToken);
	}
}