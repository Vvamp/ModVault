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

namespace ModVault.ViewModels
{
    public partial class ModListsViewModel : ObservableObject
    {
        public ModListsViewModel()
        {
            Modlists = new ObservableCollection<ModList>(ModListManager.GetModLists());
        }

        [ObservableProperty]
        ObservableCollection<ModList> modlists;

        [RelayCommand]
        async Task Delete(string s)
        {
            var itemToRemove = Modlists.FirstOrDefault(i => i.name == s);
            if (itemToRemove != null)
            {
                Modlists.Remove(itemToRemove);
            }
        }

        [RelayCommand]
        async Task CreateModList()
        {
            await Shell.Current.GoToAsync("//CreateModListModal");
        }


    }
}
