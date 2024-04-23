using CommunityToolkit.Maui.Alerts;
using System.Text.RegularExpressions;

namespace TesisAppSINMVVM.Views;

public partial class CrearNuevaCuentaPage : ContentPage
{
    private bool enEjecucion = false;

    public CrearNuevaCuentaPage()
	{
		InitializeComponent();
	}

    // NAVEGACIÓN ENTRE PÁGINAS
    private async Task CrearNuevaCuentaPagePopAsync()
    {
        await Navigation.PopAsync();
    }

    // EVENTOS
    private async void Button_SiguienteCrear_Clicked(object sender, EventArgs e)
    {
        if (enEjecucion)
        {
            return;
        }
        enEjecucion = true;
        if (Button_SiguienteCrear.Text == "Siguiente")
        {
            bool esUsuarioValido = await VerificarNuevoUsuarioAsync();
            bool esCorreValido = await VerificarNuevoCorreoAsync();
            if (!esUsuarioValido || !esCorreValido)
            {
                await Toast.Make("Todos los campos deben ser válidos").Show();
            }
            else
            {
                VerticalStackLayout_UsuarioCorreo.IsVisible = false;
                VerticalStackLayout_Contrasenas.IsVisible = true;
                Button_SiguienteCrear.Text = "Crear";
                Image_CancelCloseIcon.IsVisible = false;
                Image_BackIcon.IsVisible = true;
            }
        }
        else
        {
            bool esContrasenaValida = await VerificarNuevaContrasenaAsync();
            bool sonContrasenaIguales = await VerificarNuevaContrasenaIgualAsync();
            bool tieneParecidoUsuarioContrasena = await VerficarParecidoUsuarioContrasenaAsync();
            if (esContrasenaValida || sonContrasenaIguales || !tieneParecidoUsuarioContrasena)
            {
                await Toast.Make("Cuenta creada").Show();
                await Navigation.PopAsync();
            }
        }
        enEjecucion = false;
    }
    private async void Image_CancelClose_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await CrearNuevaCuentaPagePopAsync();
    }
    private void Image_BackIcon_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        Image_BackIcon.IsVisible = false;
        Image_CancelCloseIcon.IsVisible = true;
        VerticalStackLayout_Contrasenas.IsVisible = false;
        VerticalStackLayout_UsuarioCorreo.IsVisible = true;
        Button_SiguienteCrear.Text = "Siguiente";
    }

    private async void Entry_NuevoUsuario_TextChanged(object sender, TextChangedEventArgs e)
    {
        bool esUsuarioValido = await VerificarNuevoUsuarioAsync();
        if (esUsuarioValido)
        {
            Label_ValidacionUsuario1.IsVisible = false;
            Image_NuevoUsuarioCheckIcon.IsVisible = true;
            Image_NuevoUsuarioUncheckIcon.IsVisible = false;
        }
        else
        {
            Label_ValidacionUsuario1.IsVisible = true;
            Image_NuevoUsuarioCheckIcon.IsVisible = false;
            Image_NuevoUsuarioUncheckIcon.IsVisible = true;
        }
    }
    private async void Entry_NuevoCorreo_TextChanged(object sender, TextChangedEventArgs e)
    {
        bool esCorreoValido = await VerificarNuevoCorreoAsync();
        if (esCorreoValido)
        {
            Label_VerificacionCorreo.IsVisible = false;
            Image_CorreoCheckIcon.IsVisible = true;
            Image_CorreoUncheckIcon.IsVisible= false;
        }
        else
        {
            Label_VerificacionCorreo.IsVisible = true;
            Image_CorreoCheckIcon.IsVisible = false;
            Image_CorreoUncheckIcon.IsVisible = true;
        }
    }

    private void Button_BorrarTabla_Clicked(object sender, EventArgs e)
    {

    }
    private void Button_VerificarExistenciaUsuario_Clicked(object sender, EventArgs e)
    {

    }
    // LOGICA PARA EVENTOS
    private Task<bool> VerificarNuevoUsuarioAsync()
    {
        string nuevoUsuario = Entry_NuevoUsuario.Text;
        if (nuevoUsuario.Length != 0) 
            {
                bool tieneDigito = false;
                for (int i = 0; i < nuevoUsuario.Length; i++)
                {
                    if (Char.IsDigit(nuevoUsuario[i]))
                    {
                        tieneDigito = true;
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(nuevoUsuario) && Char.IsUpper(nuevoUsuario[0]) && tieneDigito &&
                    nuevoUsuario.Length > 7 && Regex.IsMatch(nuevoUsuario, @"^(?!.*[\sÀ-ÿ])[A-Za-z\d]*$"))
                {
                    return Task.FromResult(true);
                }
            return Task.FromResult(false);
        }
        return Task.FromResult(false);
    }
    private Task<bool> VerificarNuevoCorreoAsync()
    {
        string correo = Entry_NuevoCorreo.Text;
        return Task.FromResult(Regex.IsMatch(correo, @"^(?!.*[\sÀ-ÿ])[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$"));

        //bool isValid = Regex.IsMatch(correo, @"^(?!.*[\sÀ-ÿ])[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$");
        //if (isValid)
        //{
        //    return true;
        //}
        //return false;
    }
    private Task<bool> VerificarNuevaContrasenaAsync()
    {
        string nuevaContrasena = Entry_NuevaContrasena.Text;
        if (nuevaContrasena.Length != 0)
        {
            bool tieneDigito = false;

            for (int i = 0; i < nuevaContrasena.Length; i++)
            {
                if (Char.IsDigit(nuevaContrasena[i]))
                {
                    tieneDigito = true;
                    break;
                }
            }
            if (!string.IsNullOrEmpty(nuevaContrasena) && Char.IsUpper(nuevaContrasena[0]) &&
                Regex.IsMatch(nuevaContrasena, @"^(?!.*[\sÀ-ÿ]).*[@$#.].*$") && tieneDigito &&
                nuevaContrasena.Length > 7)
            {
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
        return Task.FromResult(false);
    }
    private Task<bool> VerificarNuevaContrasenaIgualAsync()
    {
        string contrasena1 = Entry_NuevaContrasena.Text;
        string contrasena2 = Entry_ContrasenaIgual.Text;

        return Task.FromResult(contrasena1 == contrasena2);
    }
    private Task<bool> VerficarParecidoUsuarioContrasenaAsync()
    {
        string usuario = Entry_NuevoUsuario.Text;
        string contrasena = Entry_NuevaContrasena.Text;
        return Task.FromResult((usuario.Equals(contrasena) || contrasena.Contains(usuario)));
    }




    // BASE DE DATOS
}