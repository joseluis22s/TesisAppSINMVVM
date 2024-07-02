﻿using TesisAppSINMVVM.FirebaseDataBase;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using TesisAppSINMVVM.LocalDatabase;
using TesisAppSINMVVM.Database.Respositories;

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
            //Tbl_Cheque_Repository _repoCheque =new Tbl_Cheque_Repository();
            //Tbl_HistorialCompras_Repository _repoHistorialCompras = new Tbl_HistorialCompras_Repository();
            SincronizarBD sincronizarBD = new SincronizarBD();
            //ServicioFirestore firebaseDataBase = new ServicioFirestore();
            // TODO: Eliminar este comentario
            //firebaseDataBase.DesactivarUmbralCache();
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                }).UseMauiCommunityToolkit();
            //_repoCheque.BorarTbl_Cheque();
            //_repoHistorialCompras.BorrarTblHistorialProductosAsync();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
