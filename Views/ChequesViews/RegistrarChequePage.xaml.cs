using CommunityToolkit.Maui.Alerts;
using TesisAppSINMVVM.Models;
using TesisAppSINMVVM.Database.Tables;
using TesisAppSINMVVM.FirebaseDataBase.Repositories;

namespace TesisAppSINMVVM.Views.ChequesViews;

public partial class RegistrarChequePage : ContentPage
{
    private List<Proveedor> _proveedores;
    private bool _enEjecucion;
    private double _montoCheque;

    public RegistrarChequePage()
    {
        InitializeComponent();
    }

    // NAVEGACIÓN
    #region NAVEGACIÓN
    private async Task RegistrarChequePagePopAsync(bool mostrarAlerta)
    {
        await PermitirPopAsyncNavegacion(mostrarAlerta);
    }
    #endregion


    // EVENTOS
    #region EVENTOS
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
    #endregion

    // LÓGICA PARA EVENTOS
    #region LÓGICA PARA EVENTOS
    private async Task CargarPickerInformacionAsync()
    {
        _proveedores = await ObtenerProveedoresDBAsync();
        Picker_Proveedores.ItemsSource = _proveedores;
    }
    
    private async Task GuardarRegistroChequeAsync()
    {
        string resultado = ControlarCamposvalidosCargarCheque();
        if (resultado == "true")
        {
            var itemProveedor = (Proveedor)Picker_Proveedores.SelectedItem;
            Cheque cheque = new Cheque()
            {
                NUMERO = int.Parse(Entry_NumeroCheque.Text),
                MONTO = double.Parse(Entry_MontoChequeEntero.Text + "." + Entry_MontoChequeDecimal.Text),
                PROVEEDOR = itemProveedor.PROVEEDOR,
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
                    await GuardarNuevoResgitroChequesDBAsync(cheque);
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

    #endregion

    // LÓGICA
    #region LÓGICA
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
    //private async Task VerificarExistenciaCheque
    private string ControlarCamposvalidosCargarCheque()
    {
        string numerocheque = Entry_NumeroCheque.Text;
        string montochequeEntero = Entry_MontoChequeEntero.Text;
        string montochequeDecimal = Entry_MontoChequeDecimal.Text;
        var itemProveedor = (Proveedor)Picker_Proveedores.SelectedItem;
        if (string.IsNullOrEmpty(montochequeEntero) && string.IsNullOrEmpty(montochequeDecimal))
        {
            _montoCheque = 0;
        }
        else if (string.IsNullOrEmpty(montochequeEntero) && !string.IsNullOrEmpty(montochequeDecimal))
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
            _montoCheque == 0)
        {
            return "Existen campos incompletos o erróneos";
        }
        return "true";
    }
    #endregion

    // BASE DE DATOS
    #region BASE DE DATOS
    private async Task<List<Proveedor>> ObtenerProveedoresDBAsync()
    {
        return await Proveedor_Repository.ObtenerProveedoresAsync();
    }
    private async Task GuardarNuevoResgitroChequesDBAsync(Cheque cheque)
    {
        await Cheque_Repository.GuardarNuevoResgitroChequesAsync(cheque);
    }
    private async Task<bool> VerificarExistenciaChequeDBAsync(int numeroCheque)
    {
        return await Cheque_Repository.VerificarExistenciaChequeAsync(numeroCheque);
    }
    #endregion

    // LÓGICA DE COSAS VISUALES DE LA PÁGINA
    #region LÓGICA DE COSAS VISUALES DE LA PÁGINA
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

        if (!string.IsNullOrEmpty(Entry_MontoChequeEntero.Text))
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
    #endregion

}