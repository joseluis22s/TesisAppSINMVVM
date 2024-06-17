using CommunityToolkit.Maui.Alerts;
using System.Threading;
using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.Database.Tables;

namespace TesisAppSINMVVM.Views.CompraViews;

public partial class RegistrarNuevaCompraPage : ContentPage
{

    private Tbl_Producto_Repository _repoProducto;
    private Tbl_HistorialCompras_Repository _repohistorialCompras;
    private List<Tbl_Producto> _productos;
    private Tbl_Proveedor _proveedor;
    private bool _enEjecucion;
    private double _precio;
    private int _cantidad;
    private double _total;
    private double _saldo;
    private bool _ejecutarTextChanged = true;

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
    private async void Border_PickerProductos_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (_productos.Count == 0)
        {
            await Toast.Make("Debe agregar prodcutos").Show();
        }
    }
    private void Entrys_TextChanged(object sender, TextChangedEventArgs e)
    {
        //Entry entry = (Entry)sender;
        //EvitarPunto(entry);
        //ControlarEntrysPrecio(entry);
        //CalcularTotal(entry);
        TodosEntrys_TextChanged();
    }
    private void TodosEntrys_TextChanged()
    {
        if (!_ejecutarTextChanged)
        {
            return;
        }
        
        if (!string.IsNullOrEmpty(Entry_PrecioEntero.Text))
        {
            if (Entry_PrecioEntero.Text.Contains("."))
            {
                Entry_PrecioEntero.Text = Entry_PrecioEntero.Text.Replace(".", "");
            }
            else if (!string.IsNullOrEmpty(Entry_PrecioEntero.Text))
            {
                if (!string.IsNullOrEmpty(Entry_PrecioDecimal.Text))
                {
                    if (Entry_PrecioDecimal.Text.Contains("."))
                    {
                        Entry_PrecioDecimal.Text = Entry_PrecioDecimal.Text.Replace(".", "");
                    }
                }
            }
        }
        else if (!string.IsNullOrEmpty(Entry_PrecioDecimal.Text))
        {
            if (Entry_PrecioDecimal.Text.Contains("."))
            {
                Entry_PrecioDecimal.Text = Entry_PrecioDecimal.Text.Replace(".", "");
            }
            else if (!string.IsNullOrEmpty(Entry_PrecioEntero.Text))
            {
                Entry_PrecioEntero.Text = Entry_PrecioEntero.Text.Replace(".", "");
            }
        }


        if (!string.IsNullOrEmpty(Entry_SaldoPendienteEntero.Text))
        {
            if (Entry_SaldoPendienteEntero.Text.Contains("."))// TODO: Aqui el entry tiene cero, a lo mejro por uqe le pngo para que se eva valor
            {
                Entry_SaldoPendienteEntero.Text = Entry_SaldoPendienteEntero.Text.Replace(".", "");
            }
            else if (!string.IsNullOrEmpty(Entry_SaldoPendienteEntero.Text))
            {
                if (Entry_SaldoPendienteDecimal.Text.Contains("."))
                {
                    Entry_SaldoPendienteDecimal.Text = Entry_SaldoPendienteDecimal.Text.Replace(".", "");
                }
            }
        }
        else if (!string.IsNullOrEmpty(Entry_SaldoPendienteDecimal.Text))
        {
            if (Entry_SaldoPendienteDecimal.Text.Contains("."))
            {
                Entry_SaldoPendienteDecimal.Text = Entry_SaldoPendienteDecimal.Text.Replace(".", "");
            }
            else if (!string.IsNullOrEmpty(Entry_PrecioEntero.Text))
            {
                Entry_SaldoPendienteEntero.Text = Entry_SaldoPendienteEntero.Text.Replace(".", "");
            }
        }

        if (string.IsNullOrEmpty(Entry_PrecioEntero.Text) && string.IsNullOrEmpty(Entry_PrecioDecimal.Text) &&
            string.IsNullOrEmpty(Entry_Cantidad.Text))
        {
            Label_ValorTotal.Text = "";
        }
        else if (string.IsNullOrEmpty(Entry_PrecioEntero.Text) && !string.IsNullOrEmpty(Entry_PrecioDecimal.Text) &&
            string.IsNullOrEmpty(Entry_Cantidad.Text))
        {
            Label_ValorTotal.Text = "";
        }
        else if (string.IsNullOrEmpty(Entry_PrecioEntero.Text) && !string.IsNullOrEmpty(Entry_PrecioDecimal.Text) &&
            !string.IsNullOrEmpty(Entry_Cantidad.Text))
        {
            var n1 = double.Parse("0." + Entry_PrecioDecimal.Text);
            var n2 = int.Parse(Entry_Cantidad.Text);
            var r = n1 * n2;
            Label_ValorTotal.Text = r.ToString();
        }
        else if (!string.IsNullOrEmpty(Entry_PrecioEntero.Text) && !string.IsNullOrEmpty(Entry_PrecioDecimal.Text) &&
            string.IsNullOrEmpty(Entry_Cantidad.Text))
        {
            Label_ValorTotal.Text = "";
        }else if (!string.IsNullOrEmpty(Entry_PrecioEntero.Text) && string.IsNullOrEmpty(Entry_PrecioDecimal.Text) &&
            !string.IsNullOrEmpty(Entry_Cantidad.Text))
        {
            var n1 = double.Parse(Entry_PrecioEntero.Text);
            var n2 = int.Parse(Entry_Cantidad.Text);
            var r = n1 * n2;
            Label_ValorTotal.Text = r.ToString();
        }
        else if (!string.IsNullOrEmpty(Entry_PrecioEntero.Text) && !string.IsNullOrEmpty(Entry_PrecioDecimal.Text) &&
            !string.IsNullOrEmpty(Entry_Cantidad.Text))
        {
            var n1 = double.Parse(Entry_PrecioEntero.Text + "." + Entry_PrecioDecimal.Text);
            var n2 = int.Parse(Entry_Cantidad.Text);
            var r = n1 * n2;
            Label_ValorTotal.Text = r.ToString();
        }else if (!string.IsNullOrEmpty(Entry_PrecioEntero.Text) && string.IsNullOrEmpty(Entry_PrecioDecimal.Text) &&
            string.IsNullOrEmpty(Entry_Cantidad.Text))
        {
            Label_ValorTotal.Text = "";
        }else if(string.IsNullOrEmpty(Entry_PrecioEntero.Text) && string.IsNullOrEmpty(Entry_PrecioDecimal.Text) &&
            !string.IsNullOrEmpty(Entry_Cantidad.Text))
        {
            Label_ValorTotal.Text = "";
        }
        _ejecutarTextChanged = true;
    }



    // LOGICA PARA EVENTOS
    private async Task PermitirPopAsyncNavegacion(bool mostrarAlerta)
    {
        if (mostrarAlerta)
        {
            //CompraPage._permitirEjecucion = false;
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
        if (_productos.Count == 0)
        {
            Picker_Producto.IsEnabled = false;
        }
        else
        {
            Border_PickerProductos.IsVisible = false;
            Picker_Producto.IsEnabled = true;
            Picker_Producto.ItemsSource = _productos;
        }

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
                    bool existeProducto = await VerificarExistenciaProductoDBAsync(nuevoProducto.Trim());
                    if (existeProducto)
                    {
                        await Toast.Make("El producto ya existe").Show();
                    }
                    else
                    {
                        await GuardarProductoDBAsync(nuevoProducto);
                        Picker_Producto.ItemsSource = await ObtenerProductosDBAsync();
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
                CANTIDAD = _cantidad,
                PRECIO = _precio,
                TOTAL = _total,
                SALDOPENDIENTE = _saldo
            };
            string mensaje = "PROVEEDOR :  " + compra.NOMBRE + " " + compra.APELLIDO +
                             "\nPRODUCTO   :  " + compra.PRODUCTO +
                             "\nCANTIDAD    :  " + "x" + compra.CANTIDAD +
                             "\nPRECIO        :  " + "$" + compra.PRECIO +
                             "\nTOTAL         :  " + "$" + compra.TOTAL +
                             "\nSALDO         :  " + "$" + compra.SALDOPENDIENTE;
            bool resultado = await DisplayAlert("Mensaje de confirmación", mensaje, "Confirmar", "Cancelar");
            if (resultado)
            {
                OcultarTeclado();
                VaciarCampos();
                await GuardarRegistroProductoDBAsync(compra);
                await DisplayAlert("AVISO", "El registro se ha guardado", "Aceptar");
            }
        }
        else
        {
            OcultarTeclado();
            await Toast.Make("Existen campos incompletos o erróneos").Show();
        }
    }
    private void EvitarPunto(Entry entry)
    {
        if (entry.Text.Contains('.'))
        {
            var a = entry.Text.Split('.');
            entry.Text = a[0] + a[1];
        }
    }
    private void CalcularTotal(Entry entry)
    {
        if (entry != Entry_SaldoPendienteEntero || entry != Entry_SaldoPendienteDecimal)
        {
            if (string.IsNullOrEmpty(Entry_Cantidad.Text))
            {
                Label_ValorTotal.Text = "";
            }
            else
            {
                if (!Entry_Cantidad.Text.Equals("."))
                {
                    if (Entry_Cantidad.Text.Contains("."))
                    {
                        var a = Entry_Cantidad.Text.Split(".");
                        var n = a[0] + a[1];
                        var p = _precio * int.Parse(n);
                        Label_ValorTotal.Text = p.ToString("F2");
                    }
                    else
                    {
                        var p = _precio * int.Parse(Entry_Cantidad.Text);
                        Label_ValorTotal.Text = p.ToString("F2");
                        _total = p;
                    }
                }
            }
        }
    }
    //private void CalcularTotalEntry_Cantidad(Entry entry)
    //{
    //    int cantidad;
    //    double resultado;



    //    // Eliminar cualquier caracter que no sea un número
    //    Entry_Cantidad.Text = new string(Entry_Cantidad.Text.Where(c => char.IsDigit(c)).ToArray());

    //    if (int.TryParse(Entry_Cantidad.Text, out cantidad))
    //    {
    //        //if (double.TryParse(Entry_Precio.Text, out precio))
    //        //{
    //        //    resultado = precio * cantidad;
    //        //    Label_ValorTotal.Text = resultado.ToString();
    //        //    return;
    //        //}
    //    }
    //    Label_ValorTotal.Text = "0";
    //}
    //private void SaldoPendiente()
    //{
    //    //if (Entry_SaldoPendiente.Text.Contains(".") && Entry_SaldoPendiente.Text.Split('.').Last().Length > 2)
    //    //{
    //    //    Entry_SaldoPendiente.Text = Entry_SaldoPendiente.Text.Substring(0, Entry_SaldoPendiente.Text.IndexOf(".") + 3);
    //    //}
    //}


    // LÓGICA
    private bool VerificarCamposValidos()
    {
        var itemProducto = (Tbl_Producto)Picker_Producto.SelectedItem;
        var precioE = Entry_PrecioEntero.Text;
        var precioD = Entry_PrecioDecimal.Text; 
        var cantidad = Entry_Cantidad.Text;
        var saldoE = Entry_SaldoPendienteEntero.Text;
        var saldoD = Entry_SaldoPendienteDecimal.Text;
        var total = Label_ValorTotal.Text;
        if (string.IsNullOrEmpty(precioE) && string.IsNullOrEmpty(precioD))
        {
            _precio = 0;
        }
        else if ((string.IsNullOrEmpty(precioE) && !string.IsNullOrEmpty(precioD)))
        {
            _precio = double.Parse("0." + precioD);
        }
        else if (!string.IsNullOrEmpty(precioE) && string.IsNullOrEmpty(precioD))
        {
            _precio = double.Parse(precioE);
        }
        else
        {
            _precio = double.Parse(precioE  + "." + precioD);
        }


        if (string.IsNullOrEmpty(cantidad))
        {
            _cantidad = 0;
        }
        else if (!string.IsNullOrEmpty(cantidad))
        {
            _cantidad = int.Parse(cantidad);
        }

        if (string.IsNullOrEmpty(total))
        {
            _total = 0;
        }
        else if (!string.IsNullOrEmpty(total))
        {
            _total = double.Parse(total);
        }

        if (string.IsNullOrEmpty(saldoE) && string.IsNullOrEmpty(saldoD))
        {
            if (!string.IsNullOrEmpty(total))
            {
                _ejecutarTextChanged = false;
                _saldo = 0;
                Entry_SaldoPendienteEntero.Text = "0";
                Entry_SaldoPendienteDecimal.Text = "00";
            }
        }
        else if ((string.IsNullOrEmpty(saldoE) && !string.IsNullOrEmpty(saldoD)))
        {
            _saldo = double.Parse("0." + saldoD);
            Entry_SaldoPendienteEntero.Text = "0";
        }
        else if (!string.IsNullOrEmpty(saldoE) && string.IsNullOrEmpty(saldoD))
        {
            _saldo = double.Parse(saldoE);
            //_ejecutarTextChanged = false;
            Entry_SaldoPendienteDecimal.Text = "00";
        }
        else
        {
            _saldo = double.Parse(saldoE + "." + saldoD);
        }


        if (itemProducto is null || _total == 0)
        {
            return false;
        }


        //if (string.IsNullOrEmpty(Entry_SaldoPendienteEntero.Text) &&
        //    !string.IsNullOrEmpty(Entry_SaldoPendienteDecimal.Text))
        //{
        //    if ((Entry_SaldoPendienteEntero.Text.Equals("0")))
        //    {
        //        string s = "0" + "." + Entry_SaldoPendienteDecimal.Text;
        //        _saldo = double.Parse(s);
        //    }
        //}
        //else if (!string.IsNullOrEmpty(Entry_SaldoPendienteEntero.Text) &&
        //    string.IsNullOrEmpty(Entry_SaldoPendienteDecimal.Text))
        //{
        //    string s = Entry_SaldoPendienteEntero.Text;
        //    _saldo = double.Parse(s);
        //}
        //else
        //{
        //    string s = Entry_SaldoPendienteEntero.Text + "." + Entry_SaldoPendienteDecimal.Text;
        //    _saldo = double.Parse(s);
        //}

        //if (string.IsNullOrEmpty(Entry_PrecioEntero.Text) || 
        //    !string.IsNullOrEmpty(Entry_PrecioDecimal.Text))
        //{
        //    if ((Entry_PrecioEntero.Text.Equals("0")))
        //    {
        //        string s = "0" + "." + Entry_PrecioDecimal.Text;
        //        _precio = double.Parse(s);
        //    }
            
        //}
        //else if (!string.IsNullOrEmpty(Entry_PrecioEntero.Text) ||
        //    string.IsNullOrEmpty(Entry_PrecioDecimal.Text))
        //{
        //    string s = Entry_PrecioEntero.Text;
        //    _precio = double.Parse(s);
        //}
        //else
        //{
        //    string s = Entry_PrecioEntero.Text + "." + Entry_PrecioDecimal.Text;
        //    _precio = double.Parse(s);
        //}
        //_cantidad = int.Parse(Entry_Cantidad.Text);
        //if (string.IsNullOrEmpty(Entry_Cantidad.Text) || _cantidad == 0 )
        //{
        //    _cantidad = 0;
        //}
        //if (string.IsNullOrEmpty(Label_ValorTotal.Text))
        //{
        //    return false;
        //}
        //if (_cantidad == 0)
        //{
        //    return false;
        //}
        //_total = double.Parse(Label_ValorTotal.Text);
        //if (_total == 0)
        //{
        //    return false;
        //}
        ////if (_precio is null || _cantidad is null || _total is null || _saldo is null)
        ////{

        ////}


        ////if (string.IsNullOrEmpty(producto) || string.IsNullOrEmpty(precio) || string.IsNullOrEmpty(cantidad) || string.IsNullOrEmpty(saldo))
        ////{
        ////    return false;
        ////}
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
        Entry_PrecioEntero.Unfocus();
        Entry_PrecioDecimal.Unfocus();
        Entry_Cantidad.Unfocus();
        Entry_SaldoPendienteEntero.Unfocus();
        Entry_SaldoPendienteDecimal.Unfocus();
    }
    private void VaciarCampos()
    {
        Picker_Producto.SelectedItem = null;
        Entry_PrecioEntero.Text = "";
        Entry_PrecioDecimal.Text = "";
        Entry_Cantidad.Text = "";
        Label_ValorTotal.Text = "";
        Entry_SaldoPendienteEntero.Text = "";
        Entry_SaldoPendienteDecimal.Text = "";
    }
    private void ControlarEntrysPrecio(Entry entry)
    {
        if (!string.IsNullOrEmpty(entry.Text))
        {
            if (entry.Text.Contains("."))
            {
                entry.Text = entry.Text.Replace(".", "");
            }
            else if (!string.IsNullOrEmpty(entry.Text))
            {
                if (entry.Text.Contains("."))
                {
                    entry.Text = entry.Text.Replace(".", "");
                }
            }
        }
        //if (entry == Entry_PrecioEntero || entry == Entry_PrecioDecimal)
        //{
        //    if (!entry.Text.Equals("."))
        //    {
        //        if (entry.Text.Contains("."))
        //        {
        //            if (string.IsNullOrEmpty(Entry_PrecioEntero.Text) &&
        //            string.IsNullOrEmpty(Entry_PrecioDecimal.Text))
        //            {
        //                _precio = 0;
        //            }
        //            else if (string.IsNullOrEmpty(Entry_PrecioEntero.Text) &&
        //                !string.IsNullOrEmpty(Entry_PrecioDecimal.Text))
        //            {
        //                var pDecimal = Entry_PrecioDecimal.Text.Split(".");
        //                string p = "0." + pDecimal[0] + pDecimal[1];
        //                _precio = double.Parse(p);
        //            }
        //            else if (!string.IsNullOrEmpty(Entry_PrecioEntero.Text) &&
        //                string.IsNullOrEmpty(Entry_PrecioDecimal.Text))
        //            {
        //                var pEntero = Entry_PrecioEntero.Text.Split(".");
        //                string p = pEntero[0] + pEntero[1];
        //                _precio = double.Parse(p);
        //            }
        //            else
        //            {
        //                if (Entry_PrecioEntero.Text.Contains("."))
        //                {
        //                    var pEntero = Entry_PrecioEntero.Text.Split(".");
        //                    string p1 = pEntero[0] + pEntero[1];
        //                    string p2 = Entry_PrecioDecimal.Text;
        //                    string p = p1 + "." + p2;
        //                    _precio = double.Parse(p);
        //                }
        //                else if (Entry_PrecioDecimal.Text.Contains("."))
        //                {

        //                    var pDecimal = Entry_PrecioDecimal.Text.Split(".");
        //                    string p1 = pDecimal[0] + pDecimal[1];
        //                    string p = "0." + p1;
        //                    _precio = double.Parse(p);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (string.IsNullOrEmpty(Entry_PrecioEntero.Text) &&
        //            string.IsNullOrEmpty(Entry_PrecioDecimal.Text))
        //            {
        //                _precio = 0;
        //            }
        //            else if (string.IsNullOrEmpty(Entry_PrecioEntero.Text) &&
        //                !string.IsNullOrEmpty(Entry_PrecioDecimal.Text))
        //            {
        //                string p = "0." + Entry_PrecioDecimal.Text;
        //                _precio = double.Parse(p);
        //            }
        //            else if (!string.IsNullOrEmpty(Entry_PrecioEntero.Text) &&
        //                string.IsNullOrEmpty(Entry_PrecioDecimal.Text))
        //            {
        //                string p = Entry_PrecioEntero.Text;
        //                _precio = double.Parse(p);
        //            }
        //            else
        //            {
        //                string p = Entry_PrecioEntero.Text + "." + Entry_PrecioDecimal.Text;
        //                _precio = double.Parse(p);
        //            }
        //        }
        //    }

        //}
    }

}