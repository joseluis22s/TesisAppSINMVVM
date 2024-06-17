using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;

namespace TesisAppSINMVVM
{
    // NAVEGACIÓN
    #region NAVEGACIÓN

    #endregion

    // EVENTOS
    #region EVENTOS

    #endregion

    // LÓGICA PARA EVENTOS
    #region LÓGICA PARA EVENTOS

    #endregion

    // LÓGICA
    #region LÓGICA

    #endregion

    // BASE DE DATOS
    #region BASE DE DATOS

    #endregion

    // LÓGICA DE COSAS VISUALES DE LA PÁGINA
    #region LÓGICA DE COSAS VISUALES DE LA PÁGINA

    #endregion

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
