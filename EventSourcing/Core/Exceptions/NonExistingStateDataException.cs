namespace EventSourcing.Core
{
    public class NonExistingStateDataException : Exception
    {
        public NonExistingStateDataException(string stateDataName)
            : base(
                $"Class for the state data named {stateDataName}, could not be found. Possible error due to assembly configuration"
            ) { }
    }
}
