using System;
using System.Threading;
using System.Threading.Tasks;
using Curly.PrimaNova.Abstractions;
using Curly.PrimaNova.Abstractions.Events;
using Curly.PrimaNova.Abstractions.Services;
using MediatR;

namespace Curly.PrimaNova.Services
{
	public class AudienceStateMonitor : IAudienceStateMonitor
	{
		private const int ChecksPerSecond = 4;

		private readonly IMediator _mediator;
		private readonly IAudienceStateProvider _audienceStateProvider;

		public AudienceStateMonitor(IMediator mediator,
		                            IAudienceStateProvider audienceStateProvider)
		{
			_mediator = mediator;
			_audienceStateProvider = audienceStateProvider;
		}

		public async Task Run(CancellationToken cancellationToken)
		{
			const int breakDuration = 1000 / ChecksPerSecond;
			var previousState = AudienceState.Unknown;
			while (!cancellationToken.IsCancellationRequested)
			{
				var audienceState = _audienceStateProvider.GetState();

				if (audienceState != previousState)
				{
					Console.WriteLine("Monitor noticed state change: firing event");
					var changedEvent = new AudienceStateChangedEvent(previousState, audienceState);
					await _mediator.Publish(changedEvent, cancellationToken);
				}

				await Task.Delay(breakDuration, cancellationToken);
				previousState = audienceState;
			}
		}
	}
}