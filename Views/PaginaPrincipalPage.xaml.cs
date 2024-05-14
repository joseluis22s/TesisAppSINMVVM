namespace TesisAppSINMVVM.Views;

public partial class PaginaPrincipalPage : ContentPage
{
    public PaginaPrincipalPage()
    {
        InitializeComponent();
    }


    // NAVEGACI�N
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
    private async void Border_Compra_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await CompraPagePushAsync();
    }
    private async void Border_Cheques_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await ChequesPagePushAsync();
    }
    private async void Border_Venta_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await VentaPagePushAsync();
    }


    // L�GICA DE EVENTOS
    // BASE DE DATOS
    // L�GICA PARA COSAS VISUALES
}