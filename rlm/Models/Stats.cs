﻿using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rlm.Models
{
    public class Stats : ReactiveObject
    {
        double reflexes;
        public double Reflexes { get => reflexes; set => this.RaiseAndSetIfChanged(ref reflexes, value); }

        double intelligence;
        public double Intelligence { get => intelligence; set => this.RaiseAndSetIfChanged(ref intelligence, value); }

        public void SetTotal(IEnumerable<Stats> stats) =>
            (Reflexes, Intelligence) = (stats.Sum(s => s.Reflexes), stats.Sum(s => s.Intelligence));
    }
}