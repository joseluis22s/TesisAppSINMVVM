using CommunityToolkit.Maui.Alerts;
using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.Database.Tables;

namespace TesisAppSINMVVM.Contents;

public partial class NuevaCompraContentView : ContentView
{
    private Tbl_Producto_Repository _repoProducto;
    private Tbl_HistorialCompras_Repository _repohistorialCompras;
    private List<Tbl_Producto> _productos;
    private NavigationPage _navigationPage;
    private ContentPage _compraPage;
    private bool _productosCargados = false;

    public static bool _esPresionado = false;

    public NuevaCompraContentView()
    {
        InitializeComponent();
        _repoProducto = new Tbl_Producto_Repository();
        _repohistorialCompras = new Tbl_HistorialCompras_Repository();
        _navigationPage = Application.Current.MainPage as NavigationPage;
        _compraPage = (ContentPage?)_navigationPage.CurrentPage;
    }


    // NAVEGACI�N

    // EVENTOS
    private void ContentView_Loaded(object sender, EventArgs e)
    {
        CargarProductos();
    }
    private void Button_AgregarNuevoProductoPicker_Clicked(object sender, EventArgs e)
    {
        AgregarNuevoProducto();
    }
    private async void Border_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await BorderTapped();
    }
    private void Entry_Precio_TextChanged(object sender, TextChangedEventArgs e)
    {
        CalcularTotalEntry_Precio();
    }
    private void Entry_Cantidad_TextChanged(object sender, TextChangedEventArgs e)
    {
        CalcularTotalEntry_Cantidad();
    }
    private void Button_GuardarNuevaCompra_Clicked(object sender, EventArgs e)
    {
        AgregarNuevaCompra();
    }
    private void Entry_SaldoPendiente_TextChanged(object sender, TextChangedEventArgs e)
    {
        SaldoPendiente();
    }


    // LOGICA PARA EVENTOS
    private async Task<bool> VerificarCamposValidos()
    {
        var itemProducto = (Tbl_Producto)Picker_Producto.SelectedItem;
        if (itemProducto is null)
        {
            return false;
        }

        string producto = itemProducto.PRODUCTO.ToString();
        string precio = Entry_Precio.Text;
        string cantidad = Entry_Cantidad.Text;
        string saldo = Entry_SaldoPendiente.Text;

        if (producto == "" || precio == "" || cantidad == "" || saldo == "")
        {
            return false;
        }
        return true;
    }
    private async void AgregarNuevaCompra()
    {
        bool camposValidos = await VerificarCamposValidos();
        if (camposValidos)
        {
            Tbl_Proveedor proveedorBinding = (Tbl_Proveedor)BindingContext;
            var itemProducto = (Tbl_Producto)Picker_Producto.SelectedItem;

            Tbl_HistorialCompras compra = new Tbl_HistorialCompras()
            {
                NOMBRE = proveedorBinding.NOMBRE,
                APELLIDO = proveedorBinding.APELLIDO,
                DIAFECHA = MapearDayOfWeekEspanol(DateTime.Now.DayOfWeek) + "-" + DateTime.Now.ToString("dd/MM/yyyy"),
                PRODUCTO = itemProducto.PRODUCTO,
                FECHA = DateTime.Now.ToString("dd/MM/yyyy"),
                DIA = MapearDayOfWeekEspanol(DateTime.Now.DayOfWeek),//DateTime.Now.DayOfWeek.ToString().ToUpper(),
                CANTIDAD = int.Parse(Entry_Cantidad.Text),
                PRECIO = double.Parse(Entry_Precio.Text),
                TOTAL = double.Parse(Label_ValorTotal.Text),
                SALDOPENDIENTE = double.Parse(Entry_SaldoPendiente.Text)
            };
            await GuardarRegistroProductoDBAsync(compra);
            await _compraPage.DisplayAlert("AVISO", "El registro se ha guardado", "Aceptar");
        }
        else
        {
            await Toast.Make("Los campos no deben estar vacios").Show();
        }
    }
    private async void AgregarNuevoProducto()
    {
        string nuevoProducto = await _compraPage.DisplayPromptAsync("NUEVO PRODUCTO", "Ingrese el nombre del producto:", "AGREGAR", "CANCELAR");
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
        if (_productos.Count != 0)
        {
            Border_OcultaPicker.IsEnabled = false;
            Border_OcultaPicker.IsVisible = false;
            Picker_Producto.ItemsSource = _productos;
            _productosCargados = true;
        }
        //_productos = await ObtenerProdcutosDBAsync();
        //if (_productos.Count == 0)
        //{
        //    Picker_Producto.IsVisible = false;
        //    Picker_Producto.IsEnabled = false;
        //}
        //else
        //{
        //    Border_OcultaPicker.IsEnabled = false;
        //    Picker_Producto.ItemsSource = _productos;
        //    _productosCargados = true;
        //}
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
    private async Task BorderTapped()
    {
        if (_productos.Count == 0)
        {
            await Toast.Make("Agregue al menos un producto").Show();
        }
    }
    private string MapearDayOfWeekEspanol(DayOfWeek dayOfWeek)
    {
        Dictionary<DayOfWeek, string> dayOfWeekMap = new Dictionary<DayOfWeek, string>
        {
            { DayOfWeek.Sunday, "Domingo" },
            { DayOfWeek.Monday, "Lunes" },
            { DayOfWeek.Tuesday, "Martes" },
            { DayOfWeek.Wednesday, "Mi�rcoles" },
            { DayOfWeek.Thursday, "Jueves" },
            { DayOfWeek.Friday, "Viernes" },
            { DayOfWeek.Saturday, "S�bado" }
        };

        return dayOfWeekMap[dayOfWeek];
    }
    private void CalcularTotalEntry_Precio()
    {
        double precio;
        int cantidad;
        double resultado;

        // Comprobamos si el texto contiene m�s de 2 decimales
        if (Entry_Precio.Text.Contains(".") && Entry_Precio.Text.Split('.').Last().Length > 2)
        {
            // Si contiene m�s de 2 decimales, lo truncamos a 2 decimales
            Entry_Precio.Text = Entry_Precio.Text.Substring(0, Entry_Precio.Text.IndexOf(".") + 3);
        }
        if (double.TryParse(Entry_Precio.Text, out precio))
        {
            if (int.TryParse(Entry_Cantidad.Text, out cantidad))
            {
                resultado = precio * cantidad;
                Label_ValorTotal.Text = resultado.ToString();
                return;
            }
        }
        Label_ValorTotal.Text = "0";
    }
    private void CalcularTotalEntry_Cantidad()
    {
        double precio;
        int cantidad;
        double resultado;
        if (Entry_Cantidad.Text.Contains("."))
        {
            Entry_Cantidad.Text = Entry_Cantidad.Text.Replace(".", "");
        }

        if (int.TryParse(Entry_Cantidad.Text, out cantidad))
        {
            if (double.TryParse(Entry_Precio.Text, out precio))
            {
                resultado = precio * cantidad;
                Label_ValorTotal.Text = resultado.ToString();
                return;
            }
        }
        Label_ValorTotal.Text = "0";
    }
    private void SaldoPendiente()
    {
        if (Entry_SaldoPendiente.Text.Contains(".") && Entry_SaldoPendiente.Text.Split('.').Last().Length > 2)
        {
            Entry_SaldoPendiente.Text = Entry_SaldoPendiente.Text.Substring(0, Entry_SaldoPendiente.Text.IndexOf(".") + 3);
        }
    }



    // BASE DE DATOS

    //      Tbl_Producto_Repository
    private async Task<bool> VerificarExistenciaProductoDBAsync(string resultado)
    {
        return await _repoProducto.VerificarExistenciaProductoAsync(resultado);
    }
    private async Task GuardarProductoDBAsync(string producto)
    {
        await _repoProducto.GuardarProductoAsync(producto);
    }
    private async Task<List<Tbl_Producto>> ObtenerProdcutosDBAsync()
    {
        return await _repoProducto.ObtenerProdcutosAsync();
    }
    private void Button_BorrarProductoPicker_Clicked(object sender, EventArgs e)
    {
        // TODO: L�gica para borrar un solo prodcuto en
        //       el picker, hacer tambien de la DB.
    }

    //      Tbl_HistorialCompras_Repository
    private async Task GuardarRegistroProductoDBAsync(Tbl_HistorialCompras registroCompra)
    {
        await _repohistorialCompras.GuardarRegistroProductoAsync(registroCompra);
    }

    




    // L�GICA DE COSAS VISUALES DE LA P�GINA


}