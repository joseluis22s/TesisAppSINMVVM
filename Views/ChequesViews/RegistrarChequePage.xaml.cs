using CommunityToolkit.Maui.Alerts;
using System.Runtime.Serialization;
using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.Database.Tables;

namespace TesisAppSINMVVM.Views.ChequesViews;

public partial class RegistrarChequePage : ContentPage
{

    private Tbl_Proveedor_Respository _repoProveedor;
    private Tbl_Cheque_Repository _repoCheque;
    private List<Tbl_Proveedor> _proveedores;
    private bool _enEjecucion;
    private double _montoCheque;

    public RegistrarChequePage()
    {
        InitializeComponent();
        _repoProveedor = new Tbl_Proveedor_Respository();
        _repoCheque = new Tbl_Cheque_Repository();
    }



    // NAVEGACIÓN
    private async Task RegistrarChequePagePopAsync(bool mostrarAlerta)
    {
        await PermitirPopAsyncNavegacion(mostrarAlerta);
    }


    // EVENTOS
    private async void ContentPage_Appearing(object sender, EventArgs e)
    {
        base.OnAppearing();
        await CargarPickerInformacionAsync();
    }
    private async void Button_GuardarNuevoCheque_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await GuardarRegistroChequeAsync();
        _enEjecucion = false;
    }
    private void Entry_NumeroCheque_TextChanged(object sender, TextChangedEventArgs e)
    {
        NumeroCheque_TextChanged();
    }
    private void Entry_MontoCheque_TextChanged(object sender, TextChangedEventArgs e)
    {
        MontoCheque_TextChanged();
    }
    protected override bool OnBackButtonPressed()
    {
        RegistrarChequePagePopAsync(true).GetAwaiter();
        return true;
    }
    private async void Button_Regresar_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await RegistrarChequePagePopAsync(true);
        _enEjecucion = false;
    }


    // LOGICA PARA EVENTOS
    private async Task CargarPickerInformacionAsync()
    {
        _proveedores = await ObtenerProveedoresDBAsync();
        Picker_Proveedores.ItemsSource = _proveedores;
    }
    private string ControlarCamposvalidosCargarCheque()
    {
        string numerocheque = Entry_NumeroCheque.Text;
        string montochequeEntero = Entry_MontoChequeEntero.Text;
        string montochequeDecimal = Entry_MontoChequeDecimal.Text;
        var itemProveedor = (Tbl_Proveedor)Picker_Proveedores.SelectedItem;
        if (string.IsNullOrEmpty(montochequeEntero) && string.IsNullOrEmpty(montochequeDecimal))
        {
            _montoCheque = 0;
        }
        else if(string.IsNullOrEmpty(montochequeEntero) && !string.IsNullOrEmpty(montochequeDecimal))
        {
            _montoCheque = double.Parse("0." + montochequeDecimal);
        }
        else if (!string.IsNullOrEmpty(montochequeEntero) && string.IsNullOrEmpty(montochequeDecimal))
        {
            _montoCheque = double.Parse(montochequeEntero);
        }
        else
        {
            _montoCheque = double.Parse(montochequeEntero + "." + montochequeDecimal);
        }
        //m = double.Parse(montochequeEntero + "." + montochequeDecimal);
        if (itemProveedor is null || string.IsNullOrEmpty(numerocheque) || 
            //string.IsNullOrEmpty(montochequeEntero) || string.IsNullOrEmpty(montochequeDecimal) ||
            _montoCheque == 0 )
        {
            return "Existen campos incompletos o erróneos";
        }
        return "true";
    }
    private async Task GuardarRegistroChequeAsync()
    {
        string resultado = ControlarCamposvalidosCargarCheque();
        if (resultado == "true")
        {
            var itemProveedor = (Tbl_Proveedor)Picker_Proveedores.SelectedItem;
            Tbl_Cheque cheque = new Tbl_Cheque()
            {
                NUMERO = int.Parse(Entry_NumeroCheque.Text),
                MONTO = double.Parse(Entry_MontoChequeEntero.Text + "." + Entry_MontoChequeDecimal.Text),
                PROVEEDOR = itemProveedor.NOMBRE + " " + itemProveedor.APELLIDO,
                FECHA = DatePicker_FechasEmision.Date.ToString("dd/MM/yyyy"),
                DIAFECHA = DatePicker_FechasEmision.Date.ToString("dddd, dd MMMM")
            };
            bool existeCheque = await VerificarExistenciaChequeDBAsync(cheque.NUMERO);
            if (!existeCheque)
            {
                
                string mensaje = "CHEQUE       :  #" + cheque.NUMERO +
                               "\nMONTO         :  $" + cheque.MONTO +
                               "\nPROVEEDOR :  " + cheque.PROVEEDOR +
                               "\nFECHA          :  " + cheque.FECHA;
                bool resultado1 = await DisplayAlert("Mensaje de confirmación", mensaje, "Confirmar", "Cancelar");
                if (resultado1)
                {
                    OcultarTeclado();
                    await GuardarChequeDBAsync(cheque);
                    ReiniciarCampos();
                    await Toast.Make("¡Registro guardado!").Show();
                }
            }
            else
            {
                OcultarTeclado();
                await Toast.Make("Cheque registrado anteriormente").Show();
            }

        }
        else
        {
            OcultarTeclado();
            await Toast.Make(resultado).Show();
        }
    }
    private async Task PermitirPopAsyncNavegacion(bool mostrarAlerta)
    {
        if (mostrarAlerta)
        {
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


    // BASE DE DATOS
    private async Task<List<Tbl_Proveedor>> ObtenerProveedoresDBAsync()
    {
        return await _repoProveedor.ObtenerProveedoresAsync();
    }
    private async Task GuardarChequeDBAsync(Tbl_Cheque cheque)
    {
        await _repoCheque.GuardarChequeAsync(cheque);
    }
    private async Task<bool> VerificarExistenciaChequeDBAsync(int numeroCheque)
    {
        return await _repoCheque.VerificarExistenciaChequeAsync(numeroCheque);
    }



    // LÓGICA DE COSAS VISUALES DE LA PÁGINA
    /*TODO: ¿Hacerlos TASK?*/
    private void NumeroCheque_TextChanged()
    {
        // Solo permite digitar enteros
        if (Entry_NumeroCheque.Text.Contains("."))
        {
            Entry_NumeroCheque.Text = Entry_NumeroCheque.Text.Replace(".", "");
        }
    }
    private void MontoCheque_TextChanged()
    {
        
        if (!string.IsNullOrEmpty(Entry_MontoChequeEntero.Text) )
        {
            if (Entry_MontoChequeEntero.Text.Contains("."))
            {
                Entry_MontoChequeEntero.Text = Entry_MontoChequeEntero.Text.Replace(".", "");
            }
            else if (!string.IsNullOrEmpty(Entry_MontoChequeDecimal.Text))
            {
                if (Entry_MontoChequeDecimal.Text.Contains("."))
                {
                    Entry_MontoChequeDecimal.Text = Entry_MontoChequeDecimal.Text.Replace(".", "");
                }
                
            }
        }
        else if (!string.IsNullOrEmpty(Entry_MontoChequeDecimal.Text))
        {
            if (Entry_MontoChequeDecimal.Text.Contains("."))
            {
                Entry_MontoChequeDecimal.Text = Entry_MontoChequeDecimal.Text.Replace(".", "");
            }
            else if (!string.IsNullOrEmpty(Entry_MontoChequeEntero.Text))
            {
                Entry_MontoChequeEntero.Text = Entry_MontoChequeEntero.Text.Replace(".", "");
            }
        }

    }
    private void ReiniciarCampos()
    {
        Entry_NumeroCheque.Text = "";
        Entry_MontoChequeEntero.Text = "";
        Entry_MontoChequeDecimal.Text = "";
        Picker_Proveedores.SelectedIndex = -1;
        DatePicker_FechasEmision.Date = DateTime.Today;
    }
    private void OcultarTeclado()
    {
        Entry_NumeroCheque.Unfocus();
        Entry_MontoChequeEntero.Unfocus();
        Entry_MontoChequeDecimal.Unfocus();
    }

    
}