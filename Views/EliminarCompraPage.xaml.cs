using Plugin.CloudFirestore;
using TesisAppSINMVVM.Contents;
using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.Database.Tables;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.Views;

public partial class EliminarCompraPage : ContentPage
{

    private Proveedor _proveedordd = new Proveedor();

    private ContentView _nuevaCompraContentView;
    private ContentView _historialComprasContentView;
    private Tbl_Proveedor_Respository _repoProveedor;
    private List<Tbl_Proveedor> _proveedores;
    private bool _enEjecucion;
    private bool _ejecutarSelectionChanged = true;
    private Tbl_Proveedor _proveedor;
    private NuevaCompraContentView _nuevaCompraView;

    public static bool _permitirEjecucion = true;
    public static bool _permitirContentView_BindingContextChanged = true;
    public EliminarCompraPage()
    {
        InitializeComponent();

        _nuevaCompraView = new NuevaCompraContentView();
        _nuevaCompraContentView = new NuevaCompraContentView();
        _historialComprasContentView = new HistorialComprasContentView();
        _repoProveedor = new Tbl_Proveedor_Respository();
        ContentView_CompraPageContenidoDinamico.Content = _nuevaCompraContentView;
    }


    // NAVEGACIÓN
    private async Task AgregarNuevoProveedorPagePushModalAsync()
    {
        await Navigation.PushModalAsync(new AgregarNuevoProveedorPage());
    }


    // EVENTOS
    private async void ContentPage_Appearing(object sender, EventArgs e)
    {
        if (!_permitirEjecucion)
        {
            return;
        }
        await ObtenerProveedoresAsync();
        // Controlar que no se cargue todos los proveedores solo los faltantes
        base.OnAppearing();
        Title = "COMPRA: Proveedores";
        CargarDatosCollectionView_Proveedores();
        _permitirEjecucion = true;
    }
    private async void Button_AgregarProveedor_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await AgregarNuevoProveedorPagePushModalAsync();
        _enEjecucion = false;
    }
    private void CollectionView_Proveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_ejecutarSelectionChanged)
        {
            PasarProveedorBinding();
            Title = "COMPRA: Registar compra";
            HistorialComprasContentView._ejecutarBindingContextChanged = false;
            MostarVerticalStackLayout_OpcionesCompraPage();
        }
        _ejecutarSelectionChanged = true;
    }
    private void Button_NuevaCompra_Clicked(object sender, EventArgs e)
    {
        bool a = _nuevaCompraContentView == ContentView_CompraPageContenidoDinamico.Content;
        if (a)
        {

            return;
        }
        Title = "COMPRA: Registar compra";
        ContentView_CompraPageContenidoDinamico.Content = _nuevaCompraContentView;
    }
    private void Button_Historial_Clicked(object sender, EventArgs e)
    {
        bool a = _historialComprasContentView == ContentView_CompraPageContenidoDinamico.Content;
        if (a)
        {
            return;
        }
        Title = "COMPRA: Historial compras";
        HistorialComprasContentView._ejecutarBindingContextChanged = true;
        // VERIFICAR SI LUEGO ES NECESARIO CONTROLAR QUE NO SE EJECUTE DE NUEVO EN CASO DE PRESIONAR EL BOTÓN
        //PasarProveedorBinding();
        ContentView_CompraPageContenidoDinamico.Content = _historialComprasContentView;
    }
    private void Button_RegresarProveedores_Clicked(object sender, EventArgs e)
    {
        //if (_nuevaCompraContentView.IsVisible)
        //{
        //    _nuevaCompraView.OcultarTecladoVaciarCampos();
        //}
        MostrarGrid_ProveedoresCompraPage();
        _ejecutarSelectionChanged = false;
        ContentView_CompraPageContenidoDinamico.BindingContext = "";
        HistorialComprasContentView._ejecutarBindingContextChanged = false;
        CollectionView_Proveedores.SelectedItem = null;
    }
    private async void Button_Borrar_tblProveedor_Clicked(object sender, EventArgs e)
    {
        await BorrarTblProveedorDBAsync();
    }
    protected override bool OnBackButtonPressed()
    {
        Dispatcher.Dispatch(async () =>
        {
            bool a = Grid_ProveedoresCompraPage.IsVisible;
            if (a)
            {
                await PermitirPopAsyncNavegacion();
            }
            else
            {
                MostrarGrid_ProveedoresCompraPage();
            }
        });
        return true;
    }


    // LÓGICA
    private async void CargarDatosCollectionView_Proveedores()
    {
        _proveedores = await ObtenerProveedoresDBAsync();
        CollectionView_Proveedores.ItemsSource = _proveedores;
    }
    private void PasarProveedorBinding()
    {
        _proveedor = (Tbl_Proveedor)CollectionView_Proveedores.SelectedItem;
        HistorialComprasContentView._ejecutarBindingContextChanged = false;
        ContentView_CompraPageContenidoDinamico.BindingContext = _proveedor;
    }
    private async Task PermitirPopAsyncNavegacion()
    {
        bool respuesta = await DisplayAlert("Alerta", "¿Desea regresar?", "Confimar", "Cancelar");
        if (respuesta)
        {
            await Navigation.PopAsync();
        }
    }


    // BASE DE DATOS
    private async Task ObtenerProveedoresAsync()
    {
        var documento = await CrossCloudFirestore.Current
                                     .Instance
                                     .CollectionGroup("PROVEEDOR")
                                     .GetAsync();
        var proveedores = documento.ToObjects<Proveedor>();
    }
    private async Task<List<Tbl_Proveedor>> ObtenerProveedoresDBAsync()
    {
        return await _repoProveedor.ObtenerProveedoresAsync();
    }
    private async Task BorrarTblProveedorDBAsync()
    {
        await _repoProveedor.BorrarTblProveedorAsync();
    }


    // LÓGICA DE COSAS VISUALES DE LA PÁGINA
    private void MostarVerticalStackLayout_OpcionesCompraPage()
    {
        _ejecutarSelectionChanged = false;
        CollectionView_Proveedores.SelectedItem = null;
        HistorialComprasContentView._ejecutarBindingContextChanged = false;
        Grid_ProveedoresCompraPage.IsVisible = false;
        VerticalStackLayout_OpcionesCompraPage.IsVisible = true;
    }
    private void MostrarGrid_ProveedoresCompraPage()
    {
        VerticalStackLayout_OpcionesCompraPage.IsVisible = false;
        Grid_ProveedoresCompraPage.IsVisible = true;
    }

}