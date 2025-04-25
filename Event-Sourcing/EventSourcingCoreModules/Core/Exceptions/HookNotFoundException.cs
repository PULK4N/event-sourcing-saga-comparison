namespace EventSourcing.Core
{
    public class HookTypeNotFoundException : Exception
    {
        public HookTypeNotFoundException(string hookName)
            : base(
                $"Class for the hook named {hookName}, could not be found. Possible error due to assembly configuration"
            ) { }
    }
}
