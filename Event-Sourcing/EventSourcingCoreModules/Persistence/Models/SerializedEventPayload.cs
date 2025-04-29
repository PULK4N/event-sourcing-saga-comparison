using EventSourcing.Models;
using Newtonsoft.Json;

namespace EventSourcing.Persistence.Models
{
    public class SerializedEventPayload
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid AggregateId { get; set; }
        public uint OrderNumber { get; set; }
        public Guid EventExecutor { get; set; }
        public string EventName { get; set; } = string.Empty;
        public string StateMachineId { get; set; } = string.Empty;
        public string SerializedJsonData { get; set; } = string.Empty;

        public static SerializedEventPayload FromPayload(EventPayload payload)
        {
            var serilalizedPayload = new SerializedEventPayload();

            serilalizedPayload.Id = payload.Id;
            serilalizedPayload.Timestamp = payload.Timestamp;
            serilalizedPayload.AggregateId = payload.AggregateId;
            serilalizedPayload.OrderNumber = payload.OrderNumber;
            serilalizedPayload.EventExecutor = payload.EventExecutor;
            serilalizedPayload.EventName = payload.EventName;
            serilalizedPayload.StateMachineId = payload.StateMachineId;

            serilalizedPayload.SerializedJsonData = JsonConvert.SerializeObject(payload);

            return serilalizedPayload;
        }
    }
}
