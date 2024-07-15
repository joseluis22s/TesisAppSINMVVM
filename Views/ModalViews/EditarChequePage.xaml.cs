using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Collections.ObjectModel;
using TesisAppSINMVVM.FirebaseDataBase.Repositories;
using TesisAppSINMVVM.LocalDatabase.Tables;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.Views.ModalViews;

public partial class EditarChequePage : ContentPage
{
    private Cheque_Repository _repoCheque = new Cheque_Repository(); private Proveedor_Repository _repoProveedor = new Proveedor_Repository();
    private List<Tbl_Proveedor> _tblProveedores;
    private Cheque _cheque = new Cheque();
    private bool _enEjecucion;
    public EditarChequePage()
    {
        InitializeComponent();
    }
    // NAVEGACI�N
    #region NAVEGACI�N

    #endregion


    // EVENTOS
    #region EVENTOS
    private async void ContentPage_Appearing(object sender, EventArgs e)
    {
        var tblCheque = (Tbl_Cheque)this.BindingContext;
        Cheque cheque = new Cheque();
        cheque.NUMERO = tblCheque.NUMERO;
        cheque.MONTO = tblCheque.MONTO;
        cheque.PROVEEDOR = tblCheque.PROVEEDOR;
        cheque.FECHACOBRO = tblCheque.FECHACOBRO;
        cheque.FECHAEMISION = tblCheque.FECHAEMISION;
        cheque.DIAFECHACOBRO = tblCheque.DIAFECHACOBRO;

        _cheque = cheque;
        await CargarPickerInformacionAsync();
        await CargarDatosChequesAsync();
    }
    private void Entry_MontoCheque_TextChanged(object sender, TextChangedEventArgs e)
    {

    }
    private async void Button_GuardarCambios_Clicked(object sender, EventArgs e)
    {

        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        NetworkAccess accessType = Connectivity.Current.NetworkAccess;
        if (accessType == NetworkAccess.Internet)
        {
            Cheque nuevoCheque = ExtraerNuevosDatos();
            bool existenCambios = ComprobarCambios(_cheque, nuevoCheque);
            if (existenCambios)
            {
                await EditarChequeDBasync(_cheque, nuevoCheque);
            }
            else
            {
                await Toast.Make("No se han realizado cambios", ToastDuration.Long).Show();
            }
        }
        else
        {
            await Toast.Make("Primero debe conectarse a internet", ToastDuration.Long).Show();
        }
        _enEjecucion = false;
    }
    #endregion


    // L�GICA PARA EVENTOS
    #region L�GICA PARA EVENTOS
    private async Task CargarPickerInformacionAsync()
    {
        _tblProveedores = await ObtenerProveedoresDBAsync();
        Picker_Proveedores.ItemsSource = _tblProveedores;
    }
    private bool ComprobarCambios(Cheque cheque1, Cheque cheque2)
    {
        if ((cheque1.MONTO == cheque2.MONTO) && (cheque1.PROVEEDOR == cheque2.PROVEEDOR) &&
            (cheque1.FECHACOBRO == cheque2.FECHACOBRO) && (cheque1.FECHAEMISION == cheque2.FECHAEMISION) &&
            (cheque1.DIAFECHACOBRO == cheque2.DIAFECHACOBRO))
        {
            return false;
        }
        return true;
    }
    #endregion


    // L�GICA
    #region L�GICA
    private async Task CargarDatosChequesAsync()
    {
        Entry_NumeroCheque.Text = _cheque.NUMERO.ToString();
        string monto = _cheque.MONTO.ToString();
        var montoED = monto.Split('.');
        if (montoED.Length == 1)
        {
            Entry_MontoChequeEntero.Text = montoED[0].ToString();
            Entry_MontoChequeDecimal.Text = "0";
        }
        else
        {
            Entry_MontoChequeEntero.Text = montoED[0].ToString();
            Entry_MontoChequeDecimal.Text = montoED[1].ToString();
        }

        var p = Picker_Proveedores.Items;
        int posicion = p.IndexOf(_cheque.PROVEEDOR);
        Picker_Proveedores.SelectedIndex = posicion;

        DatePicker_FechaCobro.Date = DateTime.Parse(_cheque.FECHACOBRO);
        DatePicker_FechaEmision.Date = DateTime.Parse(_cheque.FECHAEMISION);
    }
    private Cheque ExtraerNuevosDatos()
    {
        Cheque nuevoCheque = new Cheque();
        nuevoCheque.NUMERO = int.Parse(Entry_NumeroCheque.Text);
        nuevoCheque.MONTO = double.Parse(Entry_MontoChequeEntero.Text + "." + Entry_MontoChequeDecimal.Text);
        var prove = (Tbl_Proveedor)Picker_Proveedores.SelectedItem;
        nuevoCheque.PROVEEDOR = prove.PROVEEDOR;
        nuevoCheque.FECHACOBRO = DatePicker_FechaCobro.Date.ToString("dd/MM/yyyy");
        nuevoCheque.FECHAEMISION = DatePicker_FechaEmision.Date.ToString("dd/MM/yyyy");
        nuevoCheque.DIAFECHACOBRO = DatePicker_FechaCobro.Date.ToString("dddd, dd MMMM yyyy");
        return nuevoCheque;
    }
    #endregion


    // BASE DE DATOS
    #region BASE DE DATOS
    private async Task EditarChequeDBasync(Cheque cheque1, Cheque cheque2)
    {
        await _repoCheque.EditarRegistroCheque(cheque1, cheque2);
    }
    private async Task<List<Tbl_Proveedor>> ObtenerProveedoresDBAsync()
    {
        return await _repoProveedor.ObtenerProveedoresAsync();
    }
    #endregion


    // L�GICA DE COSAS VISUALES DE LA P�GINA
    #region L�GICA DE COSAS VISUALES DE LA P�GINA

    #endregion

}