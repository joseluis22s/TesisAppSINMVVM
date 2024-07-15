using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using TesisAppSINMVVM.FirebaseDataBase.Repositories;
using TesisAppSINMVVM.Models;
using TesisAppSINMVVM.Views.VentaViews.BodegaViews;

namespace TesisAppSINMVVM.Views.VentaViews;

public partial class HistorialProductoSobrantePage : ContentPage
{
    private List<ProductoInventarioBodegaGroup> _grupoInventario {  get; set; } = new List<ProductoInventarioBodegaGroup>();
    private bool _enEjecucion;
    public HistorialProductoSobrantePage()
	{
		InitializeComponent();
    }
    // TODO: Aqui quitar _enEjecucion;


    // NAVEGACIÓN
    #region NAVEGACIÓN
    private async Task HistorialProductoSobrantePagePopAsync()
    {
        //await PagePopAsync();
        await Navigation.PopAsync();
    }
    #endregion

    // EVENTOS
    #region EVENTOS
    private async void ContentPage_Appearing(object sender, EventArgs e)
    {
        base.OnAppearing();
        await CargarDatosCollectionView_Bodega();
    }
    private async void Button_NavegarAgregarNuevoRegistroProductoSobrante_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        NetworkAccess accessType = Connectivity.Current.NetworkAccess;
        if (accessType == NetworkAccess.Internet)
        {
            await NavegacionRegistrarProductoSobranteBodegaPage();
        }
        else
        {
            await Toast.Make("Primero debe conectarse a internet", ToastDuration.Long).Show();
        }
        _enEjecucion = false;
    }
    private async void Button_Regresar_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        RegistrarProductoSobranteBodegaPage._ejecutarAppearing = false;
        await HistorialProductoSobrantePagePopAsync();
        _enEjecucion = false;
    }
    protected override bool OnBackButtonPressed()
    {
        RegistrarProductoSobranteBodegaPage._ejecutarAppearing = false;
        HistorialProductoSobrantePagePopAsync().GetAwaiter();
        return true;
    }
    private async void ImageButton_Home_Clicked(object sender, EventArgs e)
    {
        await NavegarPaginaPrincipalPagePopToRootAsync();
    }
    #endregion

    // LÓGICA PARA EVENTOS
    #region LÓGICA PARA EVENTOS
    private async Task CargarDatosCollectionView_Bodega()
    {
        List<ProductoInventarioBodega> inventario = await ObtenerInvetarioDBAsync();
        if (inventario.Count == 0)
        {
            VerticalStackLayout_EmptyView_HistorialProductoSobrante.IsVisible = true;
            CollectionView_Bodega.IsVisible = false;
        }
        else
        {
            VerticalStackLayout_EmptyView_HistorialProductoSobrante.IsVisible = false;
            var gruposInvetarioProductos = inventario.OrderByDescending(c => DateTime.Parse(c.FECHAGUARDADO))
            .GroupBy(_grs => _grs.DIAFECHAGUARDADO)
            .Select(g => new ProductoInventarioBodegaGroup(g.Key, g.ToList()));

            _grupoInventario.Clear();
            foreach (var grupo in gruposInvetarioProductos)
            {
                _grupoInventario.Add(grupo);
            }
            CollectionView_Bodega.ItemsSource = _grupoInventario;
        }
        //var gruposInvetarioProductos = inventario.OrderByDescending(c => DateTime.Parse(c.FECHAGUARDADO))
        //    .GroupBy(_grs => _grs.DIAFECHAGUARDADO)
        //    .Select(g => new ProductoInventarioBodegaGroup(g.Key, g.ToList()));

        //_grupoInventario.Clear();
        //foreach (var grupo in gruposInvetarioProductos)
        //{
        //    _grupoInventario.Add(grupo);
        //}
        //CollectionView_Bodega.ItemsSource = _grupoInventario;
    }
    private async Task NavegarPaginaPrincipalPagePopToRootAsync()
    {
        await Navigation.PopToRootAsync();
    }
    #endregion

    // LÓGICA
    #region LÓGICA
    private async Task NavegacionRegistrarProductoSobranteBodegaPage()
    {
        var stack = Navigation.NavigationStack.ToArray();
        var lastPage = stack[stack.Length - 2];
        if (lastPage is RegistrarProductoSobranteBodegaPage)
        {
            await Navigation.PopAsync();
        }
        else if (lastPage is OpcionesBodegaPage)
        {
            await Navigation.PushAsync(new RegistrarProductoSobranteBodegaPage
            {
                BindingContext = this.BindingContext
            });
            Navigation.RemovePage(stack[stack.Length - 1]);
        }
    }
    private Task PagePopAsync()
    {
        RegistrarProductoSobranteBodegaPage._ejecutarAppearing = false;
        Dispatcher.Dispatch(async () =>
        {
            bool respuesta = await DisplayAlert("Alerta", "¿Desea regresar? Perderá el progreso realizado", "Confimar", "Cancelar");
            if (respuesta)
            {
                await Navigation.PopAsync();
            }
        });
        return Task.CompletedTask;
    }
    #endregion

    // BASE DE DATOS
    #region BASE DE DATOS
    private async Task<List<ProductoInventarioBodega>> ObtenerInvetarioDBAsync()
    {
        return await ProductoInventarioBodega_Repository.ObtenerProductosInventarioAsync();
    }

    #endregion

    // LÓGICA DE COSAS VISUALES DE LA PÁGINA
    #region LÓGICA DE COSAS VISUALES DE LA PÁGINA

    #endregion
}