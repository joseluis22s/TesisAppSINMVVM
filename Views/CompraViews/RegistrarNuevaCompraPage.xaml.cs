using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Text.RegularExpressions;
using TesisAppSINMVVM.FirebaseDataBase.Repositories;
using TesisAppSINMVVM.Models;
using TesisAppSINMVVM.Views.ModalViews;

namespace TesisAppSINMVVM.Views.CompraViews;

public partial class RegistrarNuevaCompraPage : ContentPage
{
    private List<Producto> _productos;
    private bool _ejecutarTextChanged = true;
    private bool _enEjecucion;
    private int _cantidad;
    private double _precio;
    private double _total;
    private double _saldo;
    private double _abono;

    public RegistrarNuevaCompraPage()
    {
        InitializeComponent();
    }



    // NAVEGACIÓN
    #region NAVEGACIÓN
    private async Task HistorialComprasContentPagePushAsync()
    {
        await Navigation.PushAsync(new HistorialComprasPage
        {
            BindingContext = this.BindingContext
        });
    }
    private async Task RegistrarNuevaCompraPagePopAsync(bool mostrarAlerta)
    {
        ProveedoresPage._permitirEjecucion = false;
        await PermitirPopAsyncNavegacion(mostrarAlerta);
    }
    private async Task AgregarNuevoProductoPushModal()
    {
        await Navigation.PushModalAsync(new AgregarNuevoProductoPage());
    }
    #endregion


