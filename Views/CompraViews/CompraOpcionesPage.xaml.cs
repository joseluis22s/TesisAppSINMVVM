using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.Views.CompraViews;

public partial class CompraOpcionesPage : ContentPage
{
	public CompraOpcionesPage()
	{
		InitializeComponent();
	}


    // NAVEGACI�N
    #region NAVEGACI�N
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