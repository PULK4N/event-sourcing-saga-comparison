namespace SagaOrchestrator
{
    public class Transaction
    {
        public int Id { get; set; }
        public string State { get; set; } = string.Empty;
        public string SerializedTransactionData { get; set; } = string.Empty;
        public string SerializedTransactionErrors { get; set; } = string.Empty;
    }
}
