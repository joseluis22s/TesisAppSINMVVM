using CommunityToolkit.Maui.Alerts;
using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.Database.Tables;
using TesisAppSINMVVM.Views;

namespace TesisAppSINMVVM.Contents;

public partial class NuevaCompraContentView : ContentView
{
    private Tbl_Producto_Repository Tbl_Producto_repo = new Tbl_Producto_Repository();
    private List<Tbl_Producto> productos;
    private NavigationPage navigationPage = Application.Current.MainPage as NavigationPage;
    private ContentPage currentPage;

    public NuevaCompraContentView()
	{
		InitializeComponent();
        currentPage = (ContentPage?)navigationPage.CurrentPage;
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
        //string nuevoProducto = await currentPage.DisplayPromptAsync("Agregar producto", "Ingrese el nombre del producto:", "ACEPTAR", "CANCELAR");
        //bool resultado = await VerificarExistenciaProductoDBAsync(nuevoProducto);
        //if (resultado)
        //{
        //    await Toast.Make("Producto ya existente").Show();
        //}
        //else
        //{
        //    await GuardarProductoDBAsync(nuevoProducto);
        //    await Toast.Make("Producto guardado con éxito").Show();
        //}
    }
    private async void ContentView_Loaded(object sender, EventArgs e)
    {
        await CargarProductos();
        //productos = await ObtenerProdcutosDBAsync();
        //Picker_Producto.ItemsSource = productos;
    }
    private async void Picker_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await PermiteElegirProductos();
        //productos = await ObtenerProdcutosDBAsync();
        //Picker_Producto.ItemsSource = productos;
        //bool resultado = productos.Count == 0;
        //if (resultado)
        //{
        //    await Toast.Make("Agregue al meno un producto").Show();
        //}
        //else
        //{
        //    Picker_Producto.Focus();
        //}
    }
    

    // LOGICA PARA EVENTOS
    private async Task AgregarNuevaCompra()
    {
        double precio = double.Parse(Entry_Precio.Text);
        int cantidad = int.Parse(Entry_Cantidad.Text);

        double total = precio * cantidad;

        Label_ValorTotal.Text = total.ToString();

        await currentPage.DisplayAlert("AVISO", "El registro se ha guardado", "Aceptar");

        Tbl_Proveedor proveedorBinding = (Tbl_Proveedor) BindingContext;

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
        string nuevoProducto = await currentPage.DisplayPromptAsync("NUEVO PRODUCTO", "Ingrese el nombre del producto:", "AGREGAR", "CANCELAR");
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
    //      Verificar si Task.Run() afecta en rendimiento o fluides o si no hace nada
    private async Task CargarProductos()
    {
        productos = await ObtenerProdcutosDBAsync();
        await Task.Run(() =>
        {
            Picker_Producto.ItemsSource = productos;
        });
    }
    //      Verificar si Task.Run() afecta en rendimiento o fluides o si no hace nada
    private async Task PermiteElegirProductos()
    {
        productos = await ObtenerProdcutosDBAsync();

        await Task.Run(() =>
        {
            Picker_Producto.ItemsSource = productos;
        });
        bool existeProdcutos = productos.Count == 0;
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
        return await Tbl_Producto_repo.VerificarExistenciaProductoAsync(resultado);
    }
    private async Task GuardarProductoDBAsync(string producto)
    {
        await Tbl_Producto_repo.GuardarProductoAsync(producto);
    }
    private async Task<List<Tbl_Producto>> ObtenerProdcutosDBAsync()
    {
        return await Tbl_Producto_repo.ObtenerProdcutosAsync();
    }


    // LÓGICA DE COSAS VISUALES DE LA PÁGINA


}