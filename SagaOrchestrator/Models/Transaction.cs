namespace SagaOrchestrator
{
    public enum TransactionState
    {
        NULL_STATE = 0,
        IN_PROGRESS = 1,
        COMPLETED = 10
    }

    public class Transaction
    {
        public DateTime Timestamp { get; set; }
        public string SerializedTransactionStep { get; set; } = string.Empty;
        public TransactionState TransactionState { get; set; }
        public string SerializedTransactionErrors { get; set; } = string.Empty;
    }
}
