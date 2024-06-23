using TesisAppSINMVVM.Views.VentaViews;

namespace TesisAppSINMVVM.Views;

public partial class VentaPage : ContentPage
{
	public VentaPage()
	{
		InitializeComponent();
	}


    // NAVEGACIÓN
    private async Task RegistrarProductoSobranteBodegaPagePushAsync()
    {
        await Navigation.PushAsync(new RegistrarProductoSobranteBodegaPage());
    }
    private async Task RegistrarVentaCreditoPagePushAsync()
    {
        await Navigation.PushAsync(new RegistrarNuevaVentaCreditoPage());
    }


    // EVENTOS
    private async void Buttton_Bodega_Clicked(object sender, EventArgs e)
    {
        await RegistrarProductoSobranteBodegaPagePushAsync();
    }
    private async void Buttton_CuentasPorCobrar_Clicked(object sender, EventArgs e)
    {
        await RegistrarVentaCreditoPagePushAsync();
    }


    // LOGICA PARA EVENTOS
    // LÓGICA
    // BASE DE DATOS
    // LÓGICA DE COSAS VISUALES DE LA PÁGINA


}