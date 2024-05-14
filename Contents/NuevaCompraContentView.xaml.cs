using CommunityToolkit.Maui.Alerts;
using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.Database.Tables;
using TesisAppSINMVVM.Views;

namespace TesisAppSINMVVM.Contents;

public partial class NuevaCompraContentView : ContentView
{
    private Tbl_Producto_Repository _repoProveedor;
    private List<Tbl_Producto> _productos;
    private NavigationPage _navigationPage;
    private ContentPage _currentPage;

    public NuevaCompraContentView()
    {
        InitializeComponent();
        _repoProveedor = new Tbl_Producto_Repository();
        _navigationPage = Application.Current.MainPage as NavigationPage;
        _currentPage = (ContentPage?)_navigationPage.CurrentPage;
    }


    // NAVEGACIÓN

    // EVENTOS
    private async void Button_GuardarNuevaCompra_Clicked(object sender, EventArgs e)
    {
        await AgregarNuevaCompra();
    }
    private async void Button_AgregarNuevoProductoPicker_Clicked(object sender, EventArgs e)
    {
        await AgregarNuevoProducto();
    }
    private async void ContentView_Loaded(object sender, EventArgs e)
    {
        await CargarProductos();
    }
    private async void Picker_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await PermiteElegirProductos();
    }


    // LOGICA PARA EVENTOS
    private async Task AgregarNuevaCompra()
    {
        double precio = double.Parse(Entry_Precio.Text);
        int cantidad = int.Parse(Entry_Cantidad.Text);

        double total = precio * cantidad;

        Label_ValorTotal.Text = total.ToString();

        await _currentPage.DisplayAlert("AVISO", "El registro se ha guardado", "Aceptar");

        Tbl_Proveedor proveedorBinding = (Tbl_Proveedor)BindingContext;

        Tbl_HistorialCompras compra = new Tbl_HistorialCompras()
        {
            NOMBRE = proveedorBinding.NOMBRE,
            APELLIDO = proveedorBinding.APELLIDO,
            PRODUCTO = Picker_Producto.SelectedItem.ToString(),
            FECHA = DateTime.Now.ToString("dd/MM/yyyy"),
            DIA = DateTime.Now.Day.ToString().ToUpper(),
            CANTIDAD = cantidad,
            PRECIO = precio,
            TOTAL = total,
            SALDOPENDIENTE = 0
        };
    }

    private async Task AgregarNuevoProducto()
    {
        string nuevoProducto = await _currentPage.DisplayPromptAsync("NUEVO PRODUCTO", "Ingrese el nombre del producto:", "AGREGAR", "CANCELAR");
        bool existeProducto = await VerificarExistenciaProductoDBAsync(nuevoProducto);
        if (existeProducto)
        {
            await Toast.Make("El producto ya existe").Show();
        }
        else
        {
            await GuardarProductoDBAsync(nuevoProducto);
            await Toast.Make("el producto se ha guardado").Show();
        }
    }
    private async Task CargarProductos()
    {
        _productos = await ObtenerProdcutosDBAsync();
        Picker_Producto.ItemsSource = _productos;
    }
    private async Task PermiteElegirProductos()
    {
        _productos = await ObtenerProdcutosDBAsync();
        Picker_Producto.ItemsSource = _productos;
        bool existeProdcutos = _productos.Count == 0;
        if (existeProdcutos)
        {
            await Toast.Make("Agregue al menos un producto").Show();
        }
        else
        {
            Picker_Producto.Focus();
        }
    }


    // BASE DE DATOS
    private async Task<bool> VerificarExistenciaProductoDBAsync(string resultado)
    {
        return await _repoProveedor.VerificarExistenciaProductoAsync(resultado);
    }
    private async Task GuardarProductoDBAsync(string producto)
    {
        await _repoProveedor.GuardarProductoAsync(producto);
    }
    private async Task<List<Tbl_Producto>> ObtenerProdcutosDBAsync()
    {
        return await _repoProveedor.ObtenerProdcutosAsync();
    }


    // LÓGICA DE COSAS VISUALES DE LA PÁGINA


}