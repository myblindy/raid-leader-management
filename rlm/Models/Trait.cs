using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rlm.Models
{
    public class Trait
    {
        public string Name { get; init; }
        public Stats Stats { get; } = new();
    }
}
