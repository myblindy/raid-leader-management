﻿using rlm.Models;
using rlm.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace rlm.Dialogs
{
    /// <summary>
    /// Interaction logic for RaidEncounterDetails.xaml
    /// </summary>
    public partial class RaidEncounterDetails : Window
    {
        public record RaiderMechanicRecordType(Raider Raider, EncounterMechanic Mechanic);
        public record ProcessedWipeBlameRecordType(int WipeCounter, IEnumerable<RaiderMechanicRecordType> Failures);
        public IEnumerable<ProcessedWipeBlameRecordType> ProcessedWipeBlameRecords { get; }

        public RaidEncounterDetails(RaidEncounterCompletedActivityLogEntry vm)
        {
            DataContext = vm;
            ProcessedWipeBlameRecords = vm.WipeBlameRecords.GroupBy(w => w.WipeCounter)
                .Select(w => new ProcessedWipeBlameRecordType(w.Key + 1, w.Select(r => new RaiderMechanicRecordType(r.Raider, r.Mechanic)).ToList()))
                .ToList();
            InitializeComponent();
        }
    }
}
