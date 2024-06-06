using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.Database.Tables;

namespace TesisAppSINMVVM.Views.VentaViews;

public partial class RegistrarVentaCreditoPage : ContentPage
{
    private Tbl_Comprador_Repository _repoComprador;
    private Tbl_VentaCredito_Repository _repoVentaCredito;
    private bool _ejecutarAppearing = true;
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
        _ejecutarAppearing = false;
    }
    private async Task HistorialVentasCredito()
    {
        await Navigation.PushAsync(new HistorialVentasCredito());
    }


    // EVENTOS
    private async void ContentPage_Appearing(object sender, EventArgs e)
    {
        if (_ejecutarAppearing)
        {
            base.OnAppearing();
            await CargarDatosPicker_Comprador();
        }
        _ejecutarAppearing = true;
    }
    private async void Button_AgregarComprador_Clicked(object sender, EventArgs e)
    {
        await AgregarCompradorPagePushModalAsync();
    }
    private async void Button_HistorialVentasCredito_Clicked(object sender, EventArgs e)
    {
        await HistorialVentasCredito();
    }
    private async void Button_GuardarVentaCredito_Clicked(object sender, EventArgs e)
    {
        await GuardarCompraCreditoAsync();
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
                MONTOVENDIDO = double.Parse(Entry_MontoVendido.Text),
                DESCRIPCION = Entry_Descripcion.Text,
                FECHAGUARDADO = DateTime.Now.ToString("dd/MM/yyyy"),
                DIAFECHAGUARDADO = DateTime.Now.ToString("dddd, dd MMMM")
            };
            await GuardarVentaCreditoDBAsync(ventaCredito);
        }
    }

    // LÓGICA
    private string ControlarCamposGuardarCompraCredito()
    {
        var itemComprador = (Tbl_Comprador)Picker_Comprador.SelectedItem;
        string monto = Entry_MontoVendido.Text;

        if (string.IsNullOrEmpty(monto) || itemComprador is null)
        {
            return "¡Llene todo los campos!";
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
}