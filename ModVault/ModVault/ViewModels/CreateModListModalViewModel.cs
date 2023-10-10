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
    public class loader
    {
        public string name { get; set; }
    }
    public partial class CreateModListModalViewModel : ObservableObject
    {
        public CreateModListModalViewModel()
        {
            modloaders = new List<string>() { "Fabric", "Forge", "Quilt"};

        }

        [ObservableProperty]
        public List<string> modloaders;




    }
}
