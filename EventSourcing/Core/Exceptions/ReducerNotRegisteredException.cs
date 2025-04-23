namespace EventSourcing.Core
{
    public class ReducerNotRegisteredException : Exception
    {
        public ReducerNotRegisteredException(string ReducerName)
            : base(
                $"Class for the reducer named {ReducerName}, could not be instantiated. Possible error due to not registering dependency injection"
            ) { }
    }
}
