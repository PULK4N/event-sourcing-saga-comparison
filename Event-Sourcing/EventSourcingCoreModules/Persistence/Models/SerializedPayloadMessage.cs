using EventSourcing.Shared.Models;
using Newtonsoft.Json;

namespace EventSourcing.Persistence.Models
{
    public enum MessageStatus
    {
        New,
        Reading,
        Error,
        Sent
    }

    public class SerializedPayloadMessage
    {
        public int Id { get; set; }
        public string SerializedPayloadMessageData { get; set; }
        public MessageStatus Status { get; set; } = MessageStatus.New;

        public static SerializedPayloadMessage FromPayload(EventPayload payload)
        {
            var serilalizedPayload = new SerializedPayloadMessage();

            serilalizedPayload.SerializedPayloadMessageData = JsonConvert.SerializeObject(payload);

            return serilalizedPayload;
        }
    }
}
