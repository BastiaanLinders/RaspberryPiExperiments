using MediatR;

namespace Curly.PrimaNova.Abstractions.Events
{
	public class AudienceStateChangedEvent : INotification
	{
		public AudienceStateChangedEvent(AudienceState previousState, AudienceState currentState)
		{
			PreviousState = previousState;
			CurrentState = currentState;
		}

		public AudienceState PreviousState { get; }
		public AudienceState CurrentState { get; }
	}
}