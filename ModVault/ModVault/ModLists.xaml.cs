using ModVault.ViewModels;

namespace ModVault;

public partial class ModLists : ContentPage
{
	public ModLists(ModListsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}