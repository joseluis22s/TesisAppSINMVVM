using TesisAppSINMVVM.Views.CompraViews;

namespace TesisAppSINMVVM.Views;

public partial class PaginaPrincipalPage : ContentPage
{
    public PaginaPrincipalPage()
    {
        InitializeComponent();
    }


    // NAVEGACI�N
    private async Task RegistrarNuevaCompraPagePushAsync()
    {
        //await Navigation.PushAsync(new CompraPage());
        await Navigation.PushAsync(new ProveedoresPage());
    }
    private async Task ChequesPagePushAsync()
    {
        await Navigation.PushAsync(new ChequesPage());
    }
    private async Task VentaPagePushAsync()
    {
        await Navigation.PushAsync(new VentaPage());
    }


    // EVENTOS
    private async void Button_Compra_Clicked(object sender, EventArgs e)
    {
        await RegistrarNuevaCompraPagePushAsync();
    }
    private async void Button_Cheques_Clicked(object sender, EventArgs e)
    {
        await ChequesPagePushAsync();
    }
    private async void Button_Venta_Clicked(object sender, EventArgs e)
    {
        await VentaPagePushAsync();
    }

    // L�GICA DE EVENTOS
    // BASE DE DATOS
    // L�GICA PARA COSAS VISUALES
}