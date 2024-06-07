using CommunityToolkit.Maui.Alerts;
using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.Database.Tables;

namespace TesisAppSINMVVM.Views.CompraViews;

public partial class RegistrarNuevaCompraPage : ContentPage
{
    private Tbl_Producto_Repository _repoProducto;
    private List<Tbl_Producto> _productos;
    private Tbl_HistorialCompras_Repository _repohistorialCompras;
    private Tbl_Proveedor _proveedor;
    private bool _enEjecucion;

    public RegistrarNuevaCompraPage()
	{
		InitializeComponent();
        _repoProducto = new Tbl_Producto_Repository();
        _repohistorialCompras = new Tbl_HistorialCompras_Repository();
    }


    // NAVEGACIÓN
    private async Task HistorialComprasContentPagePushAsync()
    {
        _proveedor = (Tbl_Proveedor)BindingContext;
        await Navigation.PushAsync(new HistorialComprasPage 
        {
            BindingContext = _proveedor
        });
    }
    private async Task RegistrarNuevaCompraPagePopAsync(bool mostrarAlerta)
    {
        ProveedoresPage._permitirEjecucion = false;
        await PermitirPopAsyncNavegacion(mostrarAlerta);
    }


    // EVENTOS
    private async void ContentPage_Appearing(object sender, EventArgs e)
    {
        base.OnAppearing();
        await CargarDatosProductos();
    }
    private async void Button_AgregarNuevoProductoPicker_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await AgregarNuevoProducto();
        _enEjecucion = false;
    }
    private void Button_GuardarNuevaCompra_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        AgregarNuevaCompra();
        _enEjecucion = false;
    }
    private async void Button_HistorialCompras_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await HistorialComprasContentPagePushAsync();
        _enEjecucion = false;
    }
    private async void Button_Regresar_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await RegistrarNuevaCompraPagePopAsync(true);
        _enEjecucion = false;
    }
    protected override bool OnBackButtonPressed()
    {
        RegistrarNuevaCompraPagePopAsync(true).GetAwaiter();
        return true;
    }
    private void Entry_Precio_TextChanged(object sender, TextChangedEventArgs e)
    {
        CalcularTotalEntry_Precio();
    }
    private void Entry_Cantidad_TextChanged(object sender, TextChangedEventArgs e)
    {
        CalcularTotalEntry_Cantidad();
    }
    private void Entry_SaldoPendiente_TextChanged(object sender, TextChangedEventArgs e)
    {
        SaldoPendiente();
    }


    // LOGICA PARA EVENTOS
    private async Task PermitirPopAsyncNavegacion(bool mostrarAlerta)
    {
        if (mostrarAlerta)
        {
            CompraPage._permitirEjecucion = false;
            bool respuesta = await DisplayAlert("Alerta", "¿Desea regresar? Perderá el progreso realizado", "Confimar", "Cancelar");
            if (respuesta)
            {
                await Navigation.PopAsync();
            }
        }
        else
        {
            await Navigation.PopAsync();
        }
    }
    private async Task CargarDatosProductos()
    {
        _productos = await ObtenerProductosDBAsync();
        Picker_Producto.ItemsSource = _productos;
    }
    private async Task AgregarNuevoProducto()
    {
        string nuevoProducto;
        do
        {
            nuevoProducto = await DisplayPromptAsync("NUEVO PRODUCTO", "Ingrese el nombre del producto:", "AGREGAR", "CANCELAR", null, -1, Keyboard.Create(KeyboardFlags.CapitalizeCharacter));
            if (nuevoProducto is not null)
            {
                if (nuevoProducto != "")
                {
                    //string nuevoProducto = await _compraPage.DisplayPromptAsync("NUEVO PRODUCTO", "Ingrese el nombre del producto:", "AGREGAR", "CANCELAR", null,-1,Keyboard.Create(KeyboardFlags.CapitalizeCharacter));
                    // TODO: De la misma que el double y el int, contorlar los espasio en blanco
                    bool existeProducto = await VerificarExistenciaProductoDBAsync(nuevoProducto);
                    if (existeProducto)
                    {
                        await Toast.Make("El producto ya existe").Show();
                    }
                    else
                    {

                        await GuardarProductoDBAsync(nuevoProducto);
                        _productos = await ObtenerProductosDBAsync();
                        await Toast.Make("el producto se ha guardado").Show();
                    }
                }
                else
                {
                    await Toast.Make("¡El campo no debe estar vacío!").Show();
                }

            }
        }
        while (nuevoProducto == "");

    }
    private async void AgregarNuevaCompra()
    {
        bool camposValidos = VerificarCamposValidos();
        if (camposValidos)
        {

            Tbl_Proveedor proveedorBinding = (Tbl_Proveedor)BindingContext;
            var itemProducto = (Tbl_Producto)Picker_Producto.SelectedItem;

            Tbl_HistorialCompras compra = new Tbl_HistorialCompras()
            {
                NOMBRE = proveedorBinding.NOMBRE,
                APELLIDO = proveedorBinding.APELLIDO,
                DIAFECHA = DateTime.Today.ToString("dddd, dd MMMM"),
                PRODUCTO = itemProducto.PRODUCTO,
                FECHA = DateTime.Now.ToString("dd/MM/yyyy"),
                DIA = MapearDayOfWeekEspanol(DateTime.Now.DayOfWeek),//DateTime.Now.DayOfWeek.ToString().ToUpper(),
                CANTIDAD = int.Parse(Entry_Cantidad.Text),
                PRECIO = double.Parse(Entry_Precio.Text),
                TOTAL = double.Parse(Label_ValorTotal.Text),
                SALDOPENDIENTE = double.Parse(Entry_SaldoPendiente.Text)
            };
            OcultarTeclado();
            VaciarCampos();
            await GuardarRegistroProductoDBAsync(compra);
            await DisplayAlert("AVISO", "El registro se ha guardado", "Aceptar");
        }
        else
        {
            OcultarTeclado();
            await Toast.Make("Los campos no deben estar vacios").Show();
        }
    }
    private void CalcularTotalEntry_Precio()
    {
        double precio;
        int cantidad;
        double resultado;

        // Comprobamos si el texto contiene más de 2 decimales
        if (Entry_Precio.Text.Contains(".") && Entry_Precio.Text.Split('.').Last().Length > 2)
        {
            // Si contiene más de 2 decimales, lo truncamos a 2 decimales
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


    // LÓGICA
    private bool VerificarCamposValidos()
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
    private string MapearDayOfWeekEspanol(DayOfWeek dayOfWeek)
    {
        Dictionary<DayOfWeek, string> dayOfWeekMap = new Dictionary<DayOfWeek, string>
        {
            { DayOfWeek.Sunday, "Domingo" },
            { DayOfWeek.Monday, "Lunes" },
            { DayOfWeek.Tuesday, "Martes" },
            { DayOfWeek.Wednesday, "Miércoles" },
            { DayOfWeek.Thursday, "Jueves" },
            { DayOfWeek.Friday, "Viernes" },
            { DayOfWeek.Saturday, "Sábado" }
        };

        return dayOfWeekMap[dayOfWeek];
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

    // BASE DE DATOS
    private async Task<bool> VerificarExistenciaProductoDBAsync(string resultado)
    {
        return await _repoProducto.VerificarExistenciaProductoAsync(resultado);
    }
    private async Task GuardarProductoDBAsync(string producto)
    {
        await _repoProducto.GuardarProductoAsync(producto);
    }
    private async Task<List<Tbl_Producto>> ObtenerProductosDBAsync()
    {
        return await _repoProducto.ObtenerProductosAsync();
    }
    private async Task GuardarRegistroProductoDBAsync(Tbl_HistorialCompras registroCompra)
    {
        await _repohistorialCompras.GuardarRegistroProductoAsync(registroCompra);
    }





    // LÓGICA DE COSAS VISUALES DE LA PÁGINA  
    private void OcultarTeclado()
    {
        Entry_Precio.Unfocus();
        Entry_Cantidad.Unfocus();
        Entry_SaldoPendiente.Unfocus();
    }
    private void VaciarCampos()
    {
        Picker_Producto.SelectedItem = null;
        Entry_Precio.Text = "";
        Entry_Cantidad.Text = "";
        Label_ValorTotal.Text = "";
        Entry_SaldoPendiente.Text = "";
    }

    
}