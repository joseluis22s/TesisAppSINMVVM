﻿using TesisAppSINMVVM.FirebaseDataBase;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using TesisAppSINMVVM.LocalDatabase;

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
            //Tbl_Cheque_Repository _repoCheque = new Tbl_Cheque_Repository();
            //Tbl_HistorialCompras_Repository _repoHistorialCompras = new Tbl_HistorialCompras_Repository();
            //Tbl_Comprador_Repository _repoComprador = new Tbl_Comprador_Repository();   
            //Tbl_VentaCredito_Repository _repoVentaCrediot = new Tbl_VentaCredito_Repository();

            //Tbl_Producto_Repository _repotblProducto = new Tbl_Producto_Repository();
            //Tbl_ProductosInventario_Repository _repoTblProdcutoInvetario = new Tbl_ProductosInventario_Repository();
            //SincronizarBD sincronizarBD = new SincronizarBD();
            //ServicioFirestore firebaseDataBase = new ServicioFirestore();
            // TODO: Eliminar este comentario
            SincronizarBD sincDB = new SincronizarBD();
            sincDB.DesactivarUmbralCache();
            //ConexionInternet connIn = new ConexionInternet();
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Roboto-Regular.ttf", "RobotoRegular");
                    fonts.AddFont("Roboto-Thin.ttf", "RobotoThin");
                    fonts.AddFont("Roboto-Light.ttf", "RobotoLight");
                }).UseMauiCommunityToolkit();
            //_repoCheque.BorarTbl_Cheque();
            //_repoHistorialCompras.BorrarTblHistorialProductosAsync();
            //_repoComprador.BorrarTblCompradorAsync();
            //_repoVentaCrediot.BorrarTblVentaCredito();

            //_repotblProducto.BorrarTablaTbl_ProductoAsync();
            //_repoTblProdcutoInvetario.BorrarTblProductoInveario();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
