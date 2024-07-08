using CommunityToolkit.Maui.Alerts;
using System.Collections.ObjectModel;
using TesisAppSINMVVM.Database.Tables;
using TesisAppSINMVVM.FirebaseDataBase.Repositories;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.Views.CompraViews;

public partial class RegistrarNuevaCompraPage : ContentPage
{
    private Producto_Repository _repoProducto = new Producto_Repository();
    private bool _enEjecucion;

    private ObservableCollection<AuxProducto> _auxProductos = new ObservableCollection<AuxProducto>();
    public ObservableCollection<AuxProducto> _AuxProductos
    {
        get { return _auxProductos; }
        set
        {
            _auxProductos = value;
        }
    }


    public RegistrarNuevaCompraPage()
    {
        InitializeComponent();
    }
    // NAVEGACIÓN
    #region NAVEGACIÓN

    #endregion


    // EVENTOS
    #region EVENTOS
    private async void ContentPage_Appearing(object sender, EventArgs e)
    {
        // TODO: Verificar si se debe agregar un _ejecutarAppearing
        base.OnAppearing();
        await CargarProductos_CollectionView_Productos();
    }
    private void Button_GuardarNuevaCompra_Clicked(object sender, EventArgs e)
    {

    }
    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {

    }

    private void Entry_TextChanged_Cantidad(object sender, TextChangedEventArgs e)
    {

    }

    private void Entry_PrecioUnitarioEntero_TextChanged(object sender, TextChangedEventArgs e)
    {

    }
    private void Entry_PrecioUnitarioDecimal_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void Button_AgregarProdcuto_Clicked(object sender, EventArgs e)
    {

    }

    #endregion


    // LÓGICA PARA EVENTOS
    #region LÓGICA PARA EVENTOS
    private async Task CargarProductos_CollectionView_Productos()
    {
        var productos = await ObtenerProductosDBAsync();
        foreach (var p in productos)
        {
            _AuxProductos.Add(new AuxProducto
            {
                PRODUCTO = p.PRODUCTO,
                MEDIDA = p.MEDIDA
            });
        }
        CollectionView_Productos.ItemsSource = _AuxProductos;
    }
    private async Task GuardarNuevaCompra(object sender)
    {
        List<ProductoComprado> productosComprados = new List<ProductoComprado>();
        _AuxProductos = (ObservableCollection<AuxProducto>)CollectionView_Productos.ItemsSource;
        foreach (var p in _AuxProductos)
        {
            if (p.ESSELECCIONADO == true)
            {
                bool camposVacios = ComprobarCamposVaciosProductos(p);
                if (camposVacios)
                {
                    _enEjecucion = false;
                    await Toast.Make("No deben existir campos vacíos").Show();
                    return;
                }
                else
                {
                    productosComprados.Add(new ProductoComprado
                    {
                        PRODUCTO = p.PRODUCTO,
                        MEDIDA = p.MEDIDA,
                        CANTIDAD = p.CANTIDAD,
                        PRECIO = p.PRECIO,

                    });
                }

            }
        }
    }
    #endregion


    // LÓGICA
    #region LÓGICA
    private bool ComprobarCamposVaciosProductos(AuxProducto auxProducto)
    {
        if (auxProducto.CANTIDAD == 0 || auxProducto.PRECIO == 0)
        {
            return true;
        }
        return false;
    }
    #endregion


    // BASE DE DATOS
    #region BASE DE DATOS
    private async Task<List<Tbl_Producto>> ObtenerProductosDBAsync()
    {
        return await _repoProducto.ObtenerProductosAsync();
    }

    #endregion


    // LÓGICA DE COSAS VISUALES DE LA PÁGINA
    #region LÓGICA DE COSAS VISUALES DE LA PÁGINA

    #endregion

    
}