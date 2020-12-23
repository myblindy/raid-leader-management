using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rlm.Models
{
    public class GlobalState
    {
        public Dictionary<string, Trait> AllTraits { get; } = new();

        public Dictionary<string, Class> AllClasses { get; } = new();

        public List<string> AllUserNames { get; } = new();

        public Random Random { get; } = new();

        public GlobalState(IEnumerable<Trait> traits, IEnumerable<Class> classes, IEnumerable<string> userNames)
        {
            foreach (var trait in traits)
                AllTraits.Add(trait.Name, trait);

            foreach (var @class in classes)
                AllClasses.Add(@class.Name, @class);

            AllUserNames.AddRange(userNames);
        }
    }
}
