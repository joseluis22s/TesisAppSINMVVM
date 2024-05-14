using TesisAppSINMVVM.Contents;
using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.Database.Tables;

namespace TesisAppSINMVVM.Views;

public partial class CompraPage : ContentPage
{

    private ContentView NuevaCompraView = new NuevaCompraContent();
    private ContentView HistorialComprasView = new HistorialComprasContent();
    private Tbl_Proveedor_Respository Tbl_Proveedor_repo = new Tbl_Proveedor_Respository();
    private List<Tbl_Proveedor> Proveedores;
    NavigationPage nav = new NavigationPage();

    public CompraPage()
    {
        InitializeComponent();
        ContentView_CompraPageContenidoDinamico.Content = NuevaCompraView;
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
    private async void Button_AgregarProveedor_Clicked(object sender, EventArgs e)
    {
        await AgregarNuevoProveedorPagePushModalAsync();
        
    }
    //private async void Button_Borrar_tbl_Clicked(object sender, EventArgs e)
    //{
    //    await BorrarTblProveedorDBAsync();
    //}
    private async void ContentPage_Appearing(object sender, EventArgs e)
    {
        base.OnAppearing();
        Proveedores = await ObtenerProveedoresDBAsync();
        CollectionView_Proveedores.ItemsSource = Proveedores;
        bool vertical1 = Grid_ProveedoresCompraPage.IsVisible;
    }
    private void CollectionView_Proveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var item = CollectionView_Proveedores.SelectedItem;
        Grid_ProveedoresCompraPage.IsVisible = false;
        VerticalStackLayout_OpcionesCompraPage.IsVisible = true;
        ContentView_CompraPageContenidoDinamico.BindingContext = item;
    }
    private void Button_CancelarProveedores_Clicked(object sender, EventArgs e)
    {
        VerticalStackLayout_OpcionesCompraPage.IsVisible = false;
        Grid_ProveedoresCompraPage.IsVisible = true;
        //CollectionView_Proveedores.SelectedItem = null;
    }


    // LÓGICA


    // BASE DE DATOS
    private async Task<List<Tbl_Proveedor>> ObtenerProveedoresDBAsync()
    {
        return await Tbl_Proveedor_repo.ObtenerProveedores();
    }
    private async Task BorrarTblProveedorDBAsync()
    {
        await Tbl_Proveedor_repo.BorrarTblProveedorAsync();
    }

    ////LÓGICA DE COSAS VISUALES DE LA PÁGINA
}