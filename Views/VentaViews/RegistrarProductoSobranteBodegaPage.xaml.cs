using TesisAppSINMVVM.Database.Respositories;
using System.Collections.ObjectModel;
using TesisAppSINMVVM.Models;
using TesisAppSINMVVM.Database.Tables;

namespace TesisAppSINMVVM.Views.VentaViews;

public partial class RegistrarProductoSobranteBodegaPage : ContentPage
{
    private Tbl_Producto_Repository _repoProducto;
    private Tbl_ProductosInventario_Repository _repoProductosInventario;
    private List<Tbl_Producto> _productos;
    private ObservableCollection<ProductoInventarioModel> _productosInventario = new ObservableCollection<ProductoInventarioModel>();
    public ObservableCollection<ProductoInventarioModel> ProductosInventario
    {
        get { return _productosInventario; }
        set 
        {
            _productosInventario = value;
        }
    }

    
    public RegistrarProductoSobranteBodegaPage()
	{
		InitializeComponent();
        _repoProducto = new Tbl_Producto_Repository();
        _productos = new List<Tbl_Producto>();
        _repoProductosInventario = new Tbl_ProductosInventario_Repository();
    }

    // NAVEGACI�N
    private async Task HistorialProductoSobrantePageAsync()
    {
        await Navigation.PushAsync(new HistorialProductoSobrantePage());
    }


    // EVENTOS
    private async void ContentPage_Appearing(object sender, EventArgs e)
    {
        base.OnAppearing();
        await CargarProductosInventario();
    }
    private async void Button_GuardarRegistroProductoSobrante_Clicked(object sender, EventArgs e)
    {
        List<Tbl_ProductosInventario> inventario = new List<Tbl_ProductosInventario>();
        var productos = (ObservableCollection<ProductoInventarioModel>)CollectionView_RegistrarProductoSobranteBodega.ItemsSource;
        foreach (var p in productos)
        {
            if (p.ESSELECCIONADO == true)
            {
                inventario.Add(new Tbl_ProductosInventario
                {
                    PRODUCTO = p.PRODUCTO,
                    CANTIDAD = p.CANTIDAD,
                    DESCRIPCION = p.DESCRIPCION,
                    FECHAGUARDADO = DateTime.Now.ToString("dd/MM/yyyy"),
                    DIAFECHAGUARDADO = DateTime.Now.ToString("dddd, dd MMMM")
                });
            }
        }
        var a = inventario;
        await _repoProductosInventario.GuardarProductosInventarioAsync(inventario);
    }
    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        CheckBox checkBox = (CheckBox)sender;
        ProductoInventarioModel producto = (ProductoInventarioModel)checkBox.BindingContext;
        producto.ESSELECCIONADO = e.Value;
    }
    private async void Button_HistorialRegistroProductoSobrante_Clicked(object sender, EventArgs e)
    {
        await HistorialProductoSobrantePageAsync();
    }



    // LOGICA PARA EVENTOS
    private async Task CargarProductosInventario()
    {
        var productos = await ObtenerProdcutosDBAsync();
        foreach (var p in productos)
        {
            ProductosInventario.Add(new ProductoInventarioModel
            {
                PRODUCTO = p.PRODUCTO
            });
        }
        CollectionView_RegistrarProductoSobranteBodega.ItemsSource = ProductosInventario;
    }


    // L�GICA


    // BASE DE DATOS
    private async Task<List<Tbl_Producto>> ObtenerProdcutosDBAsync()
    {
        return await _repoProducto.ObtenerProdcutosAsync();
    }

    








    // L�GICA DE COSAS VISUALES DE LA P�GINA

}