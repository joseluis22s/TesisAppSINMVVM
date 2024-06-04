using TesisAppSINMVVM.Views.ChequesViews;

namespace TesisAppSINMVVM.Views;

public partial class ChequesPage : ContentPage
{
	public ChequesPage()
	{
		InitializeComponent();
	}



    // NAVEGACIÓN
    private async Task RegistarChequePagePushAsync()
    {
        await Navigation.PushAsync(new RegistarChequePage());
    }
    private async Task HistorialChequesPagePushAsync()
    {
        await Navigation.PushAsync(new HistorialChequesPage());
    }


    // EVENTOS
    private async void Border_RegistarCheque_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await RegistarChequePagePushAsync();
    }
    private async void Border_HistorialCheques_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await HistorialChequesPagePushAsync();
    }


    // LOGICA PARA EVENTOS
    // BASE DE DATOS
    // LÓGICA DE COSAS VISUALES DE LA PÁGINA


}