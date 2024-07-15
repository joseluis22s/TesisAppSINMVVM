using CommunityToolkit.Maui.Alerts;
using TesisAppSINMVVM.Models;
using TesisAppSINMVVM.FirebaseDataBase.Repositories;
using TesisAppSINMVVM.LocalDatabase.Tables;
using CommunityToolkit.Maui.Core;

namespace TesisAppSINMVVM.Views.ChequesViews;

public partial class RegistrarChequePage : ContentPage
{
    private Cheque_Repository _repoCheque = new Cheque_Repository();
    private Proveedor_Repository _repoProveedor = new Proveedor_Repository();
    private List<Tbl_Proveedor> _tblProveedores;
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
    private async Task NavegarPaginaPrincipalPagePopToRootAsync()
    {
        await Navigation.PopToRootAsync();
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
    private async void ImageButton_Home_Clicked(object sender, EventArgs e)
    {
        await NavegarPaginaPrincipalPagePopToRootAsync();
    }
    #endregion

    // LÓGICA PARA EVENTOS
    #region LÓGICA PARA EVENTOS
    private async Task CargarPickerInformacionAsync()
    {
        _tblProveedores = await ObtenerProveedoresDBAsync();
        Picker_Proveedores.ItemsSource = _tblProveedores;
    }
    private async Task GuardarRegistroChequeAsync()
    {
        NetworkAccess accessType = Connectivity.Current.NetworkAccess;
        if (accessType == NetworkAccess.Internet)
        {
            string resultado = ControlarCamposvalidosCargarCheque();
            if (resultado == "true")
            {
                var itemProveedor = (Tbl_Proveedor)Picker_Proveedores.SelectedItem;
                Cheque cheque = new Cheque()
                {
                    NUMERO = int.Parse(Entry_NumeroCheque.Text),
                    MONTO = double.Parse(Entry_MontoChequeEntero.Text + "." + Entry_MontoChequeDecimal.Text),
                    PROVEEDOR = itemProveedor.PROVEEDOR,
                    FECHACOBRO = DatePicker_FechaCobro.Date.ToString("dd/MM/yyyy"),
                    FECHAEMISION = DatePicker_FechaEmision.Date.ToString("dd/MM/yyyy"),
                    DIAFECHACOBRO = DatePicker_FechaCobro.Date.ToString("dddd, dd MMMM yyyy")
                };
                bool existeCheque = await VerificarExistenciaChequeDBAsync(cheque.NUMERO);
                if (!existeCheque)
                {

                    string mensaje = "CHEQUE                    :  #" + cheque.NUMERO +
                                   "\nMONTO                     :  $" + cheque.MONTO +
                                   "\nPROVEEDOR             :  " + cheque.PROVEEDOR +
                                   "\nFECHA DE COBRO    :  " + cheque.FECHACOBRO +
                                   "\nFECHA DE EMISIÓN :  " + cheque.FECHAEMISION;
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
        else
        {
            await Toast.Make("Primero debe conectarse a internet", ToastDuration.Long).Show();
        }
        
    }

    #endregion

    // LÓGICA
    #region LÓGICA
    private async Task PermitirPopAsyncNavegacion(bool mostrarAlerta)
    {
        bool camposVacios = VerificarCamposVacios();
        if (!camposVacios)
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
    private bool VerificarCamposVacios()
    {
        var numerocheque = Entry_NumeroCheque.Text;
        var monetoE = Entry_MontoChequeEntero.Text;
        var montoD = Entry_MontoChequeDecimal.Text;
        var proveedorItem = (Tbl_Proveedor)Picker_Proveedores.SelectedItem;
        if (proveedorItem is null && string.IsNullOrEmpty(numerocheque) && 
            string.IsNullOrEmpty(monetoE) && string.IsNullOrEmpty(montoD))
        {
            return true;
        }
        return false;
    }
    //private async Task VerificarExistenciaCheque
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
    private async Task<List<Tbl_Proveedor>> ObtenerProveedoresDBAsync()
    {
        return await _repoProveedor.ObtenerProveedoresAsync();
    }
    private async Task GuardarNuevoResgitroChequesDBAsync(Cheque cheque)
    {
        await _repoCheque.GuardarNuevoResgitroChequesAsync(cheque);
    }
    private async Task<bool> VerificarExistenciaChequeDBAsync(int numeroCheque)
    {
        return await _repoCheque.VerificarExistenciaChequeAsync(numeroCheque);
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
        DatePicker_FechaCobro.Date = DateTime.Today;
        DatePicker_FechaEmision.Date = DateTime.Today;
    }
    private void OcultarTeclado()
    {
        Entry_NumeroCheque.Unfocus();
        Entry_MontoChequeEntero.Unfocus();
        Entry_MontoChequeDecimal.Unfocus();
    }
    #endregion
}