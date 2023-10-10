using Microsoft.Extensions.Logging;
using ModVault.ViewModels;

namespace ModVault;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
        Akavache.Registrations.Start("ModVault");

        var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("lineicons.ttf", "LineIcons");
			});

		builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<MainViewModel>();
       // builder.Services.AddSingleton<MainViewModel>();
		builder.Services.AddSingleton<ModListsViewModel>();
		builder.Services.AddSingleton<ModLists>();


#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}


}
