using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace TesisAppSINMVVM.Views.VentaViews.BodegaViews;

public partial class OpcionesBodegaPage : ContentPage
{
    public OpcionesBodegaPage()
    {
        InitializeComponent();
    }

    // NAVEGACIÓN
    #region NAVEGACIÓN
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


    // LÓGICA PARA EVENTOS
    #region LÓGICA PARA EVENTOS

    #endregion


    // LÓGICA
    #region LÓGICA

    #endregion


    // BASE DE DATOS
    #region BASE DE DATOS

    #endregion


    // LÓGICA DE COSAS VISUALES DE LA PÁGINA
    #region LÓGICA DE COSAS VISUALES DE LA PÁGINA

    #endregion
}