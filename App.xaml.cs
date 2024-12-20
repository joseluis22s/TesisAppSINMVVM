﻿using TesisAppSINMVVM.Views;

namespace TesisAppSINMVVM
{
    public partial class App : Application
    {
        //private Tbl_Producto_Repository _repoProducto1 = new Tbl_Producto_Repository();
        //private Tbl_Proveedor_Respository _repoProveedor1 = new Tbl_Proveedor_Respository();
        //private Tbl_Usuario_Repository _repoUsuario 1= new Tbl_Usuario_Repository();
        //private Tbl_Cheque_Repository _repoCheque = new Tbl_Cheque_Repository();


        public App()
        {
            UserAppTheme = AppTheme.Light;
            // BORRAR DATOS
            //_repoProducto1.BorrarTablaTbl_ProductoAsync();
            //_repoProveedor1.BorrarTblProveedorAsync();
            //_repoCheque.BorarTbl_Cheque();


            InitializeComponent();

            //MainPage = new AppShell();}
            MainPage = new NavigationPage(new IniciarSesionPage());
            //MainPage = new NavigationPage(new PaginaPrincipalPage());

            // CREAR DATOS
            //List<Tbl_Proveedor> listaTbl_Proveedor = new List<Tbl_Proveedor>()
            //{
            //    new Tbl_Prov
            //    eedor
            //    {
            //        NOMBRE = "Nombre_1",
            //        APELLIDO = "Apellido_1",
            //        TELEFONO = "0912345678"
            //    },
            //    new Tbl_Proveedor
            //    {
            //        NOMBRE = "Nombre_2",
            //        APELLIDO = "Apellido_2",
            //        TELEFONO = "0999999999"
            //    },
            //    new Tbl_Proveedor
            //    {
            //        NOMBRE = "Nombre_3",
            //        APELLIDO = "Apellido_3",
            //        TELEFONO = "0987654321"
            //    }
            //};

            //List<Tbl_Producto> listaTbl_Producto = new List<Tbl_Producto>()
            //{
            //    new Tbl_Producto
            //    {
            //        PRODUCTO = "Producto_1"
            //    },
            //    new Tbl_Producto
            //    {
            //        PRODUCTO = "Producto_2"
            //    },
            //    new Tbl_Producto
            //    {
            //        PRODUCTO = "Producto_3"
            //    }
            //};

            // GUARDAR DATOS
            //_repoProveedor1.GuardarTodoNuevoProveedorAsync(listaTbl_Proveedor);
            //_repoProducto1.GuardarTodoProductoAsync(listaTbl_Producto);





        }
    }
}
