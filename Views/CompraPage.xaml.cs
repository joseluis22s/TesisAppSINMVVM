using TesisAppSINMVVM.Contents;
using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.Database.Tables;

namespace TesisAppSINMVVM.Views;

public partial class CompraPage : ContentPage
{

    private ContentView _nuevaCompraContentView;
    private ContentView _historialComprasContentView;
    private Tbl_Proveedor_Respository _repoProveedor;
    private List<Tbl_Proveedor> _proveedores;
    //private NavigationPage _navigationPage;

    public CompraPage()
    {
        InitializeComponent();
        _nuevaCompraContentView = new NuevaCompraContentView();
        _historialComprasContentView = new HistorialComprasContentView();
        _repoProveedor = new Tbl_Proveedor_Respository();
        //_navigationPage = new NavigationPage();
        ContentView_CompraPageContenidoDinamico.Content = _nuevaCompraContentView;
    }


    // NAVEGACIÓN
    private async void AgregarNuevoProveedorPagePushModalAsync()
    {
        await Navigation.PushModalAsync(new AgregarNuevoProveedorPage());
    }


    // EVENTOS
    private void Button_NuevaCompra_Clicked(object sender, EventArgs e)
    {
        ContentView_CompraPageContenidoDinamico.Content = _nuevaCompraContentView;
    }
    private void Button_Historial_Clicked(object sender, EventArgs e)
    {
        ContentView_CompraPageContenidoDinamico.Content = _historialComprasContentView;
    }
    private void Button_AgregarProveedor_Clicked(object sender, EventArgs e)
    {
        AgregarNuevoProveedorPagePushModalAsync();
    }
    private async void Button_Borrar_tblProveedor_Clicked(object sender, EventArgs e)
    {
        await BorrarTblProveedorDBAsync();
    }
    private void ContentPage_Appearing(object sender, EventArgs e)
    {
        base.OnAppearing();

        CargarDatosCollectionView_Proveedores();
    }
    private void CollectionView_Proveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        PasarProveedorBinding();
        MostarVerticalStackLayout_OpcionesCompraPage();
    }
    private void Button_RegresarProveedores_Clicked(object sender, EventArgs e)
    {
        MostrarGrid_ProveedoresCompraPage();
    }


    // LÓGICA
    private async void CargarDatosCollectionView_Proveedores()
    {
        _proveedores = await ObtenerProveedoresDBAsync();
        CollectionView_Proveedores.ItemsSource = _proveedores;
    }
    private void PasarProveedorBinding()
    {
        var item = CollectionView_Proveedores.SelectedItem;
        ContentView_CompraPageContenidoDinamico.BindingContext = item;
    }


    // BASE DE DATOS
    private async Task<List<Tbl_Proveedor>> ObtenerProveedoresDBAsync()
    {
        return await _repoProveedor.ObtenerProveedores();
    }
    private async Task BorrarTblProveedorDBAsync()
    {
        await _repoProveedor.BorrarTblProveedorAsync();
    }


    // LÓGICA DE COSAS VISUALES DE LA PÁGINA
    private void MostarVerticalStackLayout_OpcionesCompraPage()
    {
        Grid_ProveedoresCompraPage.IsVisible = false;
        VerticalStackLayout_OpcionesCompraPage.IsVisible = true;
    }
    private void MostrarGrid_ProveedoresCompraPage()
    {
        VerticalStackLayout_OpcionesCompraPage.IsVisible = false;
        Grid_ProveedoresCompraPage.IsVisible = true;
    }
}