using CommunityToolkit.Maui.Alerts;
using TesisAppSINMVVM.Database;

namespace TesisAppSINMVVM.Views;

public partial class IniciarSesionPage : ContentPage
{

    Repositorio repositorioDB;

    public IniciarSesionPage()
    {
        InitializeComponent();
        repositorioDB = new Repositorio();
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
        if (string.IsNullOrEmpty(usuario))
        {
            usuario = "";
        }
        if (string.IsNullOrEmpty(contrasena))
        {
            contrasena = "";
        }
        await PermitirIniciarSesion(usuario, contrasena);
    }

    // LOGICA PARA EVENTOS
    private async Task PermitirIniciarSesion(string usuario, string contrasena)
    {
        //if (usuario.Equals("Jose") && contrasena.Equals("123"))
        //{
        //	return Task.FromResult(true);
        //}
        //return Task.FromResult(false);
        if (usuario == "" || contrasena == "")
        {
            Label_UsuarioNoExiste.IsVisible = false;
            Label_ContrasenaIncorrecta.IsVisible = false;
            await Toast.Make("Los campos no deben estar vacios").Show();
            return;
        }
        bool esUsuario = await VerificarExistenciaUsuarioDBAsync(usuario);
        bool esContrasena = await VerificarContrasenaCorrectaDBAsync(usuario, contrasena);
        if (esUsuario && esContrasena)
        {
            await PaginaPrincipalPagePushAsync();
        }
        else
        {
            await Toast.Make("Error al iniciar sesión").Show();
        }
        Mostar(esUsuario, esContrasena);
    }

    // BASE DE DATOS
    private async Task<bool> VerificarExistenciaUsuarioDBAsync(string usuario)
    {
        return await repositorioDB.VerificarExistenciaUsuarioAsync(usuario);
    }
    private async Task<bool> VerificarContrasenaCorrectaDBAsync(string usuario, string contrasena)
    {
        return await repositorioDB.VerificarContrasenaCorrectaAsync(usuario, contrasena);
    }

    // LÓGICA DE COSAS VISUALES DE LA PÁGINA
    private void Mostar(bool esUsuario, bool esContrasena)
    {
        if (esUsuario)
        {
            Label_UsuarioNoExiste.IsVisible = false;
            if (esContrasena)
            {
                Label_ContrasenaIncorrecta.IsVisible = false;
            }
            else
            {
                Label_ContrasenaIncorrecta.IsVisible = true;
            }
        }
        else
        {
            Label_UsuarioNoExiste.IsVisible = true;
        }
        
    }
}