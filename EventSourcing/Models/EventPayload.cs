namespace EventSourcing.Models
{
    public class EventPayload
    {
        private EventPayload() { }

        public DateTime Timestamp { get; set; }
        public Guid AggregateId { get; set; }
        public Guid EventExecutor { get; set; }
        public string EventName { get; set; }
        public string StateMachineId { get; set; }
        public Dictionary<string, object> Data { get; set; }

        public static EventPayload Create(
            Dictionary<string, object> data,
            string name,
            Guid eventExecutor,
            string stateMachineId
        )
        {
            var payload = new EventPayload();

            payload.EventName = name;
            payload.Timestamp = DateTime.UtcNow;
            payload.Data = data;
            payload.StateMachineId = stateMachineId;
            payload.EventExecutor = eventExecutor;

            return payload;
        }
    }
}
