using rlm.Support;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace rlm.Models
{
    public class Raid
    {
        public string Name { get; set; }
        public Encounter[] Encounters { get; init; }
        public int RequiredRaiders => 40;
        public int RequiredTanks => 3;
        public int RequiredHealers => 12;
        public int BaseItemLevel { get; }

        public Raid() { }
        public Raid(int ilvl, Range encounters, Range encounterMechanics, Range encounterDurationSeconds, GlobalState globalState)
        {
            BaseItemLevel = ilvl;
            Name = globalState.Random.Next(globalState.AllEncounterNames);
            Encounters = new Encounter[globalState.Random.Next(encounters)];
            for (int encIdx = 0; encIdx < Encounters.Length; ++encIdx)
                Encounters[encIdx] = new Encounter
                {
                    Name = globalState.Random.Next(globalState.AllEncounterNames),
                    Duration = TimeSpan.FromSeconds(globalState.Random.Next(encounterDurationSeconds)),
                    EncounterMechanics = globalState.Random.Next(globalState.AllEncounterMechanics, encounterMechanics).ToArray()
                };
        }
    }

    public class Encounter
    {
        public string Name { get; set; }
        public TimeSpan Duration { get; set; }
        public EncounterMechanic[] EncounterMechanics { get; init; }
    }

    public class EncounterMechanic
    {
        public string Name { get; set; }
        public double Difficulty { get; set; }
        public EncounterMechanicRate Rate { get; set; }
        [JsonConverter(typeof(EnumBitFieldJsonConverter<Roles>))]
        public Roles TargetRoles { get; set; }
        public EncounterTargetCount TargetCount { get; set; }
        [JsonConverter(typeof(EnumBitFieldJsonConverter<Roles>))]
        public Roles FailureCheckRoles { get; set; }
        public bool FailureCheckAffectedTargets { get; set; }
        public EncounterFailureType FailureType { get; set; }
        public double FailureDifficultyIncrease { get; set; }

        public static EncounterMechanic FromJsonFilePath(string path) =>
            JsonSerializer.Deserialize<EncounterMechanic>(File.ReadAllText(path), Global.JsonDeserializerOptions);

        public static IEnumerable<EncounterMechanic> AllFromJsonFilePath(string path) =>
            Directory.EnumerateFiles(path).Select(FromJsonFilePath);

    }

    public enum EncounterMechanicRate { VeryOften, Often, Normal, Rare, VeryRare }
    public enum EncounterTargetCount { One, All, Few, Many }
    public enum EncounterFailureType { Death, DifficultyIncrease }
}
