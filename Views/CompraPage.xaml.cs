using TesisAppSINMVVM.Contents;
using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.Database.Tables;

namespace TesisAppSINMVVM.Views;

public partial class CompraPage : ContentPage
{

    private ContentView _nuevaCompraContentView = new NuevaCompraContentView();
    private ContentView _historialComprasContentView = new HistorialComprasContentView();
    private Tbl_Proveedor_Respository _repoProveedor = new Tbl_Proveedor_Respository();
    private List<Tbl_Proveedor> _proveedores;
    private NavigationPage nav = new NavigationPage();

    public CompraPage()
    {
        InitializeComponent();
        ContentView_CompraPageContenidoDinamico.Content = _nuevaCompraContentView;
    }

    // NAVEGACIÓN
    private async Task AgregarNuevoProveedorPagePushModalAsync()
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

        await CargarDatosCollectionView_Proveedores();
        //_proveedores = await ObtenerProveedoresDBAsync();
        //CollectionView_Proveedores.ItemsSource = _proveedores;
        // *NO SE QUE ES* bool vertical1 = Grid_ProveedoresCompraPage.IsVisible;
    }
    private async void CollectionView_Proveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        //var item = CollectionView_Proveedores.SelectedItem;
        //Grid_ProveedoresCompraPage.IsVisible = false;
        //VerticalStackLayout_OpcionesCompraPage.IsVisible = true;
        //ContentView_CompraPageContenidoDinamico.BindingContext = item;

        await PasarProveedorBinding();
        await MostarVerticalStackLayout_OpcionesCompraPage();
    }
    private async void Button_CancelarProveedores_Clicked(object sender, EventArgs e)
    {
        //VerticalStackLayout_OpcionesCompraPage.IsVisible = false;
        //Grid_ProveedoresCompraPage.IsVisible = true;
        await MostrarGrid_ProveedoresCompraPage();
        // *NO SE PORQUE ESTA* CollectionView_Proveedores.SelectedItem = null;
    }


    // LÓGICA

    //      Verificar si se puede extender hasta la linea '79'
    //      Verificar si es necesario Task.Run
    private async Task CargarDatosCollectionView_Proveedores()
    {
        _proveedores = await ObtenerProveedoresDBAsync();
        await Task.Run(() =>
        {
            CollectionView_Proveedores.ItemsSource = _proveedores;
        });
    }
    //      Verificar si es necesario Task.Run
    private async Task PasarProveedorBinding()
    {
        
        await Task.Run(() =>
        {
            var item = CollectionView_Proveedores.SelectedItem;
            ContentView_CompraPageContenidoDinamico.BindingContext = item;
        });
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

    //      Verificar si es necesario Task.Run
    private async Task MostarVerticalStackLayout_OpcionesCompraPage()
    {
        await Task.Run(() =>
        {
            Grid_ProveedoresCompraPage.IsVisible = false;
            VerticalStackLayout_OpcionesCompraPage.IsVisible = true;
        });
    }
    //      Verificar si es necesario Task.Run
    private async Task MostrarGrid_ProveedoresCompraPage()
    {
        await Task.Run(() =>
        {
            VerticalStackLayout_OpcionesCompraPage.IsVisible = false;
            Grid_ProveedoresCompraPage.IsVisible = true;
        });
    }
}