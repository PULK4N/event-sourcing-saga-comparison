namespace EventSourcing.Models
{
    public interface SharedStateData
    {
        public Guid AggregateId { get; set; }
        public DateTime LastUpdateTimestamp { get; set; }
    }
}
