using TesisAppSINMVVM.Database.Respositories;
using System.Collections.ObjectModel;
using TesisAppSINMVVM.Models;
using TesisAppSINMVVM.Database.Tables;
using CommunityToolkit.Maui.Alerts;
using TesisAppSINMVVM.Platforms.Android;

namespace TesisAppSINMVVM.Views.VentaViews;

public partial class RegistrarProductoSobranteBodegaPage : ContentPage
{
    private Tbl_Producto_Repository _repoProducto;
    private Tbl_ProductosInventario_Repository _repoProductosInventario;
    private List<Tbl_Producto> _productos;
    private ObservableCollection<ProductoInventarioModel> _productosInventario = new ObservableCollection<ProductoInventarioModel>();
    private bool _enEjecucion;
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
        //    if (_enEjecucion)
        //    {
        //        return;
        //    }
        //    _enEjecucion = true;
        //await Navigation.PopAsync();
        await PagePopAsync();
        //_enEjecucion = false;
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
        var send = sender;
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        List<Tbl_ProductosInventario> inventario = new List<Tbl_ProductosInventario>();
        var productos = (ObservableCollection<ProductoInventarioModel>)CollectionView_RegistrarProductoSobranteBodega.ItemsSource;
        foreach (var p in productos)
        {
            if (p.ESSELECCIONADO == true)
            {
                if (string.IsNullOrEmpty(p.CANTIDAD))
                {
                    _enEjecucion = false;
                    await Toast.Make("¡Los campos de cantidad no deben estar vacíos!").Show();
                    return;
                }
                else
                {
                    inventario.Add(new Tbl_ProductosInventario
                    {
                        PRODUCTO = p.PRODUCTO,
                        CANTIDAD = int.Parse(p.CANTIDAD),
                        DESCRIPCION = p.DESCRIPCION,
                        FECHAGUARDADO = DateTime.Now.ToString("dd/MM/yyyy"),
                        DIAFECHAGUARDADO = DateTime.Now.ToString("dddd, dd MMMM")
                    });
                }
                
            }
        }
        var a = inventario;
        if (inventario.Count == 0)
        {
            await Toast.Make("Selecione al menos un producto").Show();
        }
        else
        {
            foreach (var p in productos)
            {
                p.CANTIDAD = "";
                p.DESCRIPCION = "";
                p.ESSELECCIONADO = false;
            }
            //#if ANDROID
            //KeyboardHelper.HideKeyboard();
            //#endif 
            await Toast.Make("¡Registro guardado!").Show();
            await _repoProductosInventario.GuardarProductosInventarioAsync(inventario);
        }
        _enEjecucion = false;
        
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
    private void Entry_TextChanged_Cantidad(object sender, TextChangedEventArgs e)
    {
        Entry entry = (Entry)sender;
        if (entry.Text.Contains("."))
        {
            entry.Text = entry.Text.Replace(".", "");
        }
    }
    private async void Button_Regresar_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await RegistrarProductoSobranteBodegaPagePopAsync();
        _enEjecucion = false;
    }
    protected override bool OnBackButtonPressed()
    {
        RegistrarProductoSobranteBodegaPagePopAsync().GetAwaiter();
        return true;
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
    //private async Task GuardarRegistroProductoSobrante()
    //{

    //}
    private async Task PagePopAsync()
    {
        bool respuesta = await DisplayAlert("Alerta", "¿Desea regresar? Perderá el progreso realizado", "Confimar", "Cancelar");
        if (respuesta)
        {
            await Navigation.PopAsync();
        }
    }


    // LÓGICA


    // BASE DE DATOS
    private async Task<List<Tbl_Producto>> ObtenerProdcutosDBAsync()
    {
        return await _repoProducto.ObtenerProductosAsync();
    }

    













    // LÓGICA DE COSAS VISUALES DE LA PÁGINA

}