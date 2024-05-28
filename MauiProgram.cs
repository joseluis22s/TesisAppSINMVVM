using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;

namespace TesisAppSINMVVM
{
    // NAVEGACIÓN
    // EVENTOS
    // LOGICA PARA EVENTOS
    // BASE DE DATOS
    // LÓGICA DE COSAS VISUALES DE LA PÁGINA
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                }).UseMauiCommunityToolkit();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
