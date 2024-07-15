using TesisAppSINMVVM.Views.VentaViews;
using TesisAppSINMVVM.Views.VentaViews.BodegaViews;
using TesisAppSINMVVM.Views.VentaViews.VentaCreditoViews;

namespace TesisAppSINMVVM.Views;

public partial class VentaPage : ContentPage
{
	public VentaPage()
	{
		InitializeComponent();
	}


    // NAVEGACI�N
    private async Task OpcionesBodegaPagePushAsync()
    {
        await Navigation.PushAsync(new OpcionesBodegaPage());
    }
    private async Task OpcionesVentaCreditoPagePushAsync()
    {
        await Navigation.PushAsync(new OpcionesVentaCreditoPage());
    }

    // EVENTOS
    private async void Buttton_Bodega_Clicked(object sender, EventArgs e)
    {
        await OpcionesBodegaPagePushAsync();
    }
    private async void Buttton_CuentasPorCobrar_Clicked(object sender, EventArgs e)
    {
        await OpcionesVentaCreditoPagePushAsync();
    }
    // LOGICA PARA EVENTOS
    // L�GICA
    // BASE DE DATOS
    // L�GICA DE COSAS VISUALES DE LA P�GINA


}