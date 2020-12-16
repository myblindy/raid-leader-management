using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rlm.Models
{
    public class Raider : ReactiveObject
    {
        public Stats Stats { get; } = new();

        public Stats TotalStats { get; } = new();

        public ObservableCollection<Trait> Traits { get; } = new();

        string name;
        public string Name { get => name; set => this.RaiseAndSetIfChanged(ref name, value); }

        Class @class;
        public Class Class { get => @class; set => this.RaiseAndSetIfChanged(ref @class, value); }

        Specialization spec;
        public Specialization Specialization { get => spec; set => this.RaiseAndSetIfChanged(ref spec, value); }

        public Raider()
        {
            this.WhenAnyObservable(x => x.Stats.Changed).Subscribe(w => UpdateTotalStats());
            Traits.ToObservableChangeSet(x => x)
                .ToCollection()
                .Subscribe(w => UpdateTotalStats());
        }

        public Raider(string name, Class @class, Specialization spec) : this() =>
            (Name, Class, Specialization) = (name, @class, spec);

        public Raider(string name, string @class, string spec, Dictionary<string, Class> classes) :
            this(name, classes[@class], classes[@class].Specializations[spec])
        {
        }

        private void UpdateTotalStats() =>
            TotalStats.SetTotal(Traits.Select(t => t.Stats).Append(Stats));
    }
}
