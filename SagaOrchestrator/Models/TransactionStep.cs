using Newtonsoft.Json.Linq;

namespace SagaOrchestrator
{
    public enum TransactionStepState
    {
        NULL_STATE = -1,
        INITIATED = 0,
        TIMED_OUT_1 = 1,
        TIMED_OUT_2 = 2,
        TIMED_OUT_3 = 3,
        TIMEOUT_FAILED = 8,
        TRANSITION_FAILED = 9,
        COMPLETED = 10
    }

    public abstract class TransactionStep
    {
        public virtual  TransactionStepState Execute(JObject data)
        {
            return TransactionStepState.COMPLETED;
        }
    }
}
