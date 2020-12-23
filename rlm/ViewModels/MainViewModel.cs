using DynamicData;
using ReactiveUI;
using rlm.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace rlm.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        public GlobalState GlobalState { get; } = new(
            Trait.AllFromJsonFilePath(@"data/traits"),
            Class.AllFromJsonFilePath(@"data/classes"),
            File.ReadAllLines(@"data/usernames.txt").Distinct().Where(s => !string.IsNullOrWhiteSpace(s)));

        public ObservableCollection<Raider> Raiders { get; } = new();

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

        public ICommand RunToggleCommand { get; }

        public MainViewModel()
        {
            Raiders.AddRange(Raider.CreateRaiders(tankRange: 2..5, healerRange: 7..12, damageDealerRange: 25..35, traitRange: 0..4, ilvlRange: 45..65, GlobalState));
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
            });

            RunToggleCommand = ReactiveCommand.Create(() => Running = !Running);
        }
    }
}
