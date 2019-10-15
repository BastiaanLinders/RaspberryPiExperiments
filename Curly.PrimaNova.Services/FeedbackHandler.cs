using System;
using System.Threading;
using System.Threading.Tasks;
using Curly.PrimaNova.Abstractions.Events;
using Curly.PrimaNova.Abstractions.Services;
using MediatR;

namespace Curly.PrimaNova.Services
{
	public class FeedbackHandler : INotificationHandler<AudienceStateChangedEvent>
	{
		private readonly IAudienceStateVisualizer _audienceStateVisualizer;

		public FeedbackHandler(IAudienceStateVisualizer audienceStateVisualizer)
		{
			_audienceStateVisualizer = audienceStateVisualizer;
		}

		public Task Handle(AudienceStateChangedEvent notification, CancellationToken cancellationToken)
		{
			Console.WriteLine($"Monitor noticed state change: {notification.PreviousState} --> {notification.CurrentState}");
			_audienceStateVisualizer.VisualizeState(notification.CurrentState);
			return Task.CompletedTask;
		}
	}
}