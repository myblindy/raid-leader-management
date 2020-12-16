using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rlm.Models
{
    public record Class(string Name)
    {
        public Dictionary<string, Specialization> Specializations { get; } = new();

        public Class(string name, params Specialization[] specs) : this(name)
        {
            foreach (var spec in specs)
                Specializations.Add(spec.Name, spec);
        }
    }

    public record Specialization(string Name, RaiderRoles Role);

    [Flags]
    public enum RaiderRoles { Tank = 1 << 0, Healer = 1 << 1, MeleeDamage = 1 << 2, RangedDamage = 1 << 3 }
}
