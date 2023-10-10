using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ModVault.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ModVault.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            SelectedModList = CoreManager.GetActiveModList();
            Init();
        }

        [ObservableProperty]
        ObservableCollection<MatchedMod> items;

        [ObservableProperty]
        ModList selectedModList;


        // Using mvvm toolkit instead of raw for clean code
        [ObservableProperty]
        string text;

        [RelayCommand]
        async Task Add()
        {
            if (string.IsNullOrWhiteSpace(Text))
                return;

            // Get mod information
            var mod = await CoreManager.FindModByVersionAndLoader(Text, SelectedModList.loader, SelectedModList.game_version.ToString());
            if (mod == null)
            {
                Text = string.Empty;
                return;
            }

            if(SelectedModList.mods.Any(i => i.ReferenceId == mod.ReferenceId))
            {
                // Already exists
                Text = string.Empty;
                return;
            }

            SelectedModList.mods.Add(mod);
            await ModListManager.CreateModList(SelectedModList);
            Items.Add(mod);

            Text = string.Empty;
        }

        async Task Init()
        {
            Items = new ObservableCollection<MatchedMod>();
            foreach (var mod in SelectedModList.mods)
            {
                Items.Add(mod);
            }
        }

        [RelayCommand]
        async Task Delete(string s)
        {
            var itemToRemove = SelectedModList.mods.FirstOrDefault(i => i.Name == s);
            SelectedModList.Delete(s);
            if (itemToRemove != null)
            {
                Items.Remove(itemToRemove);
                await ModListManager.CreateModList(SelectedModList);
            }
        }

        [RelayCommand]
        public async Task UpdateMods()
        {
            var modList = SelectedModList;
            if (modList == null)
                return;
            foreach (var mod in modList.mods)
            {
                // download the mods
                await CoreManager.DownloadMod(mod);
            }
            return;
        }

        [RelayCommand]
        async Task StageModFolder()
        {
            var modList = SelectedModList;
            if (modList == null)
                return;
            // Stage staging folder to minecraft mods folder
            var minecraft_game_folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft");
            var mod_folder = Path.Combine(minecraft_game_folder, "mods");

            // Check for existing folder in .minecraft/mods
            if(Directory.Exists(mod_folder))
            {
                var mod_folder_info = new DirectoryInfo(mod_folder);
                if (mod_folder_info.LinkTarget != null)
                {
                    Directory.Delete(mod_folder);
                }
                else
                {
                    var mod_folder_backup_name = mod_folder + ".old";
                    // If exist: Move to mods.old
                    if (Directory.Exists(mod_folder_backup_name))
                        Directory.Delete(mod_folder_backup_name);
                    Directory.Move(mod_folder, mod_folder_backup_name);
                }
            }

            // Create hardlink to our folder
            Directory.CreateSymbolicLink(mod_folder, modList.mod_path);
        }
    }
}
