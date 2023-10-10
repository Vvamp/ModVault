using System.ComponentModel.Design.Serialization;

namespace ModVault.Core
{
    public enum ModSource
    {
        Modrinth,
        CurseForge,
        GitHub,
        Local,
        Unknown
    }
    public enum ModLoader
    {
        fabric,
        forge,
        quilt,
        other
    }

   
    public class File
    {
        public string url { get; set; }
        public string filename { get; set;}
        public Dictionary<string, string> hashes { get; set; }
    }
    public class ModVersionData
    {
        public string ModVersion { get; set; }
        public DateTime VersionDate { get; set; }
        public Version[] GameVersions { get; set; }
        public ModLoader[] Loaders { get; set; }
        public File PrimaryFile { get; set; }
        public static ModLoader GetModLoaderByString(string modloader_name)
        {
            ModLoader modLoader = ModLoader.other;
            if (Enum.TryParse(modloader_name, out modLoader))
                return modLoader;
            return ModLoader.other;
        }

        public static ModLoader[] GetModLoaderByStrings(string[] modloader_names)
        {
            List<ModLoader> modloaders = new List<ModLoader>();
            foreach (var modloader_name in modloader_names)
            {
                ModLoader modLoader = ModLoader.other;
                if (Enum.TryParse(modloader_name, out modLoader))
                    modloaders.Add(modLoader);
                modloaders.Add(ModLoader.other);
            }
            return modloaders.ToArray();
        }
    }
    public class Mod
    {
        public string Name { get; set; }
        public List<ModVersionData> VersionData { get; set; } = new List<ModVersionData>();
        public string? Author { get; set; }
        public string ReferenceId { get; set; }
        public ModSource Source { get; set; }

        public MatchedMod? WithLoaderAndVersion(ModLoader loader, string game_version)
        {
            var potential_versions_with_loader = VersionData.Where(vd => vd.Loaders.Contains(loader)).ToList();
            if (potential_versions_with_loader == null || potential_versions_with_loader.Count == 0)
            {
                return null; }

            var matching_versions = potential_versions_with_loader.Where(vd => vd.GameVersions.Contains(game_version.TryGetVersion())).ToList();
            if (matching_versions == null || matching_versions.Count == 0)
            {
                return null; }

            var last_version = matching_versions.OrderByDescending(x => x.VersionDate).First();

            // Return a new mod with only one version: the latest matching one
            return new MatchedMod()
            {
                Name = Name,
                VersionData = VersionData,
                Author = Author,
                ReferenceId = ReferenceId,
                Source = Source,
                GameVersion = game_version.TryGetVersion(),
                Loader = loader,
                ModVersion = last_version.ModVersion
            };

        }
    }

    public class MatchedMod : Mod
    {
        public Version GameVersion { get; set; }
        public string ModVersion { get; set; }
        public ModLoader Loader { get; set; }
        public string? LocalPath { get; set; }
        public ModVersionData? GetVersionData()
        {
            return VersionData.FirstOrDefault(vd => vd.ModVersion == ModVersion && vd.Loaders.Contains(Loader));
        }
    }

    public class ModReference
    {
        public string Name { get; set; }
        public string ReferenceId { get; set; }
        public DateTime ModUploadDate { get; set; }
        public Version GameVersion { get; set; }
        public string ModVersion { get; set; }
        public ModLoader Loader { get; set; }
        public string? LocalPath { get; set; }
    }
}