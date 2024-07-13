using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Linq;
using System.Runtime.InteropServices;
using TesisAppSINMVVM.FirebaseDataBase.Repositories;
using TesisAppSINMVVM.LocalDatabase.Tables;
using TesisAppSINMVVM.Models;
using TesisAppSINMVVM.Models.Groups;

namespace TesisAppSINMVVM.Views.CompraViews;

public partial class HistorialComprasPage : ContentPage
{
    private ProductoComprado_Repository _repoProductoComprado = new ProductoComprado_Repository();
    private string _nombreProveedor;
    public List<ProductoCompradoGroup> _productosComprados { get; private set; } = new List<ProductoCompradoGroup>();
    public List<HistorialComprasGroup> _historialCompras { get; private set; } = new List<HistorialComprasGroup>();

    public HistorialComprasPage()
	{
		InitializeComponent();
    }


    // NAVEGACIÓN
    #region NAVEGACIÓN
    private async Task HistorialComprasPagePopAsync()
    {
        await Navigation.PopAsync();
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
        await HistorialComprasPagePopAsync();
    }
    private async void Button_NavegarAgregarNuevaCompra_Clicked(object sender, EventArgs e)
    {
        await RegistrarNuevaCompraPagePushAsync();
    }
    #endregion

    // LÓGICA PARA EVENTOS
    #region LÓGICA PARA EVENTOS
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
            var productosCompradosGrouped = productosComprados
            .GroupBy(p => p.DIAFECHAGUARDADO)
            .Select(grupo => new ProductoCompradoGroup(grupo.Key, grupo.ToList()));
            CollectionView_HistorialCompras.ItemsSource = productosCompradosGrouped.ToList();
        }

        //var productosCompradosGrouped = productosComprados
        //    .GroupBy(p => p.DIAFECHAGUARDADO)
        //    .Select(grupo => new ProductoCompradoGroup(grupo.Key, grupo.ToList()));
        //CollectionView_HistorialCompras.ItemsSource = productosCompradosGrouped.ToList();
    }
    #endregion

    // LÓGICA
    #region LÓGICA
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

    // LÓGICA DE COSAS VISUALES DE LA PÁGINA
    #region LÓGICA DE COSAS VISUALES DE LA PÁGINA

    #endregion

    
}