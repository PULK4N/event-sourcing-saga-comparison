namespace EventSourcing.Core
{
    public class HookTypeNotRegisteredException : Exception
    {
        public HookTypeNotRegisteredException(string hookName)
            : base(
                $"Class for the hook named {hookName}, could not be instantiated. Possible error due to not configuring Dependency Injection"
            ) { }
    }
}
