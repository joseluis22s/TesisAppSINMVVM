namespace TesisAppSINMVVM.Views;

public partial class PaginaPrincipalPage : ContentPage
{

    private bool _enEjecucion;

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
    private async void Border_Compra_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await CompraPagePushAsync();
        _enEjecucion = false;
    }
    private async void Border_Cheques_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await ChequesPagePushAsync();
        _enEjecucion = false;
    }
    private async void Border_Venta_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await VentaPagePushAsync();
        _enEjecucion = false;
    }


    // LÓGICA DE EVENTOS
    // BASE DE DATOS
    // LÓGICA PARA COSAS VISUALES
}