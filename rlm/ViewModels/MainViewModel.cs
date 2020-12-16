using ReactiveUI;
using rlm.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
                new() { Name = "Smooth", Stats = { Charisma = 5 } },
                new() { Name = "Reckless", Stats = {Intelligence = -1, Reflexes = 2 } },
            }.ToDictionary(w => w.Name);

        public Dictionary<string, Class> AllClasses =
            new Class[]
            {
                new("Fighter",
                    new Specialization( "Brawler", RaiderRoles.MeleeDamage ),
                    new Specialization( "Protector", RaiderRoles.Tank ),
                    new Specialization( "Weaponeer", RaiderRoles.MeleeDamage )),
                new("Cleric",
                    new Specialization( "Holy", RaiderRoles.Healer ),
                    new Specialization( "Shadow", RaiderRoles.RangedDamage ),
                    new Specialization( "Battle", RaiderRoles.MeleeDamage )),
                new("Stabber",
                    new Specialization( "Assassin", RaiderRoles.MeleeDamage ),
                    new Specialization( "Thief", RaiderRoles.MeleeDamage ),
                    new Specialization( "Arcane Shadow", RaiderRoles.RangedDamage )),
                new("Paladin",
                    new Specialization( "Protector", RaiderRoles.Tank ),
                    new Specialization( "Holy", RaiderRoles.Healer ),
                    new Specialization( "Battle", RaiderRoles.MeleeDamage )),
            }.ToDictionary(w => w.Name);

        public ObservableCollection<Raider> Raiders { get; }

        DateTime currentDate = new(2004, 11, 23);
        public DateTime CurrentDate { get => currentDate; set => this.RaiseAndSetIfChanged(ref currentDate, value); }

        bool running;
        public bool Running { get => running; set => this.RaiseAndSetIfChanged(ref running, value); }

        int runSpeed = 1;
        public int RunSpeed { get => runSpeed; set => this.RaiseAndSetIfChanged(ref runSpeed, value); }

        public ICommand RunToggleCommand { get; }

        public MainViewModel()
        {
            Raiders = new()
            {
                new("John", "Fighter", "Protector", AllClasses) { Traits = { AllTraits["Analytical"], AllTraits["Wired"] } },
                new("Thomas", "Paladin", "Protector", AllClasses) { Traits = { AllTraits["Reckless"], AllTraits["Wired"] } },
                new("Timmy", "Stabber", "Arcane Shadow", AllClasses) { Traits = { AllTraits["Smooth"] } },
                new("Alice", "Cleric", "Holy", AllClasses) { Traits = { AllTraits["Slow"] } },
            };

            RunToggleCommand = ReactiveCommand.Create(() => Running = !Running);
        }
    }
}
