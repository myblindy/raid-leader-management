using DynamicData;
using ReactiveUI;
using rlm.Models;
using rlm.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace rlm.ViewModels
{
    public class MainViewModel : ReactiveObject, IActivatableViewModel
    {
        public GlobalState GlobalState { get; } = new(
            Trait.AllFromJsonFilePath(@"data/traits"),
            Class.AllFromJsonFilePath(@"data/classes"),
            File.ReadAllLines(@"data/user_names.txt").Distinct().Where(s => !string.IsNullOrWhiteSpace(s)),
            File.ReadAllLines(@"data/encounter_names.txt").Distinct().Where(s => !string.IsNullOrWhiteSpace(s)),
            EncounterMechanic.AllFromJsonFilePath(@"data/mechanics"));

        public ObservableCollection<Raider> Raiders { get; } = new();

        public ObservableCollection<Raid> Raids { get; } = new();

        #region Summary Counts
        int clothCount, leatherCount, mailCount, plateCount;
        public int ClothCount { get => clothCount; set => this.RaiseAndSetIfChanged(ref clothCount, value); }
        public int LeatherCount { get => leatherCount; set => this.RaiseAndSetIfChanged(ref leatherCount, value); }
        public int MailCount { get => mailCount; set => this.RaiseAndSetIfChanged(ref mailCount, value); }
        public int PlateCount { get => plateCount; set => this.RaiseAndSetIfChanged(ref plateCount, value); }

        int tankCount, healerCount, meleeDamageCount, rangedDamageCount;
        public int TankCount { get => tankCount; set => this.RaiseAndSetIfChanged(ref tankCount, value); }
        public int HealerCount { get => healerCount; set => this.RaiseAndSetIfChanged(ref healerCount, value); }
        public int MeleeDamageCount { get => meleeDamageCount; set => this.RaiseAndSetIfChanged(ref meleeDamageCount, value); }
        public int RangedDamageCount { get => rangedDamageCount; set => this.RaiseAndSetIfChanged(ref rangedDamageCount, value); }
        #endregion

        DateTime currentDate = new(2004, 11, 23);
        public DateTime CurrentDate { get => currentDate; set => this.RaiseAndSetIfChanged(ref currentDate, value); }

        bool running;
        public bool Running { get => running; set => this.RaiseAndSetIfChanged(ref running, value); }

        int runSpeed = 1;
        public int RunSpeed { get => runSpeed; set => this.RaiseAndSetIfChanged(ref runSpeed, value); }

        public ReadOnlyObservableCollection<DailyRaidSchedule> WeeklyRaidSchedule { get; } = new(new(Enumerable.Range(0, 7).Select(idx => new DailyRaidSchedule { DayOfWeek = (DayOfWeek)idx })));

        public ICommand RunToggleCommand { get; }

        readonly Simulator simulator;

        public ViewModelActivator Activator { get; } = new();

        public MainViewModel()
        {
            Raiders.AddRange(Raider.CreateRaiders(tankRange: 2..5, healerRange: 13..18, damageDealerRange: 25..35, traitRange: 0..4, ilvlRange: 45..65, GlobalState));
            Raids.Add(Enumerable.Range(0, 3).Select(_ => new Raid(ilvl: 60, encounters: 6..12, encounterMechanics: 2..6, encounterDurationSeconds: 200..400, GlobalState)));

            this.WhenActivated(disposables =>
            {
                Raiders.AsObservableChangeSet().Subscribe(w =>
                {
                    ClothCount = LeatherCount = MailCount = PlateCount = HealerCount = TankCount = MeleeDamageCount = RangedDamageCount = 0;

                    foreach (var raider in Raiders.Where(r => r.Specialization is not null))
                    {
                        switch (raider.Specialization.ArmorType)
                        {
                            case ArmorType.Cloth: ++ClothCount; break;
                            case ArmorType.Leather: ++LeatherCount; break;
                            case ArmorType.Mail: ++MailCount; break;
                            case ArmorType.Plate: ++PlateCount; break;
                            default: throw new InvalidOperationException();
                        }
                        switch (raider.Specialization.Role)
                        {
                            case Roles.Healer: ++HealerCount; break;
                            case Roles.Tank: ++TankCount; break;
                            case Roles.MeleeDamage: ++MeleeDamageCount; break;
                            case Roles.RangedDamage: ++RangedDamageCount; break;
                            default: throw new InvalidOperationException();
                        }
                    }
                }).DisposeWith(disposables);
            });

            RunToggleCommand = ReactiveCommand.Create(() => Running = !Running);

            WeeklyRaidSchedule[(int)DayOfWeek.Saturday].Hours = 5;
            simulator = new(this);
        }
    }

    public class DailyRaidSchedule : ReactiveObject
    {
        public DayOfWeek DayOfWeek { get; init; }

        int hours;
        public int Hours { get => hours; set => this.RaiseAndSetIfChanged(ref hours, value); }
    }
}
