using CommunityToolkit.Maui.Alerts;
using System.Runtime.Serialization;
using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.Database.Tables;

namespace TesisAppSINMVVM.Views.ChequesViews;

public partial class RegistarChequePage : ContentPage
{

    private Tbl_Proveedor_Respository _repoProveedor;
    private Tbl_Cheque_Repository _repoCheque;
    private List<Tbl_Proveedor> _proveedores;

    public RegistarChequePage()
    {
        InitializeComponent();
        _repoProveedor = new Tbl_Proveedor_Respository();
        _repoCheque = new Tbl_Cheque_Repository();
    }



    // NAVEGACI�N


    // EVENTOS
    private async void ContentPage_Appearing(object sender, EventArgs e)
    {
        base.OnAppearing();
        await CargarPickerInformacionAsync();
    }
    private async void Button_GuardarNuevoCheque_Clicked(object sender, EventArgs e)
    {
        await GuardarRegistroChequeAsync();
    }
    private void Entry_NumeroCheque_TextChanged(object sender, TextChangedEventArgs e)
    {
        NumeroCheque_TextChanged();
    }
    private void Entry_MontoCheque_TextChanged(object sender, TextChangedEventArgs e)
    {
        MontoCheque_TextChanged();
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
        string montocheque = Entry_MontoCheque.Text;
        var itemProveedor = (Tbl_Proveedor)Picker_Proveedores.SelectedItem;

        if (string.IsNullOrEmpty(numerocheque) || itemProveedor is null || 
            string.IsNullOrEmpty(montocheque) )
        {
            return "�Llene todo los campos!";
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
                MONTO = double.Parse(Entry_MontoCheque.Text),
                PROVEEDOR = itemProveedor.NOMBRE + " " + itemProveedor.APELLIDO,
                FECHA = DatePicker_FechasEmision.Date.ToString("dd/MM/yyyy"),
                DIAFECHA = DatePicker_FechasEmision.Date.ToString("dddd, dd MMMM")
            };
            bool existeCheque = await VerificarExistenciaChequeDBAsync(cheque.NUMERO);
            if (!existeCheque)
            {
                
                string mensaje = "N�MERO       :  " + cheque.NUMERO +
                               "\nMONTO         :  " + cheque.MONTO +
                               "\nPROVEEDOR :  " + cheque.PROVEEDOR +
                               "\nFECHA          :  " + cheque.FECHA;
                bool resultado1 = await DisplayAlert("Mensaje de confirmaci�n", mensaje, "Confirmar", "Cancelar");
                if (resultado1)
                {
                    ReiniciarCamposOcultarTeclado();
                    await GuardarChequeDBAsync(cheque);
                    await Toast.Make("�Registro guardado!").Show();
                }
            }
            else
            {
                await Toast.Make("Cheque registrado anteriormente").Show();
            }

        }
        else
        {
            await Toast.Make(resultado).Show();
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



    // L�GICA DE COSAS VISUALES DE LA P�GINA
    /*TODO: �Hacerlos TASK?*/
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
        // Solo permite digitar con 2 decimales y un soslo punto decimal
        if (Entry_MontoCheque.Text.Contains(".") && Entry_MontoCheque.Text.Split('.').Last().Length > 2)
        {
            Entry_MontoCheque.Text = Entry_MontoCheque.Text.Substring(0, Entry_MontoCheque.Text.IndexOf(".") + 3);
        }
    }
    private void ReiniciarCamposOcultarTeclado()
    {
        Entry_NumeroCheque.Unfocus();
        Entry_NumeroCheque.Text = "";
        Entry_MontoCheque.Unfocus();
        Entry_MontoCheque.Text = "";
        Picker_Proveedores.SelectedIndex = -1;
        DatePicker_FechasEmision.Date = DateTime.Today;
    }
}