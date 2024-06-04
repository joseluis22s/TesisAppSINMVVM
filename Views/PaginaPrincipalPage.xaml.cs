namespace TesisAppSINMVVM.Views;

public partial class PaginaPrincipalPage : ContentPage
{
    public PaginaPrincipalPage()
    {
        InitializeComponent();
    }


    // NAVEGACIÓN
    private async Task CompraPagePushAsync()
    {
        await Navigation.PushAsync(new CompraPage());
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
        await CompraPagePushAsync();
    }
    private async void Button_Cheques_Clicked(object sender, EventArgs e)
    {
        await ChequesPagePushAsync();
    }
    private async void Button_Venta_Clicked(object sender, EventArgs e)
    {
        await VentaPagePushAsync();
    }

    // LÓGICA DE EVENTOS
    // BASE DE DATOS
    // LÓGICA PARA COSAS VISUALES
}