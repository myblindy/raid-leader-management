using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using rlm.Support;
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

        public ObservableCollection<int> WeaponSlots { get; } = new(Enumerable.Repeat(0, 2));
        public ObservableCollection<int> ArmorSlots { get; } = new(Enumerable.Repeat(0, 8));
        public ObservableCollection<int> OtherSlots { get; } = new(Enumerable.Repeat(0, 6));

        public double AverageItemLevel { get; private set; }

        public Raider()
        {
            this.WhenAnyObservable(x => x.Stats.Changed).Subscribe(w => UpdateTotalStats());
            Traits.ToObservableChangeSet().Subscribe(w => UpdateTotalStats());

            WeaponSlots.ToObservableChangeSet().Concat(ArmorSlots.ToObservableChangeSet().Concat(OtherSlots.ToObservableChangeSet()))
                .Subscribe(w => UpdateAverageItemLevel());
            this.WhenAnyValue(x => x.Specialization).Subscribe(w => UpdateAverageItemLevel());
        }

        public Raider(string name, Class @class, Specialization spec) : this() =>
            (Name, Class, Specialization) = (name, @class, spec);

        private void UpdateAverageItemLevel() =>
            AverageItemLevel = Specialization is null ? 0 : ((Specialization.OffHandWeaponType == WeaponType.None ? WeaponSlots[0] : WeaponSlots.Average()) + ArmorSlots.Average() + OtherSlots.Average()) / 3.0;

        private void UpdateTotalStats() =>
            TotalStats.SetTotal(Traits.Select(t => t.Stats).Append(Stats));

        public static IEnumerable<Raider> CreateRaiders(Range tankRange, Range healerRange, Range damageDealerRange, Range traitRange, Range ilvlRange, GlobalState globalState)
        {
            var tanks = globalState.Random.Next(tankRange.Start.Value, tankRange.End.Value);
            var healers = globalState.Random.Next(healerRange.Start.Value, healerRange.End.Value);
            var dds = globalState.Random.Next(damageDealerRange.Start.Value, damageDealerRange.End.Value);

            var names = new List<string>();

            while (tanks > 0 || healers > 0 || dds > 0)
            {
                Class @class = globalState.Random.Next(globalState.AllClasses);
                string name = globalState.Random.Next(globalState.AllUserNames.Except(names));
                var raider = new Raider(name, @class, globalState.Random.Next(@class.Specializations));
                raider.Traits.AddRange(globalState.Random.Next(globalState.AllTraits, globalState.Random.Next(traitRange.Start.Value, traitRange.End.Value)));

                raider.ArmorSlots.SetElements(_ => globalState.Random.Next(ilvlRange.Start.Value, ilvlRange.End.Value));
                raider.OtherSlots.SetElements(_ => globalState.Random.Next(ilvlRange.Start.Value, ilvlRange.End.Value));
                raider.WeaponSlots.SetElements(_ => globalState.Random.Next(ilvlRange.Start.Value, ilvlRange.End.Value));

                switch (raider.Specialization.Role)
                {
                    case Roles.Healer: if (healers-- > 0) yield return raider; names.Add(name); break;
                    case Roles.Tank: if (tanks-- > 0) yield return raider; names.Add(name); break;
                    default: if (dds-- > 0) yield return raider; names.Add(name); break;
                }
            }
        }
    }
}
