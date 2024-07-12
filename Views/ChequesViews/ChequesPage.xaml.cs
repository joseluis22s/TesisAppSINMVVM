using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
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
        NetworkAccess accessType = Connectivity.Current.NetworkAccess;
        if (accessType == NetworkAccess.Internet)
        {
            await Navigation.PushAsync(new RegistrarChequePage());
        }
        else
        {
            await Toast.Make("Primero debe conectarse a internet", ToastDuration.Long).Show();
        }
    }
    private async Task HistorialChequesPagePushAsync()
    {
        await Navigation.PushAsync(new HistorialChequesPage());
    }


    // EVENTOS
    private async void Buttton_RegistarCheque_Clicked(object sender, EventArgs e)
    {
        await RegistarChequePagePushAsync();
    }

    private async void Buttton_HistorialCheques_Clicked(object sender, EventArgs e)
    {
        await HistorialChequesPagePushAsync();
    }


    // LOGICA PARA EVENTOS
    // BASE DE DATOS
    // LÓGICA DE COSAS VISUALES DE LA PÁGINA


}