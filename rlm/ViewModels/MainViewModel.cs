using ReactiveUI;
using rlm.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rlm.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        public Dictionary<string, Trait> AllTraits =
            new Trait[]
            {
                new() { Name = "Analytical", Stats = { Intelligence = 2 } },
                new() { Name = "Wired", Stats = { Reflexes = 4 } },
                new() { Name = "Slow", Stats = { Reflexes = -1 } },
            }.ToDictionary(w => w.Name);

        public ObservableCollection<Raider> Raiders { get; }

        public MainViewModel() =>
            Raiders = new()
            {
                new() { Name = "John", Traits = { AllTraits["Analytical"], AllTraits["Wired"] } },
                new() { Name = "Timmy" },
                new() { Name = "Alice", Traits = { AllTraits["Slow"] } },
            };
    }
}
