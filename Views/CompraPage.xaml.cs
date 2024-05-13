using System.Collections.ObjectModel;
using TesisAppSINMVVM.Contents;
using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.Database.Tables;

namespace TesisAppSINMVVM.Views;

public partial class CompraPage : ContentPage
{
    private ContentView NuevaCompraView { get; set; }
    private ContentView HistorialComprasView { get; set; }
    private Tbl_Proveedor_Respository Tbl_Proveedor_repo;
    private List<Tbl_Proveedor> Proveedores;
    public CompraPage()
	// AQUI INICIA
    {
		InitializeComponent();
        //InicializacionAsync();
        NuevaCompraView = new NuevaCompraContent();
        HistorialComprasView = new HistorialComprasContent();
        ContentView_CompraPageContenidoDinamico.Content = NuevaCompraView;
        Tbl_Proveedor_repo = new Tbl_Proveedor_Respository();
        CollectionView_Proveedores.ItemsSource = Proveedores;
    }

    // NAVEGACIÓN
    private async Task AgregarNuevoProveedorPagePushModalAsync()
    {
        await Navigation.PushModalAsync(new AgregarNuevoProveedorPage());
    }
    // EVENTOS
    private void Button_NuevaCompra_Clicked(object sender, EventArgs e)
    {
        ContentView_CompraPageContenidoDinamico.Content = NuevaCompraView;
    }
    private void Button_Historial_Clicked(object sender, EventArgs e)
    {
        ContentView_CompraPageContenidoDinamico.Content = HistorialComprasView;
    }
    //private async void ContentPage_Loaded(object sender, EventArgs e)
    //{
    //    bool vertical1 = VerticalStackLayout_ProveedoresCompraPage.IsVisible;
    //    if (vertical1)
    //    {
    //        await ActualizarTitulo("PROVEEDORES");
    //    }
    //}
    private async void ContentPage_Appearing(object sender, EventArgs e)
    // AQUI APEAR
    {
        base.OnAppearing();

        bool vertical1 = Grid_ProveedoresCompraPage.IsVisible;
        if (vertical1)
        {
            await ActualizarTitulo("PROVEEDORES");
        }
        //Proveedores = await ObtenerProveedoresDBAsync();
        //CollectionView_Proveedores.ItemsSource = Proveedores;
    }
    private async void Button_AgregarProveedor_Clicked(object sender, EventArgs e)
    {
        await AgregarNuevoProveedorPagePushModalAsync();
    }
    private async void Button_Borrar_tbl_Clicked(object sender, EventArgs e)
    {
        await BorrarTblProveedorDBAsync();
    }

    // LOGICA PARA EVENTOS
    // LÓGICA
    private async Task ActualizarTitulo(string titulo)
    {
        await Task.Run(() => {
            Title = titulo;
        });
    }

    //private Task CargarProveedores()
    //{
    //    //CollectionView_Proveedores
    //}





    // BASE DE DATOS
    private async Task<List<Tbl_Proveedor>> ObtenerProveedoresDBAsync()
    {
        var a = await Tbl_Proveedor_repo.ObtenerProveedores();
        return a;
        //return await Tbl_Proveedor_repo.ObtenerProveedores();
    }
    private async Task BorrarTblProveedorDBAsync()
    {
        await Tbl_Proveedor_repo.BorrarTblProveedorAsync();
    }

    




    //private async Task<bool> VerificarExistenciaProveedoresAsync()
    //{

    //    // TODO: Primero crear la qyery para la BD, segundo verificar si existe , luego ver is hay y asi.
    //    //return await Tbl_Proveedor_repo.VerificarExistenciaProveedorAsync();
    //}

    // INICIALIZACIÓN ASYNC

    //private async Task InicializacionAsync()
    //{
    //    await Task.Run(() => {
    //        NuevaCompraView = new NuevaCompraContent();
    //        HistorialComprasView = new HistorialComprasContent();
    //        ContentView_CompraPageContenidoDinamico.Content = NuevaCompraView;
    //        Tbl_Proveedor_repo = new Tbl_Proveedor_Respository();
    //    });

    //}
    // LÓGICA DE COSAS VISUALES DE LA PÁGINA
}