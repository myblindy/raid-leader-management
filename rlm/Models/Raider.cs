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

        public Raider()
        {
            this.WhenAnyObservable(x => x.Stats.Changed).Subscribe(w => UpdateTotalStats());
            Traits.ToObservableChangeSet(x => x)
                .ToCollection()
                //.Select(items => items.Any())
                .Subscribe(w => UpdateTotalStats());
        }

        private void UpdateTotalStats() =>
            TotalStats.SetTotal(Traits.Select(t => t.Stats).Concat(Enumerable.Repeat(Stats, 1)));
    }
}
