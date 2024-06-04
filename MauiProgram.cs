using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using IeuanWalker.Maui.StateButton;

namespace TesisAppSINMVVM
{
    // NAVEGACIÓN
    // EVENTOS
    // LOGICA PARA EVENTOS
    // LÓGICA
    // BASE DE DATOS
    // LÓGICA DE COSAS VISUALES DE LA PÁGINA
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseStateButton()
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
