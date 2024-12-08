namespace SagaOrchestrator
{
    public class TransactionStep
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public int OrderNumber { get; set; }
        public DateTime Timestamp { get; set; }
        public string SerializedTransactionStepData { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string SerializedTransactionErrors { get; set; } = string.Empty;
    }
}
