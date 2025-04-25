namespace EventSourcing.Core
{
    public class ReducerNotFoundException : Exception
    {
        public ReducerNotFoundException(string ReducerName)
            : base(
                $"Class for the reducer named {ReducerName}, could not be found. Possible error due to assembly configuration"
            ) { }
    }
}