    // EVENTOS
    #region EVENTOS
    private async void ContentPage_Appearing(object sender, EventArgs e)
    {
        base.OnAppearing();
        await CargarDatosPicker_Productos();
    }
    private async void Button_AgregarNuevoProductoPicker_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await AgregarNuevoProductoPushModal();
        await CargarDatosPicker_Productos();
        // TODO: Revisar aquí si el picker se abre solo
        _enEjecucion = false;
    }
    private async void Button_GuardarNuevaCompra_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await AgregarNuevaCompra();
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
        await Border_PickerProductos_Pulsado();
    }
    private void Entrys_TextChanged(object sender, TextChangedEventArgs e)
    {
        //if(!_ejecutarTextChanged)
        //{
            //return;
        //}
        Entry entry = (Entry)sender;
        Entrys_TextoCambiado(entry);
    }
    #endregion



    // LÓGICA PARA EVENTOS
    #region LÓGICA PARA EVENTOS
    private async Task CargarDatosPicker_Productos()
    {
        _productos = await ObtenerProductosDBAsync();
        if (_productos.Count == 0)
        {
            Picker_Productos.IsEnabled = false;
        }
        else
        {
            Border_PickerProductos.IsVisible = false;
            Picker_Productos.IsEnabled = true;
            Picker_Productos.ItemsSource = _productos;
        }
    }
    private async Task AgregarNuevaCompra()
    {
        bool camposValidos = VerificarCamposValidos();
        if (camposValidos)
        {
            bool resultado1 = VerificarAbono();
            if (resultado1 != true)
            {
                string mensaje1 = "El valor de ABONO ($" + _abono + ") es mayor al valor TOTAL DE COMPRA ($" + _total + ")";
                bool resultado2 = await DisplayAlert("Mensaje de confirmación", mensaje1, "Confirmar", "Cancelar");
                if (!resultado2)
                {
                    return;
                }
            }
            var proveedorBinding = (Proveedor)BindingContext;
            var itemProducto = (Producto)Picker_Productos.SelectedItem;

            HistorialCompras compra = new HistorialCompras();
            compra.DIAFECHA = DateTime.Today.ToString("dddd, dd MMMM");
            compra.PROVEEDOR = proveedorBinding.PROVEEDOR;
            compra.PRODUCTO = itemProducto.PRODUCTO;
            compra.MEDIDA = itemProducto.MEDIDA;
            compra.FECHA = DateTime.Now.ToString("dd/MM/yyyy");
            compra.CANTIDAD = _cantidad;
            compra.PRECIO = _precio;
            compra.TOTAL = _total;
            compra.ABONO = _abono;
            compra.SALDOPENDIENTE = _saldo;

            string mensaje2 = "PROVEEDOR :  " + compra.PROVEEDOR +
                             "\nPRODUCTO   :  " + compra.PRODUCTO + " - " + compra.MEDIDA +
                             "\nCANTIDAD    :  " + "x" + compra.CANTIDAD +
                             "\nPRECIO         :  " + "$" + compra.PRECIO +
                             "\nTOTAL           :  " + "$" + compra.TOTAL +
                             "\nABONO          :  " + "$" + compra.ABONO +
                             "\nSALDO           :  " + "$" + compra.SALDOPENDIENTE;
            bool resultado3 = await DisplayAlert("Mensaje de confirmación", mensaje2, "Confirmar", "Cancelar");
            if (resultado3)
            {
                OcultarTeclado();
                VaciarCampos();
                await GuardarRegistroProductoDBAsync(compra);
                await Toast.Make("El registro se ha guardado", ToastDuration.Long).Show();
            }
            
        }
        else
        {
            OcultarTeclado();
            await Toast.Make("Existen campos incompletos o erróneos",ToastDuration.Long).Show();
        }
    }
    private async Task Border_PickerProductos_Pulsado()
    {
        if (_productos.Count == 0)
        {
            await Toast.Make("¡Primero agregue productos!", ToastDuration.Long).Show();
        }
    }
    private void Entrys_TextoCambiado(Entry entry)
    {
        CalcularTotal_TextChanged(entry);
        CalcularSaldo_TextChanged(entry);
        LimpiarPuntosEntry(entry);
    }
    
    #endregion


    // LÓGICA
    #region LÓGICA
    private bool VerificarAbono()
    {
        if (_abono > _total)
        {
            return false;
        }
        return true;
    }
    private bool VerificarCamposValidos()
    {
        var itemProducto = (Producto)Picker_Productos.SelectedItem;
        var precioE = Entry_PrecioEntero.Text;
        var precioD = Entry_PrecioDecimal.Text;
        var cantidad = Entry_Cantidad.Text;
        var abonoE = Entry_AbonoEntero.Text;
        var abonoD = Entry_AbonoDecimal.Text;
        string saldo;
        string total;
        if (!string.IsNullOrEmpty(Label_SaldoPendiente.Text))
        {
            saldo = Label_SaldoPendiente.Text.Replace('$', ' ');
        }
        else
        {
            saldo = Label_SaldoPendiente.Text;
        }
        if (!string.IsNullOrEmpty(Label_ValorTotal.Text))
        {
            total = Label_ValorTotal.Text.Replace('$', ' ');
        }
        else
        {
            total = Label_ValorTotal.Text;
        }
    
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
            _precio = double.Parse(precioE + "." + precioD);
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

        //if (string.IsNullOrEmpty(abonoE) && string.IsNullOrEmpty(abonoD))
        //{
        //    if (!string.IsNullOrEmpty(total))
        //    {
        //        _ejecutarTextChanged = false;
        //        _abono = 0;
        //        Entry_AbonoEntero.Text = "0";
        //        Entry_AbonoDecimal.Text = "00";
        //        _ejecutarTextChanged = true;
        //    }
        //}
        //else if ((string.IsNullOrEmpty(abonoE) && !string.IsNullOrEmpty(abonoD)))
            if ((string.IsNullOrEmpty(abonoE) && !string.IsNullOrEmpty(abonoD)))
        {
            _abono = double.Parse("0." + abonoD);
            Entry_AbonoEntero.Text = "0";
        }
        else if (!string.IsNullOrEmpty(abonoE) && string.IsNullOrEmpty(abonoD))
        {
            _abono = double.Parse(abonoE);
            //Entry_AbonoDecimal.Text = "00";
        }
        else
        {
            _abono = double.Parse(abonoE + "." + abonoD);
        }

        if (string.IsNullOrEmpty(saldo))
        {
            _saldo = 0;
        }
        else if (!string.IsNullOrEmpty(saldo))
        {
            _saldo = double.Parse(saldo);
        }

        if (itemProducto is null || _total == 0)
        {
            return false;
        }


        
        return true;
    }
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
    #endregion


    // BASE DE DATOS
    #region BASE DE DATOS
    private async Task<List<Producto>> ObtenerProductosDBAsync()
    {
        return await Producto_Repository.ObtenerProductosAsync();
    }
    private async Task GuardarRegistroProductoDBAsync(HistorialCompras registroCompra)
    {
        await HistorialCompras_Repository.GuardarRegistroProductoAsync(registroCompra);
    }

    #endregion


    // LÓGICA DE COSAS VISUALES DE LA PÁGINA
    #region LÓGICA DE COSAS VISUALES DE LA PÁGINA
    private void LimpiarPuntosEntry(Entry entry)
    {
        if (entry.Text.Contains("."))
        {
            entry.Text = Regex.Replace(entry.Text, @"\.", string.Empty);
        }
    }
    private void CalcularTotal_TextChanged(Entry entry)
    {
        double precio = 0;
        int cantidad = 0;
        //TODO: Meter condición general para veridicar si los
        //entrys son los de precios o cantiadad y evitar qu vaya a todas las condiciones
        if (entry == Entry_PrecioEntero || entry == Entry_PrecioDecimal || entry == Entry_Cantidad)
        {
            if (!string.IsNullOrEmpty(Entry_PrecioEntero.Text) && !string.IsNullOrEmpty(Entry_PrecioDecimal.Text))
            {
                precio = double.Parse(Entry_PrecioEntero.Text + "." + Entry_PrecioDecimal.Text);
            }
            else if (!string.IsNullOrEmpty(Entry_PrecioEntero.Text))
            {
                precio = double.Parse(Entry_PrecioEntero.Text);
            }
            else if (!string.IsNullOrEmpty(Entry_PrecioDecimal.Text))
            {
                precio = double.Parse("0." + Entry_PrecioDecimal.Text);
            }

            if (!string.IsNullOrEmpty(Entry_Cantidad.Text))
            {
                cantidad = int.Parse(Entry_Cantidad.Text);
            }

            if (precio > 0 && cantidad > 0)
            {
                var valorTotal = precio * cantidad;
                Label_ValorTotal.Text = "$ " + valorTotal.ToString("F2");
            }
            else
            {
                Label_ValorTotal.Text = string.Empty;
            }
    }
}
    private void CalcularSaldo_TextChanged(Entry entry)
    {
        double abono = 0;
        double total = 0;
        //TODO: Meter condición general para veridicar si los
        //entrys son los de precios o cantiadad y evitar qu vaya a todas las condiciones
        if (!string.IsNullOrEmpty(Label_ValorTotal.Text) && (entry == Entry_AbonoEntero || entry == Entry_AbonoDecimal))
        {
            if (!string.IsNullOrEmpty(Entry_AbonoEntero.Text) && !string.IsNullOrEmpty(Entry_AbonoDecimal.Text))
            {
                abono = double.Parse(Entry_AbonoEntero.Text + "." + Entry_AbonoDecimal.Text);
            }
            else if (!string.IsNullOrEmpty(Entry_AbonoEntero.Text) && string.IsNullOrEmpty(Entry_AbonoDecimal.Text))
            {
                abono = double.Parse(Entry_AbonoEntero.Text);
            }
            else if (string.IsNullOrEmpty(Entry_AbonoEntero.Text) && !string.IsNullOrEmpty(Entry_AbonoDecimal.Text))
            {
                abono = double.Parse("0." + Entry_AbonoDecimal.Text);
            }

            if (!string.IsNullOrEmpty(Label_ValorTotal.Text))
            {
                total = double.Parse(Label_ValorTotal.Text.Replace('$', ' '));
            }

            if (abono > 0 && total > 0)
            {
                double saldo = total - abono;
                Label_SaldoPendiente.Text = "$ " + saldo.ToString("F2");
            }
            else
            {
                Label_SaldoPendiente.Text = string.Empty;
            }
        }
    }
    private void OcultarTeclado()
    {
        Entry_PrecioEntero.Unfocus();
        Entry_PrecioDecimal.Unfocus();
        Entry_Cantidad.Unfocus();
        Entry_AbonoEntero.Unfocus();
        Entry_AbonoDecimal.Unfocus();
    }
    private void VaciarCampos()
    {
        Picker_Productos.SelectedItem = null;
        Entry_PrecioEntero.Text = "";
        Entry_PrecioDecimal.Text = "";
        Entry_Cantidad.Text = "";
        Label_ValorTotal.Text = "";
        Entry_AbonoEntero.Text = "";
        Entry_AbonoDecimal.Text = "";
        Label_SaldoPendiente.Text = "";
    }
    #endregion

}