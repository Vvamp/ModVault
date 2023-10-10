using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ModVault.Core
{
    public class ModList
    {
        public ModList(string name, ModLoader loader, Version game_version)
        {
            this.name = name;
            this.loader = loader;
            this.game_version = game_version;
        }

        public string name { get; set; }
        public ModLoader loader { get; set; }
        public Version game_version { get; set; }

        public List<MatchedMod> mods { get; set; } = new List<MatchedMod>();

        // Add list of mods currently in modlist (with filename, mod name, author, etc).
        // Maybe change up the Mods class or add a new one with less info. I made a ModReference class. Look at it with fresh eyes
        // On download: download latest valid version of those files
        // When listing mods, show ALL mods in mods folder. Make missing mods(in list but not folder) red. Files not in modlist but are in mods folder show up with source 'local' and name 'filename'
        
        public string mod_path { 
            get {
                var mod_list_path = Path.Combine(CoreManager.ModListFolderPath, this.name);
                var mod_path = Path.Combine(mod_list_path, "mods");
                return mod_path;
            
            } 
        }

        public string metadata_file_path
        {
            get
            {
                var mod_list_path = Path.Combine(CoreManager.ModListFolderPath, this.name);
                var mod_list_info_file = Path.Combine(mod_list_path, "metadata.json");
                return mod_list_info_file;

            }
        }
        public void Delete(string mod_name)
        {
            var itemToRemove = mods.FirstOrDefault(i => i.Name == mod_name);
            CoreManager.DeleteMod(itemToRemove);
            mods.Remove(itemToRemove);
        }
    }

    

}
