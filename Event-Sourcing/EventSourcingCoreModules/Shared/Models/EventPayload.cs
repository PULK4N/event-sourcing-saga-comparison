using EventSourcing.Shared.Interfaces;

namespace EventSourcing.Shared.Models
{
    public class EventPayload
    {
        public EventPayload() { }

        public EventExecutionInfo EventExecutionInfo { get; set; }
        public IEvent EventData { get; set; }

        public static EventPayload Create(
            Guid eventExecutor,
            Guid AggregateId,
            string stateMachineId,
            IEvent eventData
        )
        {
            var payload = new EventPayload();

            payload.EventExecutionInfo = new EventExecutionInfo();
            payload.EventExecutionInfo.Id = Guid.NewGuid();
            payload.EventExecutionInfo.EventName = eventData.GetType().Name;
            payload.EventExecutionInfo.AssemblyQualifiedEventName = eventData
                .GetType()
                .AssemblyQualifiedName;
            payload.EventExecutionInfo.Timestamp = DateTime.UtcNow;
            payload.EventExecutionInfo.StateMachineId = stateMachineId;
            payload.EventExecutionInfo.EventExecutor = eventExecutor;
            payload.EventExecutionInfo.AggregateId = AggregateId;
            payload.EventData = eventData;

            return payload;
        }
    }
}
