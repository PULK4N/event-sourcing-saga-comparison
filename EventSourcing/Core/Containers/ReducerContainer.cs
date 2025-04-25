namespace EventSourcing.Core.Containers
{
    public static class ReducerTypeContainer
    {
        private static readonly Dictionary<string, Type> reducers = new Dictionary<string, Type>();

        public static void AddReducerType(string fullName, Type reducer)
        {
            var name = fullName.Split('.').Last();
            reducers.Add(name, reducer);
        }

        public static Type? GetReducer(string name)
        {
            if (!reducers.ContainsKey(name))
                return null;
            return reducers[name];
        }
    }
}
