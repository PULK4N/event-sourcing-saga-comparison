namespace EventSourcing.Core
{
    public class StateDataNotFoundException : Exception
    {
        public StateDataNotFoundException(string stateDataName)
            : base(
                $"Class for the state data named {stateDataName}, could not be found. Possible error due to assembly configuration"
            ) { }
    }
}
