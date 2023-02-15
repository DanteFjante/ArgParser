using System.Collections;
using System.Globalization;

namespace ArgParser
{
    public class ArgCollection : IReadOnlyCollection<ArgCollection.Option>
    {
        List<Option> collection;

        public ArgCollection(string[] args)
        {
            collection = new List<Option>();
            if (args != null)
            {
                collection.AddRange(MapArguments(args));
            }
        }

        public bool HasOption(char option) => collection.Any(p => !p.GetType().IsInstanceOfType(typeof(Argument)) && p.Value.First() == option);
        public bool HasArgument(string arg, StringComparison stringComparisonType = StringComparison.InvariantCulture) => collection.Any(p => (p as Argument)?.Name.Equals(arg, stringComparisonType) ?? false);

        public string? GetArgument(string name) => collection.Find(p => (p as Argument)?.Name.Equals(name) ?? false)?.Value;

        public T ParseArgument<T>(string name, IFormatProvider formatprovider = null) where T : IParsable<T>
        {
            return T.Parse(GetArgument(name), formatprovider ?? CultureInfo.InvariantCulture);
        }

        public bool TryParseArgument<T>(string name, out T value, IFormatProvider formatProvider = null) where T : IParsable<T>
        {
            return T.TryParse(GetArgument(name), formatProvider ?? CultureInfo.InvariantCulture, out value);
        }

        public int Count => collection.Count;

        public IEnumerator GetEnumerator()
        {
            return collection.GetEnumerator();
        }

        IEnumerator<Option> IEnumerable<Option>.GetEnumerator()
        {
            return collection.GetEnumerator();
        }

        static IEnumerable<Option> MapArguments(string[] args)
        {
            List<Option> list = new List<Option>();
            for (int i = 0; i < args.Length - 1; i++)
            {
                if (args[i].StartsWith("--"))
                {
                    list.Add(new Argument(args[i], args[i + 1]));
                }
                else if (args[i].StartsWith("-"))
                {
                    List<Option> options = new List<Option>();
                    list.AddRange(args[i].Skip(1).Select(p => new Option(p.ToString())));
                }
            }
            return list;

        }
        class Option
        {
            protected string value;
            public Option(string value)
            {
                this.value = value;
            }

            public string Value => value;
        }

        class Argument : Option
        {
            protected string name;
            public Argument(string name, string value) : base(value)
            {
                this.name = name;
                this.value = value;
            }
            public string Name => name;
        }
    }
}
