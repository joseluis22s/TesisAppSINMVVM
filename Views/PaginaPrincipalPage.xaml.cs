using TesisAppSINMVVM.LocalDatabase;
using TesisAppSINMVVM.Views.CompraViews;

namespace TesisAppSINMVVM.Views;

public partial class PaginaPrincipalPage : ContentPage
{
    int _contador = 0;
    public PaginaPrincipalPage()
    {
        InitializeComponent();
    }


    // NAVEGACIÓN
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
    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
    protected override bool OnBackButtonPressed()
    {
        return true;
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

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        if (_contador == 0)
        {
            SincronizarBD sincronizarBD = new SincronizarBD();
            sincronizarBD.SincornizarDBInicio();
            _contador = 1;
        }
    }

    

    // LÓGICA DE EVENTOS
    // BASE DE DATOS
    // LÓGICA PARA COSAS VISUALES
}