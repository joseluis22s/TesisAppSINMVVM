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


    // NAVEGACIÓN
    #region NAVEGACIÓN
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
    private void NavegarPaginaPrincipalPageAsync()
    {
        var stack = Navigation.NavigationStack.ToArray();
        for (int i = 2; i < stack.Length ; i++)
        {
            Navigation.RemovePage(stack[i]);
        }
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
    private void ImageButton_Home_Clicked(object sender, EventArgs e)
    {
        NavegarPaginaPrincipalPageAsync();
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