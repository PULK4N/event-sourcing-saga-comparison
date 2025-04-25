namespace EventSourcing.Core
{
    public class EventReducerMapNotAddedException : Exception
    {
        public EventReducerMapNotAddedException(string eventName)
            : base(
                $"Map for the event {eventName}, could not be found. Possible error due to not adding event reducer map to appsettings."
            ) { }
    }
}
