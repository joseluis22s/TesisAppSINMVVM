using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace TesisAppSINMVVM.Views.VentaViews.BodegaViews;

public partial class OpcionesBodegaPage : ContentPage
{
    public OpcionesBodegaPage()
    {
        InitializeComponent();
    }

    // NAVEGACI�N
    #region NAVEGACI�N
    private async Task RegistrarProductoSobranteBodegaPagePushAsync()
    {
        NetworkAccess accessType = Connectivity.Current.NetworkAccess;
        if (accessType == NetworkAccess.Internet)
        {
            await Navigation.PushAsync(new RegistrarProductoSobranteBodegaPage());
        }
        else
        {
            await Toast.Make("Primero debe conectarse a internet", ToastDuration.Long).Show();
        }
    }
    private async Task HistorialProductoSobrantePagePushAsync()
    {
        await Navigation.PushAsync(new HistorialProductoSobrantePage());
    }
    #endregion


    // EVENTOS
    #region EVENTOS

    private async void Buttton_RegistrarProductoSobrante_Clicked(object sender, EventArgs e)
    {
        await RegistrarProductoSobranteBodegaPagePushAsync();
    }

    private async void Buttton_HistorialProductoSobrante_Clicked(object sender, EventArgs e)
    {
        await HistorialProductoSobrantePagePushAsync();
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