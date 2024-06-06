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
    public static bool _ejecutarAppearing = true;
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

    // NAVEGACIÓN
    private async Task RegistrarProductoSobranteBodegaPagePopAsync()
    {
        await PagePopAsync();
    }
    private async Task HistorialRegistroProductoSobrantePagePushAsync()
    {
        await Navigation.PushAsync(new HistorialProductoSobrantePage());
    }


    // EVENTOS
    private async void ContentPage_Appearing(object sender, EventArgs e)
    {
        if (_ejecutarAppearing)
        {
            base.OnAppearing();
            await CargarProductosInventario();
        }
        _ejecutarAppearing = true;
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
        await HistorialRegistroProductoSobrantePagePushAsync();
    }

    private async void Button_Regresar_Clicked(object sender, EventArgs e)
    {
        await RegistrarProductoSobranteBodegaPagePopAsync();
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
    private Task PagePopAsync()
    {
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


    // LÓGICA


    // BASE DE DATOS
    private async Task<List<Tbl_Producto>> ObtenerProdcutosDBAsync()
    {
        return await _repoProducto.ObtenerProductosAsync();
    }











    // LÓGICA DE COSAS VISUALES DE LA PÁGINA

}