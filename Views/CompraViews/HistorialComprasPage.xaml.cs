using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using TesisAppSINMVVM.FirebaseDataBase.Repositories;
using TesisAppSINMVVM.LocalDatabase.Tables;
using TesisAppSINMVVM.Models;
using TesisAppSINMVVM.Models.Groups;

namespace TesisAppSINMVVM.Views.CompraViews;

public partial class HistorialComprasPage : ContentPage
{
    private ProductoComprado_Repository _repoProductoComprado = new ProductoComprado_Repository();
    private List<ProductoCompradoGroup> _grupoProductoComprado { get; set; } = new List<ProductoCompradoGroup>();
    private string _nombreProveedor;
    //public List<ProductoCompradoGroup> _productosComprados { get; private set; } = new List<ProductoCompradoGroup>();
    private bool _enEjecucion;

    public HistorialComprasPage()
	{
		InitializeComponent();
    }


    // NAVEGACI�N
    #region NAVEGACI�N
    private async Task HistorialComprasPagePopAsync()
    {
        await Navigation.PopAsync();
    }
    private void NavegarPaginaPrincipalPageAsync()
    {
        var stack = Navigation.NavigationStack.ToArray();
        for (int i = 2; i < stack.Length; i++)
        {
            Navigation.RemovePage(stack[i]);
        }
    }
    private async Task RegistrarNuevaCompraPagePushAsync()
    {
        NetworkAccess accessType = Connectivity.Current.NetworkAccess;
        if (accessType == NetworkAccess.Internet)
        {
            await NavegacionRegistrarNuevaCompraPage();
        }
        else
        {
            await Toast.Make("Primero debe conectarse a internet", ToastDuration.Long).Show();
        }
    }
    #endregion
    

    // EVENTOS
    #region EVENTOS
    private async void ContentPage_Appearing(object sender, EventArgs e)
    {
        base.OnAppearing();
        await CargarNombreProveedor();
        await CargarDatosCollectionView_HistorialCompras();
    }
    private async void Button_Regresar_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await HistorialComprasPagePopAsync();
        _enEjecucion = false;
    }
    private async void Button_NavegarAgregarNuevaCompra_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await RegistrarNuevaCompraPagePushAsync();
        _enEjecucion = false;
    }
    private void ImageButton_Home_Clicked(object sender, EventArgs e)
    {
        NavegarPaginaPrincipalPageAsync();
    }
    #endregion

    // L�GICA PARA EVENTOS
    #region L�GICA PARA EVENTOS
    private async Task CargarNombreProveedor()
    {
        Proveedor proveedor = (Proveedor)this.BindingContext;
        _nombreProveedor = proveedor.PROVEEDOR;
    }
    private async Task CargarDatosCollectionView_HistorialCompras()
    {
        List<Tbl_ProductoComprado> productosComprados = await ObtenerProductosCompradosDBAsync(_nombreProveedor);
        if (productosComprados.Count == 0)
        {
            VerticalStackLayout_EmptyView.IsVisible = true;
            CollectionView_HistorialCompras.IsVisible = false;
        }
        else
        {
            VerticalStackLayout_EmptyView.IsVisible = false;

            var gruposFechaEmision = productosComprados.OrderByDescending(c => DateTime.Parse(c.FECHAGUARDADO))
                .GroupBy(c => c.DIAFECHAGUARDADO)
                .Select(g => new ProductoCompradoGroup(
                    g.Key,
                    g.OrderByDescending(c => c.NUMEROCOMPRA).ToList()
                ));
            _grupoProductoComprado.Clear();
            foreach (var grupo in gruposFechaEmision)
            {
                _grupoProductoComprado.Add(grupo);
            }

            CollectionView_HistorialCompras.ItemsSource = _grupoProductoComprado.ToList();
        }
    }
    #endregion

    // L�GICA
    #region L�GICA
    private async Task NavegacionRegistrarNuevaCompraPage()
    {
        var stack = Navigation.NavigationStack.ToArray();
        var lastPage = stack[stack.Length - 2];
        if (lastPage is RegistrarNuevaCompraPage)
        {
            await Navigation.PopAsync();
        }
        else if (lastPage is CompraOpcionesPage)
        {
            await Navigation.PushAsync(new RegistrarNuevaCompraPage
            {
                BindingContext = this.BindingContext
            });
            Navigation.RemovePage(stack[stack.Length - 1]);
        }
    }
    #endregion

    // BASE DE DATOS
    #region BASE DE DATOS
    private async Task<List<Tbl_ProductoComprado>> ObtenerProductosCompradosDBAsync(string proveedor)
    {
        return await _repoProductoComprado.ObtenerProductosCompradosAsync(proveedor);
    }


    #endregion

    // L�GICA DE COSAS VISUALES DE LA P�GINA
    #region L�GICA DE COSAS VISUALES DE LA P�GINA

    #endregion
}