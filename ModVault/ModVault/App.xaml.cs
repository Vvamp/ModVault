using Akavache;

namespace ModVault;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}

    protected override void OnSleep()
    {
        BlobCache.Shutdown().Wait();
        base.OnSleep();
    }

    protected override void OnStart()
    {
        Akavache.Registrations.Start("ModVault");
        base.OnStart();
    }

}
