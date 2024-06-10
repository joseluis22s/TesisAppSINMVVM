using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core.Platform;
using Microsoft.Maui;
using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.Database.Tables;

namespace TesisAppSINMVVM.Views.VentaViews;

public partial class RegistrarVentaCreditoPage : ContentPage
{
    private Tbl_Comprador_Repository _repoComprador;
    private Tbl_VentaCredito_Repository _repoVentaCredito;
    private bool _enEjecucion;
    private bool _paginaCargada = false;
    private double _montoVendido;

    public static bool _ejecutarAppearing = true;

    public RegistrarVentaCreditoPage()
    {
        InitializeComponent();
        _repoComprador = new Tbl_Comprador_Repository();
        _repoVentaCredito = new Tbl_VentaCredito_Repository();
    }


    // NAVEGACIÓN
    private async Task AgregarCompradorPagePushModalAsync()
    {
        await Navigation.PushModalAsync(new AgregarCompradorPage());
        //_ejecutarAppearing = false;
        //////////zauqiiiiiiiiiiiiiiiiiii
    }
    private async Task HistorialVentasCreditoPushAsync()
    {
        await Navigation.PushAsync(new HistorialVentasCredito());
    }
    private async Task RegistrarVentaCreditoPagePopAsync()
    {
        //await Navigation.PopAsync();
        await PermitirPopAsyncNavegacion();
    }


    // EVENTOS
    private async void ContentPage_Appearing(object sender, EventArgs e)
    {
        if (!_paginaCargada)
        {
            base.OnAppearing();
            await CargarDatosPicker_Comprador();
            _paginaCargada = true;
        }
    }
    private async void Button_AgregarComprador_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await AgregarCompradorPagePushModalAsync();
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
        MontoVendido_TextChanged();
    }

    // LOGICA PARA EVENTOS
    private async Task CargarDatosPicker_Comprador()
    {
        var compradores = await ObtenerCompradoresDBAsync();
        Picker_Comprador.ItemsSource = compradores;
    }
    private async Task GuardarCompraCreditoAsync()
    {
        string resultado = ControlarCamposGuardarCompraCredito();
        if (resultado == "true")
        {
            var itemComprador = (Tbl_Comprador)Picker_Comprador.SelectedItem;
            Tbl_VentaCredito ventaCredito = new Tbl_VentaCredito()
            {
                COMPRADOR = itemComprador.NOMBRE + " " + itemComprador.APELLIDO,
                MONTOVENDIDO = _montoVendido,
                DESCRIPCION = Entry_Descripcion.Text,
                FECHAGUARDADO = DateTime.Now.ToString("dd/MM/yyyy"),
                DIAFECHAGUARDADO = DateTime.Now.ToString("dddd, dd MMMM")
            };
            await OcultarTeclado();
            await GuardarVentaCreditoDBAsync(ventaCredito);
            await Toast.Make("¡Registro guardado!").Show();
            LimpiarCampos();
        }
        else
        {
            await OcultarTeclado();
            await Toast.Make(resultado).Show();
        }
    }
    private async Task PermitirPopAsyncNavegacion()
    {
        bool respuesta = await DisplayAlert("Alerta", "¿Desea regresar? Perderá el progreso realizado", "Confimar", "Cancelar");
        if (respuesta)
        {
            await Navigation.PopAsync();
        }
    }

    private void MontoVendido_TextChanged()
    {
        if (!string.IsNullOrEmpty(Entry_MontoVendidoEntero.Text))
        {
            if (Entry_MontoVendidoEntero.Text.Contains("."))
            {
                Entry_MontoVendidoEntero.Text = Entry_MontoVendidoEntero.Text.Replace(".", "");
            }
            else if (!string.IsNullOrEmpty(Entry_MontoVendidoDecimal.Text))
            {
                if (Entry_MontoVendidoDecimal.Text.Contains("."))
                {
                    Entry_MontoVendidoDecimal.Text = Entry_MontoVendidoDecimal.Text.Replace(".", "");
                }
            }
        }
        else if (!string.IsNullOrEmpty(Entry_MontoVendidoDecimal.Text))
        {
            if (Entry_MontoVendidoDecimal.Text.Contains("."))
            {
                Entry_MontoVendidoDecimal.Text = Entry_MontoVendidoDecimal.Text.Replace(".", "");
            }
            else if (!string.IsNullOrEmpty(Entry_MontoVendidoEntero.Text))
            {
                Entry_MontoVendidoEntero.Text = Entry_MontoVendidoEntero.Text.Replace(".", "");
            }
        }
    }


    // LÓGICA
    private string ControlarCamposGuardarCompraCredito()
    {
        var itemComprador = (Tbl_Comprador)Picker_Comprador.SelectedItem;
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
            return "Existen campos incompletos o erróneos";
        }
        return "true";
    }



    // BASE DE DATOS
    private async Task<List<Tbl_Comprador>> ObtenerCompradoresDBAsync()
    {
        return await _repoComprador.ObtenerCompradoresAsync();
    }
    private async Task GuardarVentaCreditoDBAsync(Tbl_VentaCredito ventaC)
    {
        await _repoVentaCredito.GuardarVentaCreditoAsync(ventaC);
    }


    // LÓGICA DE COSAS VISUALES DE LA PÁGINA
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
}