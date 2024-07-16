using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Collections.ObjectModel;
using TesisAppSINMVVM.Models;
using TesisAppSINMVVM.Views.CompraViews;

namespace TesisAppSINMVVM.Views.CompraViews;

public partial class CompraOpcionesPage : ContentPage
{
    private bool _enEjecucion;
    public CompraOpcionesPage()
	{
		InitializeComponent();
	}


    // NAVEGACI�N
    #region NAVEGACI�N
    private async Task RegistarNuevaCompraPushAsync()
    {
        NetworkAccess accessType = Connectivity.Current.NetworkAccess;
        if (accessType == NetworkAccess.Internet)
        {
            await Navigation.PushAsync(new RegistrarNuevaCompraPage
            {
                BindingContext = this.BindingContext
            });
        }
        else
        {
            await Toast.Make("Primero debe conectarse a internet", ToastDuration.Long).Show();
        }
    }
    private async Task HistorialComprasPushAsync()
    {
        await Navigation.PushAsync(new HistorialComprasPage
        {
            BindingContext = this.BindingContext
        });
    }
    private async Task NavegarPaginaPrincipalPagePopToRootAsync()
    {
        await Navigation.PopToRootAsync();
        await Navigation.PushAsync(new PaginaPrincipalPage());
    }
    #endregion

    // EVENTOS
    #region EVENTOS
    private async void Buttton_RegistarNuevaCompra_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await RegistarNuevaCompraPushAsync();
        _enEjecucion = false;
    }
    private async void Buttton_HistorialCompras_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await HistorialComprasPushAsync();
        _enEjecucion = false;
    }
    private async void ImageButton_Home_Clicked(object sender, EventArgs e)
    {
        await NavegarPaginaPrincipalPagePopToRootAsync();
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