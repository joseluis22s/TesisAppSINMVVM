using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace TesisAppSINMVVM.Views.VentaViews.VentaCreditoViews;

public partial class OpcionesVentaCreditoPage : ContentPage
{
    private bool _enEjecucion;
    //TODO: Ver si se puede eliminar _enEjecucion
    public OpcionesVentaCreditoPage()
	{
		InitializeComponent();
	}



    // NAVEGACI�N
    #region NAVEGACI�N
    private async Task RegistrarNuevaVentaCreditoPagePushAsync()
    {
        NetworkAccess accessType = Connectivity.Current.NetworkAccess;
        if (accessType == NetworkAccess.Internet)
        {
            await Navigation.PushAsync(new RegistrarNuevaVentaCreditoPage());
        }
        else
        {
            await Toast.Make("Primero debe conectarse a internet", ToastDuration.Long).Show();
        }
    }
    private async Task HistorialVentasCreditoPagePushAsync()
    {
        await Navigation.PushAsync(new HistorialVentasCreditoPage());
    }
    private void NavegarPaginaPrincipalPageAsync()
    {
        var stack = Navigation.NavigationStack.ToArray();
        for (int i = 2; i < stack.Length; i++)
        {
            Navigation.RemovePage(stack[i]);
        }
    }
    #endregion


    // EVENTOS
    #region EVENTOS
    private async void Buttton_RegistrarNuevaVentaCredito_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await RegistrarNuevaVentaCreditoPagePushAsync();
        _enEjecucion = false;
    }

    private async void Buttton_HistorialVentasCredito_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await HistorialVentasCreditoPagePushAsync();
        _enEjecucion = false;
    }
    private void ImageButton_Home_Clicked(object sender, EventArgs e)
    {
        NavegarPaginaPrincipalPageAsync();
    }
    #endregion


    // L�GICA PARA EVENTOS
    #region L�GICA PARA EVENTOS

    #endregion


    // L�GICA
    #region L�GICA

    #endregion


    // BASE DE DATOS
    #region BASE DE DATOS

    #endregion


    // L�GICA DE COSAS VISUALES DE LA P�GINA
    #region L�GICA DE COSAS VISUALES DE LA P�GINA

    #endregion
}