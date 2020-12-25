using rlm.Models;
using rlm.Support;
using rlm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace rlm.Services
{
    class Simulator
    {
        readonly MainViewModel vm;
        readonly Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
        readonly Thread simThread;

        public Simulator(MainViewModel vm)
        {
            this.vm = vm;
            simThread = new(SimulatorThread) { Name = "Simulator Thread", IsBackground = true };
            simThread.Start();
        }

        void SimulatorThread()
        {
            var speeds = new[] { TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(0) };
            var raidTick = TimeSpan.FromSeconds(10);
            var baseSuccessChance = .95;

            while (true)
            {
                while (!vm.Running) Thread.Sleep(1);

                var nextDayAt = DateTime.Now + speeds[vm.RunSpeed - 1];

                // if a raid day, resolve the raid
                var raidHoursToday = TimeSpan.FromHours(vm.WeeklyRaidSchedule[(int)vm.CurrentDate.DayOfWeek].Hours);
                if (raidHoursToday.TotalHours > 0)
                {
                    foreach (var raid in vm.Raids)
                    {
                        // build the group based on raid requirements
                        var raiders = vm.GlobalState.Random.Next(vm.Raiders.Where(r => r.Specialization.Role == Roles.Tank), raid.RequiredTanks)
                            .Concat(vm.GlobalState.Random.Next(vm.Raiders.Where(r => r.Specialization.Role == Roles.Healer), raid.RequiredHealers)
                            .Concat(vm.GlobalState.Random.Next(vm.Raiders.Where(r => r.Specialization.Role == Roles.MeleeDamage || r.Specialization.Role == Roles.RangedDamage), raid.RequiredRaiders - raid.RequiredTanks - raid.RequiredHealers)))
                            .ToList();

                        IEnumerable<Raider> RaidersWithRoles(Roles roles) =>
                            raiders.Where(r =>
                                (roles.HasFlag(Roles.Tank) && r.Specialization.Role == Roles.Tank) ||
                                (roles.HasFlag(Roles.Healer) && r.Specialization.Role == Roles.Healer) ||
                                (roles.HasFlag(Roles.MeleeDamage) && r.Specialization.Role == Roles.MeleeDamage) ||
                                (roles.HasFlag(Roles.RangedDamage) && r.Specialization.Role == Roles.RangedDamage));

                        foreach (var encounter in raid.Encounters)
                        {
                        wipe:
                            // healers can't dps
                            var encounterDuration = encounter.Duration * (raid.BaseItemLevel / raiders.Where(r => r.Specialization.Role != Roles.Healer).Average(r => r.AverageItemLevel));

                            double extraDifficulty = 0;

                            while ((encounterDuration -= raidTick).Ticks > 0)
                            {
                                foreach (var mechanic in encounter.EncounterMechanics)
                                    if (vm.GlobalState.Random.NextDouble() < 1.0 / (int)mechanic.Rate)
                                    {
                                        // the mechanic triggered

                                        // figure out the mechanic targets
                                        var targetRaidersEnumerable = RaidersWithRoles(mechanic.TargetRoles);

                                        targetRaidersEnumerable = mechanic.TargetCount switch
                                        {
                                            EncounterTargetCount.One => Enumerable.Repeat(vm.GlobalState.Random.Next(targetRaidersEnumerable), 1),
                                            EncounterTargetCount.All => targetRaidersEnumerable,
                                            EncounterTargetCount.Few => vm.GlobalState.Random.Next(targetRaidersEnumerable, targetRaidersEnumerable.Count() / 3),
                                            EncounterTargetCount.Many => vm.GlobalState.Random.Next(targetRaidersEnumerable, targetRaidersEnumerable.Count() * 2 / 3),
                                            _ => throw new InvalidOperationException(),
                                        };

                                        var targetRaiders = targetRaidersEnumerable.ToList();

                                        // figure out the targets to check for failure
                                        var failureCheckRaidersEnumerable = RaidersWithRoles(mechanic.FailureCheckRoles);
                                        if (mechanic.FailureCheckAffectedTargets) failureCheckRaidersEnumerable = failureCheckRaidersEnumerable.Concat(targetRaiders);
                                        var failureCheckRaiders = failureCheckRaidersEnumerable.ToList();

                                        // check who failed
                                        var raidersWhoFailed = failureCheckRaiders.Where(r => 
                                            vm.GlobalState.Random.NextDouble() < r.AverageItemLevel / raid.BaseItemLevel * baseSuccessChance * (mechanic.Difficulty + extraDifficulty) / 100.0).ToList();
                                        if (raidersWhoFailed.Any())
                                        {
                                            switch (mechanic.FailureType)
                                            {
                                                case EncounterFailureType.Death:
                                                    if (targetRaiders.Any(r => r.Specialization.Role == Roles.Tank))
                                                    {
                                                        // tank died, it's a wipe
                                                        goto wipe;
                                                    }
                                                    break;
                                                case EncounterFailureType.DifficultyIncrease:
                                                    extraDifficulty += mechanic.FailureDifficultyIncrease;
                                                    break;
                                            }
                                        }
                                    }

                                // advance time
                                if ((raidHoursToday -= raidTick).Ticks <= 0)
                                    goto raidEnd;
                            }

                            // boss dead!!
                            continue;
                        }
                    }

                raidEnd: { }
                }

                // next day
                while (DateTime.Now < nextDayAt) Thread.Sleep(1);
                dispatcher.BeginInvoke(() => vm.CurrentDate = vm.CurrentDate.AddDays(1));
            }
        }
    }
}
