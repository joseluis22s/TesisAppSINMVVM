using CommunityToolkit.Maui.Alerts;
using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.Database.Tables;

namespace TesisAppSINMVVM.Contents;

public partial class NuevaCompraContentView : ContentView
{
    private Tbl_Producto_Repository _repoProveedor;
    private List<Tbl_Producto> _productos;
    private NavigationPage _navigationPage;
    private ContentPage _currentPage;
    private bool _productosCargados = false;

    public NuevaCompraContentView()
    {
        InitializeComponent();
        _repoProveedor = new Tbl_Producto_Repository();
        _navigationPage = Application.Current.MainPage as NavigationPage;
        _currentPage = (ContentPage?)_navigationPage.CurrentPage;
    }


    // NAVEGACIÓN

    // EVENTOS
    private void Button_GuardarNuevaCompra_Clicked(object sender, EventArgs e)
    {
        //AgregarNuevaCompra();
    }
    private void Button_AgregarNuevoProductoPicker_Clicked(object sender, EventArgs e)
    {
        AgregarNuevoProducto();
    }
    private void ContentView_Loaded(object sender, EventArgs e)
    {
        CargarProductos();
    }
    private void Picker_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        PermiteElegirProductos();
    }



    // LOGICA PARA EVENTOS
    private async void AgregarNuevaCompra()
    {
        double precio = double.Parse(Entry_Precio.Text);
        int cantidad = int.Parse(Entry_Cantidad.Text);

        double total = precio * cantidad;

        Label_ValorTotal.Text = total.ToString();

        await _currentPage.DisplayAlert("AVISO", "El registro se ha guardado", "Aceptar");

        Tbl_Proveedor proveedorBinding = (Tbl_Proveedor)BindingContext;

        Tbl_HistorialCompras compra = new Tbl_HistorialCompras()
        {
            NOMBRE = nameof(proveedorBinding.NOMBRE),
            APELLIDO = nameof(proveedorBinding.APELLIDO),
            PRODUCTO = nameof(Picker_Producto.SelectedItem),
            FECHA = DateTime.Now.ToString("dd/MM/yyyy"),
            DIA = DateTime.Now.Day.ToString().ToUpper(),
            CANTIDAD = cantidad,
            PRECIO = precio,
            TOTAL = total,
            SALDOPENDIENTE = 0
        };
    }
    private async void AgregarNuevoProducto()
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
    private async void CargarProductos()
    {
        _productos = await ObtenerProdcutosDBAsync();
        Picker_Producto.ItemsSource = _productos;
        _productosCargados = true;
}
    private async void PermiteElegirProductos()
    {
        if (!_productosCargados)
        {
            _productos = await ObtenerProdcutosDBAsync();
        }
        Picker_Producto.ItemsSource = _productos;
        bool existeProductos = _productos.Count == 0;
        if (existeProductos)
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

    private void Button_BorrarProductoPicker_Clicked(object sender, EventArgs e)
    {
        // TODO: Lógica para borrar un solo prodcuto en
        //       el picker, hacer tambien de la DB.
    }


    // LÓGICA DE COSAS VISUALES DE LA PÁGINA


}