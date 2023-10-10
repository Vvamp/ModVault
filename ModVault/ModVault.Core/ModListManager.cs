using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Xml.Linq;
using System.Diagnostics;

namespace ModVault.Core
{
    public static class ModListManager
    {
        public static async Task<ModList?> GetModListByName(string name)
        {
            var mod_list_path = Path.Combine(CoreManager.ModListFolderPath, name);
            var mod_list_info_file = Path.Combine(mod_list_path, "metadata.json");
            var mod_files = Path.Combine(mod_list_path, "mods");
            if (Directory.Exists(mod_list_path) == false)
                return null;

            if (System.IO.File.Exists(mod_list_info_file) == false)
                return null;

            var meta_data_raw = await System.IO.File.ReadAllTextAsync(mod_list_info_file);
            var mod_list = JsonSerializer.Deserialize<ModList>(meta_data_raw);
            return mod_list;
        }

        public static async Task CreateModList(ModList list)
        {
            await CoreManager.VerifyApplicationFolders();
            var mod_list_path = Path.Combine(CoreManager.ModListFolderPath, list.name);
            var mod_list_info_file = list.metadata_file_path;
            var mod_files = list.mod_path;
            if (Directory.Exists(mod_list_path) == false)
                Directory.CreateDirectory(mod_files);

            var meta_data_raw = JsonSerializer.Serialize(list);
            await System.IO.File.WriteAllTextAsync(mod_list_info_file, meta_data_raw);
        }

        public static async Task<List<ModList>> GetModListsAsync()
        {
            List<ModList> list = new List<ModList>();
            string[] potential_modlist_folders;
            // List folders in modlist path
            try
            {
                potential_modlist_folders = Directory.GetDirectories(CoreManager.ModListFolderPath);
            }catch(Exception ex)
            {
                return list;
            }


            // For each folder, check for a metadata.json
            // Read and create
            foreach(var potential_modlist_folder in potential_modlist_folders)
            {
                var potential_modlist_folder_name = potential_modlist_folder.Split(Path.DirectorySeparatorChar).Last();
                var mod = await GetModListByName(potential_modlist_folder_name);
                if (mod != null)
                {
                    list.Add(mod);
                }
               
            }

            return list;
        }

        public static List<ModList> GetModLists()
        {
            var t = Task.Run(async () =>
            {
                return await GetModListsAsync();
            });

            return t.Result;
        }

    }
}
