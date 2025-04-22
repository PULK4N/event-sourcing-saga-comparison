namespace EventSourcing.Models
{
    /*
     * Necessary information for hook execution
     */
    public class HookInfo
    {
        public object StateData { get; set; }
        public IEnumerable<EventPayload> ExecutedEvents { get; set; }
    }
}
