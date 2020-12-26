using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rlm.Models
{
    public class GlobalState
    {
        public Trait[] AllTraits { get; }

        public Class[] AllClasses { get; }

        public EncounterMechanic[] AllEncounterMechanics { get; }

        public string[] AllUserNames { get; }

        public string[] AllEncounterNames { get; }

        public string[] ArmorEquipmentSlotNames { get; } = new[]
        {
            "Head", "Shoulders", "Chest", "Wrist", "Hands", "Waist", "Legs", "Feet", 
        };

        public Random Random { get; } = new();

        public GlobalState(IEnumerable<Trait> traits, IEnumerable<Class> classes, IEnumerable<string> userNames, IEnumerable<string> encounterNames, IEnumerable<EncounterMechanic> encounterMechanics)
        {
            AllTraits = traits.ToArray();
            AllClasses = classes.ToArray();
            AllUserNames = userNames.ToArray();
            AllEncounterMechanics = encounterMechanics.ToArray();
            AllEncounterNames = encounterNames.ToArray();
        }
    }
}
