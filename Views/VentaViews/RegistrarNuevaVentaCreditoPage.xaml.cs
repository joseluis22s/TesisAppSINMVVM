using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Platform;
using System.Text.RegularExpressions;
using TesisAppSINMVVM.Database.Tables;
using TesisAppSINMVVM.FirebaseDataBase.Repositories;
using TesisAppSINMVVM.Models;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace TesisAppSINMVVM.Views.VentaViews;

public partial class RegistrarNuevaVentaCreditoPage : ContentPage
{
    private Comprador_Repository _repoComprador = new Comprador_Repository();
    private bool _enEjecucion;
    //private bool _paginaCargada = false;
    private double _montoVendido;
    private List<Comprador> _compradores;

    public static bool _ejecutarAppearing = true;

    public RegistrarNuevaVentaCreditoPage()
    {
        InitializeComponent();
    }



    // NAVEGACI�N
    #region NAVEGACI�N
    private async Task HistorialVentasCreditoPushAsync()
    {
        await Navigation.PushAsync(new HistorialVentasCredito());
    }
    private async Task RegistrarVentaCreditoPagePopAsync()
    {
        await PermitirPopAsyncNavegacion();
    }
    #endregion


    // EVENTOS
    #region EVENTOS
    private async void ContentPage_Appearing(object sender, EventArgs e)
    {
        //TODO: Eliminar p�gina creada y su condici�n, no se pa que se usa
        //if (!_paginaCargada)
        //{
            base.OnAppearing();
            await CargarDatosPicker_Comprador();
        //    _paginaCargada = true;
        //}
    }
    private async void Button_AgregarComprador_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await AgregarNuevoComprador();
        _enEjecucion = false;
    }
    private async void Button_HistorialVentasCredito_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await HistorialVentasCreditoPushAsync();
        _enEjecucion = false;
    }
    private async void Button_GuardarVentaCredito_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await GuardarCompraCreditoAsync();
        _enEjecucion = false;
    }
    private async void Button_Regresar_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await RegistrarVentaCreditoPagePopAsync();
        _enEjecucion = false;
    }
    protected override bool OnBackButtonPressed()
    {
        RegistrarVentaCreditoPagePopAsync().GetAwaiter();
        return true;
    }
    private void Entry_MontoVendido_TextChanged(object sender, TextChangedEventArgs e)
    {
        Entry entry = (Entry)sender;
        MontoVendido_TextoCambiado(entry);
    }
    #endregion


    // L�GICA PARA EVENTOS
    #region L�GICA PARA EVENTOS
    private async Task AgregarNuevoComprador()
    {
        string nuevoComprador;
        do
        {
            nuevoComprador = await DisplayPromptAsync("NUEVO COMPRADOR", "Ingrese el nombre del nuevo comprador:", "AGREGAR", "CANCELAR", null, -1, Keyboard.Create(KeyboardFlags.CapitalizeCharacter));
            if (nuevoComprador is not null)
            {
                if (nuevoComprador != "")
                {
                    nuevoComprador = nuevoComprador.Trim().ToUpper();
                    bool existeProveedor = _compradores.Any(p => p.COMPRADOR == nuevoComprador);
                    if (!existeProveedor)
                    {
                        Comprador comprador = new Comprador();
                        comprador.COMPRADOR = nuevoComprador;
                        await GuardarNuevoCompradorDBAsync(comprador);
                        await CargarDatosPicker_Comprador();
                        await Toast.Make("Se ha guardado el nuevo comprador", ToastDuration.Long).Show();
                    }
                    else
                    {
                        await Toast.Make("Comprador ya existente", ToastDuration.Long).Show();
                    }
                }
                else
                {
                    await Toast.Make("El campo no debe estar vac�o").Show();
                }
            }
        } while (nuevoComprador == "");
    }
    private async Task GuardarCompraCreditoAsync()
    {
        string resultado1 = ControlarCamposGuardarCompraCredito();
        if (resultado1 == "true")
        {
            var itemComprador = (Comprador)Picker_Comprador.SelectedItem;
            VentaCredito ventaCredito = new VentaCredito()
            {
                COMPRADOR = itemComprador.COMPRADOR,
                MONTOVENDIDO = _montoVendido,
                DESCRIPCION = Entry_Descripcion.Text,
                FECHAGUARDADO = DateTime.Now.ToString("dd/MM/yyyy"),
                DIAFECHAGUARDADO = DateTime.Now.ToString("dddd, dd MMMM")
            };
            string mensaje = "COMPRADOR        : " + ventaCredito.COMPRADOR +
                             "\nMONTO VENDIDO : $" + ventaCredito.MONTOVENDIDO +
                             "\nDESCRIPCI�N       : " + ventaCredito.DESCRIPCION;
            bool resultado2 = await DisplayAlert("Mensaje de confirmaci�n", mensaje, "Confirmar", "Cancelar");
            if (resultado2)
            {
                await OcultarTeclado();
                await GuardarVentaCreditoDBAsync(ventaCredito);
                await Toast.Make("�Registro guardado!").Show();
                LimpiarCampos();
            }
        }
        else
        {
            await OcultarTeclado();
            await Toast.Make(resultado1).Show();
        }
    }
    private void MontoVendido_TextoCambiado(Entry entry)
    {
        LimpiarPuntosEntry(entry);
        //if (!string.IsNullOrEmpty(Entry_MontoVendidoEntero.Text))
        //{
        //    if (Entry_MontoVendidoEntero.Text.Contains("."))
        //    {
        //        Entry_MontoVendidoEntero.Text = Entry_MontoVendidoEntero.Text.Replace(".", "");
        //    }
        //    else if (!string.IsNullOrEmpty(Entry_MontoVendidoDecimal.Text))
        //    {
        //        if (Entry_MontoVendidoDecimal.Text.Contains("."))
        //        {
        //            Entry_MontoVendidoDecimal.Text = Entry_MontoVendidoDecimal.Text.Replace(".", "");
        //        }
        //    }
        //}
        //else if (!string.IsNullOrEmpty(Entry_MontoVendidoDecimal.Text))
        //{
        //    if (Entry_MontoVendidoDecimal.Text.Contains("."))
        //    {
        //        Entry_MontoVendidoDecimal.Text = Entry_MontoVendidoDecimal.Text.Replace(".", "");
        //    }
        //    else if (!string.IsNullOrEmpty(Entry_MontoVendidoEntero.Text))
        //    {
        //        Entry_MontoVendidoEntero.Text = Entry_MontoVendidoEntero.Text.Replace(".", "");
        //    }
        //}
    }
    #endregion


    // L�GICA
    #region L�GICA
    private async Task CargarDatosPicker_Comprador()
    {
        var compradores = await ObtenerCompradoresDBAsync();
        Picker_Comprador.ItemsSource = compradores;
    }
    private async Task PermitirPopAsyncNavegacion()
    {
        bool respuesta = await DisplayAlert("Alerta", "�Desea regresar? Perder� el progreso realizado", "Confimar", "Cancelar");
        if (respuesta)
        {
            await Navigation.PopAsync();
        }
    }
    private string ControlarCamposGuardarCompraCredito()
    {
        var itemComprador = (Comprador)Picker_Comprador.SelectedItem;
        string montoE = Entry_MontoVendidoEntero.Text;
        string montoD = Entry_MontoVendidoDecimal.Text;
        if (string.IsNullOrEmpty(montoE) && string.IsNullOrEmpty(montoD))
        {
            _montoVendido = 0;
        }
        else if (string.IsNullOrEmpty(montoE) && !string.IsNullOrEmpty(montoD))
        {
            _montoVendido = double.Parse("0." + montoD);
        }
        else if (!string.IsNullOrEmpty(montoE) && string.IsNullOrEmpty(montoD))
        {
            _montoVendido = double.Parse(montoE);
        }
        else
        {
            _montoVendido = double.Parse(montoE + "." + montoD);
        }
        if (_montoVendido == 0 || itemComprador is null)
        {
            return "Existen campos incompletos o err�neos";
        }
        return "true";
    }
    #endregion


    // BASE DE DATOS
    #region BASE DE DATOS
    private async Task<List<Tbl_Comprador>> ObtenerCompradoresDBAsync()
    {
        return await _repoComprador.ObtenerCompradoresAsync();
    }
    private async Task GuardarNuevoCompradorDBAsync(Comprador comprador)
    {
        await Comprador_Repository.GuardarNuevoCompradorAsync(comprador);
    }
    private async Task GuardarVentaCreditoDBAsync(VentaCredito ventaCredito)
    {
        await VentaCredito_Repository.GuardarVentaCreditoAsync(ventaCredito);
    }
    #endregion


    // L�GICA DE COSAS VISUALES DE LA P�GINA
    #region L�GICA DE COSAS VISUALES DE LA P�GINA
    private void LimpiarPuntosEntry(Entry entry)
    {
        if (entry.Text.Contains("."))
        {
            entry.Text = Regex.Replace(entry.Text, @"\.", string.Empty);
        }
    }
    private async Task OcultarTeclado()
    {
        Entry_MontoVendidoEntero.Unfocus();
        Entry_MontoVendidoDecimal.Unfocus();
        Entry_Descripcion.Unfocus();
        await Entry_MontoVendidoEntero.HideKeyboardAsync(CancellationToken.None);
        await Entry_MontoVendidoDecimal.HideKeyboardAsync(CancellationToken.None);
        await Entry_Descripcion.HideKeyboardAsync(CancellationToken.None);
    }
    private void LimpiarCampos()
    {
        Entry_MontoVendidoEntero.Text = "";
        Entry_MontoVendidoDecimal.Text = "";
        Entry_Descripcion.Text = "";
        Picker_Comprador.SelectedIndex = -1;
    }
    #endregion
}