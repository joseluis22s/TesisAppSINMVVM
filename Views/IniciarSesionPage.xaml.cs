using CommunityToolkit.Maui.Alerts;

namespace TesisAppSINMVVM.Views;

public partial class IniciarSesionPage : ContentPage
{
	string user;
	public IniciarSesionPage()
	{
		InitializeComponent();
	}
	// NAVEGACIÓN
	private async Task CrearNuevaCuentaPagePushAsync()
	{
		await Navigation.PushAsync(new CrearNuevaCuentaPage());
	}
	private async Task PaginaPrincipalPagePushAsync()
	{
        await Navigation.PushAsync(new PaginaPrincipalPage());
    }

    // EVENTOS
    private async void Button_CrearNuevaCuenta_Clicked(object sender, EventArgs e)
    {
		await CrearNuevaCuentaPagePushAsync();
    }
    private async void Button_Ingresar_Clicked(object sender, EventArgs e)
    {
		string usuario = Entry_Usuario.Text;
		string contrasena = Entry_Contrasena.Text;
		bool esPermitido = await PermitirIniciarSesion(usuario, contrasena);
        if (esPermitido)
		{
            await PaginaPrincipalPagePushAsync();
        }
        else
		{
            await Toast.Make("Usuario o contrasena incorrectos").Show();
        }
    }

	// LOGICA PARA EVENTOS
	private Task<bool> PermitirIniciarSesion(string usuario, string contrasena)
	{
		if (usuario.Equals("Jose") && contrasena.Equals("123"))
		{
			return Task.FromResult(true);
		}
		return Task.FromResult(false);
	}

	
}