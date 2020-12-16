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
            }.ToDictionary(w => w.Name);

        public ObservableCollection<Raider> Raiders { get; }

        DateTime currentDate = new(2004, 11, 23);
        public DateTime CurrentDate { get => currentDate; set => this.RaiseAndSetIfChanged(ref currentDate, value); }

        bool running;
        public bool Running { get => running; set => this.RaiseAndSetIfChanged(ref running, value); }

        int runSpeed;
        public int RunSpeed { get => runSpeed; set => this.RaiseAndSetIfChanged(ref runSpeed, value); }

        public ICommand RunToggleCommand { get; }

        public MainViewModel()
        {
            Raiders = new()
            {
                new() { Name = "John", Traits = { AllTraits["Analytical"], AllTraits["Wired"] } },
                new() { Name = "Timmy", Traits = { AllTraits["Smooth"] } },
                new() { Name = "Alice", Traits = { AllTraits["Slow"] } },
            };

            RunToggleCommand = ReactiveCommand.Create(() => Running = !Running);
        }
    }
}
