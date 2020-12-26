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
        public string Name { get; init; }
        public Encounter[] Encounters { get; init; }
        public int RequiredRaiders => 40;
        public int RequiredTanks => 3;
        public int RequiredHealers => 12;
        public int BaseItemLevel { get; }

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
                    Raid = this,
                    EncounterMechanics = globalState.Random.Next(globalState.AllEncounterMechanics, encounterMechanics).ToArray()
                };

            // generate the loot
            {
                var loot = new List<Loot>();

                // all armor
                foreach (var armorType in Enum.GetValues<ArmorType>())
                    for (int slotIdx = 0; slotIdx < Raider.MaxArmorSlots; ++slotIdx)
                        for (int copies = globalState.Random.Next(2, 7); copies >= 0; --copies)
                            loot.Add(new ArmorLoot(BaseItemLevel, armorType, slotIdx));

                // distribute the loot
                var encIdx = 0;
                while (loot.Any())
                {
                    var lootIdx = globalState.Random.Next(loot.Count);
                    Encounters[encIdx].PossibleLoot.Add(loot[lootIdx]);
                    loot.RemoveAt(lootIdx);
                    encIdx = ++encIdx % Encounters.Length;
                }
            }
        }
    }

    public class Encounter
    {
        public string Name { get; init; }
        public TimeSpan Duration { get; init; }
        public Raid Raid { get; init; }
        public EncounterMechanic[] EncounterMechanics { get; init; }
        public List<Loot> PossibleLoot { get; } = new();

        public IEnumerable<Loot> GenerateLootDrops(GlobalState globalState)
        {
            for (int lootIndex = globalState.Random.Next(2, 5); lootIndex >= 0; --lootIndex)
                yield return globalState.Random.Next(PossibleLoot);
        }
    }

    public abstract record Loot(int ItemLevel) { }
    public record ArmorLoot(int ItemLevel, ArmorType ArmorType, int SlotIndex) : Loot(ItemLevel) { }

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
