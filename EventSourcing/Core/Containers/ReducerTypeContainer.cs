namespace EventSourcing.Core.Containers
{
    public static class ReducerTypeContainer
    {
        private static readonly Dictionary<string, Type> reducerTypes =
            new Dictionary<string, Type>();

        public static void AddReducerType(string fullName, Type reducer)
        {
            var name = fullName.Split('.').Last();
            reducerTypes.Add(name, reducer);
        }

        public static Type? GetReducerType(string name)
        {
            if (!reducerTypes.ContainsKey(name))
                return null;
            return reducerTypes[name];
        }
    }
}
