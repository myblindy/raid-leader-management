using rlm.Support;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace rlm.Models
{
    public class Trait
    {
        public string Name { get; [Obsolete("Do not use setter")] set; }
        public Stats Stats { get; [Obsolete("Do not use setter.")] set; } = new();

        public static Trait FromJsonFilePath(string path) =>
            JsonSerializer.Deserialize<Trait>(File.ReadAllText(path), Global.JsonDeserializerOptions);

        public static IEnumerable<Trait> AllFromJsonFilePath(string path) =>
            Directory.EnumerateFiles(path).Select(FromJsonFilePath);
    }
}
