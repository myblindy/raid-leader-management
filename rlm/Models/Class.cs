using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.IO;
using rlm.Support;

namespace rlm.Models
{
    public class Class
    {
        public string Name { get; [Obsolete("Do not use setter.")] set; }
        public List<Specialization> Specializations { get; [Obsolete("Do not use setter.")] set; } = new();

        public static Class FromJsonFilePath(string path) =>
            JsonSerializer.Deserialize<Class>(File.ReadAllText(path), Global.JsonDeserializerOptions);

        public static IEnumerable<Class> AllFromJsonFilePath(string path) =>
            Directory.EnumerateFiles(path).Select(FromJsonFilePath);
    }

    public record Specialization(string Name, Roles Role, ArmorType ArmorType, WeaponType MainHandWeaponType, WeaponType OffHandWeaponType);

    public enum ArmorType { Cloth, Leather, Mail, Plate }

    public enum WeaponType
    {
        None,

        OneHandPhysicalMelee,
        OneHandCasterMelee,
        OneHandPhysicalRanged,
        OneHandCasterRanged,
        TankShield,
        CasterShield,
        CasterFocus,

        TwoHandPhysicalMelee,
        TwoHandPhysicalRanged,
        TwoHandCasterMelee,
        TwoHandCasterRanged,
    }

    [Flags]
    public enum Roles { None = 0, Tank = 1 << 0, Healer = 1 << 1, MeleeDamage = 1 << 2, RangedDamage = 1 << 3 }
}
