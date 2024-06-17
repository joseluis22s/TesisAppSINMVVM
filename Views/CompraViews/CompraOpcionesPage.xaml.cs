using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.Views.CompraViews;

public partial class CompraOpcionesPage : ContentPage
{
	public CompraOpcionesPage()
	{
		InitializeComponent();
	}


    // NAVEGACIÓN
    #region NAVEGACIÓN
    private async Task RegistarNuevaCompraPushAsync()
    {
        await Navigation.PushAsync(new RegistrarNuevaCompraPage
        {
            BindingContext = this.BindingContext
        });
    }
    private async Task HistorialComprasPushAsync()
    {
        await Navigation.PushAsync(new HistorialComprasPage
        {
            BindingContext = this.BindingContext
        });
    }

    #endregion

    // EVENTOS
    #region EVENTOS
    private async void Buttton_RegistarNuevaCompra_Clicked(object sender, EventArgs e)
    {
        await RegistarNuevaCompraPushAsync();
    }
    private async void Buttton_HistorialCompras_Clicked(object sender, EventArgs e)
    {
        await HistorialComprasPushAsync();
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