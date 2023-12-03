using Microsoft.Extensions.Logging;
using Pear.Client.Repositories;

namespace Pear.Client
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            string dbPath = System.IO.Path.Combine(FileSystem.AppDataDirectory, "data.db");
            builder.Services.AddSingleton(p => ActivatorUtilities.CreateInstance<PearFriendsRepository>(p, dbPath));

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
