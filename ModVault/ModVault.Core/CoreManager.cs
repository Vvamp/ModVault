using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Akavache;
using ModVault.Core.Modrinth;
using System.Text.Json;
namespace ModVault.Core
{
    public static class CoreManager
    {
        public static string dataFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ModVault");
        public static string cacheFolderPath = Path.Combine(dataFolderPath, "Cache");
        public static string ModListFolderPath = Path.Combine(dataFolderPath, "ModLists");
        public static string modDownloadPath = Path.Combine(dataFolderPath, "ModDownloads");
        public static string[] localFolders = {dataFolderPath, cacheFolderPath, modDownloadPath, ModListFolderPath};
        
        static CoreManager()
        {
            VerifyApplicationFolders();
        }

        public static async Task<Mod?> FindModById(string mod_id)
        {
            var modrinth_mod = await new ModrinthConnector().TryGetMod(mod_id);
            return modrinth_mod;
        }

        public static async Task<MatchedMod?> FindModByVersionAndLoader(string mod_id, ModLoader mod_loader, string minecraft_version)
        {
            var potential_mod = await FindModById(mod_id);
            if (potential_mod == null)
            {
                return null;
            }

            return potential_mod.WithLoaderAndVersion(mod_loader, minecraft_version);            
        }

        public static async Task DownloadMod(MatchedMod mod)
        {
            var primary_file = mod.GetVersionData()?.PrimaryFile;
            var mod_list = await GetActiveModListAsync();
          
            if (primary_file == null || mod_list == null)
            {
                return; //throw error
            }
            await VerifyApplicationFolders();

            var target_path = Path.Combine(mod_list.mod_path, primary_file.filename);

            using (var client = new HttpClient())
            {
                using (var s = await client.GetStreamAsync(primary_file.url))
                {
                    // If already exists: delete 
                    if (System.IO.File.Exists(target_path))
                        System.IO.File.Delete(target_path);

                    using (var fs = new FileStream($"{target_path}", FileMode.CreateNew))
                    {
                        s.CopyTo(fs);
                        mod.LocalPath = target_path;
                    }
                }
            }
        }

        [Obsolete("Functionality merged into DownloadMod")]
        public static async Task StageMod(MatchedMod mod)
        {
            var mod_list = await GetActiveModListAsync();
            if (mod_list == null)
                return;

            var staging_folder = mod_list.mod_path;
            if (mod.LocalPath == null || System.IO.File.Exists(mod.LocalPath) == false)
                await DownloadMod(mod);

            var target_path = Path.Combine(staging_folder, mod.GetVersionData()?.PrimaryFile.filename);
            try
            {
                System.IO.File.Move(mod.LocalPath, target_path);
            }catch(Exception ex)
            {
                return; // Show error
            }
            mod.LocalPath = target_path;

        }

        public static async Task DeleteMod(MatchedMod mod)
        {
            var mod_list = await GetActiveModListAsync();
            if (mod_list == null)
                return;

            if(System.IO.File.Exists(mod.LocalPath))
                System.IO.File.Delete(mod.LocalPath);
        }

        public static async Task VerifyApplicationFolders()
        {
            foreach(var localFolder in localFolders)
            {
                if (!Directory.Exists(localFolder))
                {
                    Directory.CreateDirectory(localFolder);
                }
            }
            var current_mod_list = await GetActiveModListAsync();

            if (current_mod_list != null && !Directory.Exists(current_mod_list.mod_path))
                Directory.CreateDirectory(current_mod_list.mod_path);
        }

        public static async Task<ModList?> GetActiveModListAsync()
        {
            string modListName;
            try
            {
                modListName = await BlobCache.UserAccount.GetObject<string>("CurrentModList");
                // retrieve mod list
                return await ModListManager.GetModListByName(modListName);
            }
            catch(Exception ex) 
            {
                return null;
            }

        }

        public static ModList? GetActiveModList()
        {
            var t = Task.Run(async () =>
            {
                return await GetActiveModListAsync();
            });

            return t.Result;

        }

        public static async Task<bool> SetActiveModList(ModList modList)
        {
            try
            {
                await BlobCache.UserAccount.InsertObject("CurrentModList", modList.name);
            }catch(Exception ex)
            {
                return false;
            }


            return true;
            
        }
    }
}
