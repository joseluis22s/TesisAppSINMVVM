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

    // NAVEGACI�N ENTRE P�GINAS
    private async Task CrearNuevaCuentaPagePopAsync()
    {
        await Navigation.PopAsync();
    }

    // EVENTOS
    private async void Button_SiguienteCrear_Clicked(object sender, EventArgs e)
    {
        //if (enEjecucion)
        //{
        //    return;
        //}
        //enEjecucion = true;
        if (Button_SiguienteCrear.Text == "Siguiente")
        {
            bool esUsuarioValido = await VerificarNuevoUsuarioAsync();
            bool esCorreValido = await VerificarNuevoCorreoAsync();
            if (!esUsuarioValido || !esCorreValido)
            {
                await Toast.Make("Todos los campos deben ser v�lidos").Show();
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
            bool tieneParecidoUsuarioContrasena = await VerificarParecidoUsuarioContrasenaAsync();
            if (esContrasenaValida && sonContrasenaIguales && !tieneParecidoUsuarioContrasena)
            {
                await Toast.Make("Cuenta creada").Show();
                await Navigation.PopAsync();
            }
            else
            {
                await Toast.Make("Todos los campos deben ser v�lidos").Show();
            }
        }
        //enEjecucion = false;
    }
    private async void Image_CancelClose_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        bool respuesta = await DisplayAlert("Alerta", "�Desea regresar? Perder� el progreso realizado", "Confirmar", "Cancelar");
        if (respuesta)
        {
            await CrearNuevaCuentaPagePopAsync();
        }
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
    private async void Entry_NuevaContrasena_TextChanged(object sender, TextChangedEventArgs e)
    {
        bool esContrasenavalida = await VerificarNuevaContrasenaAsync();
        bool esParecidoUsuarioContrasena = await VerificarParecidoUsuarioContrasenaAsync();
        string contrasenaIgual = Entry_ContrasenaIgual.Text;
        if (!string.IsNullOrEmpty(contrasenaIgual))
        {
            bool esIgualContrasena = await VerificarNuevaContrasenaIgualAsync();
            if (esIgualContrasena)
            {
                Label_ValidacionContrasenaIgual.IsVisible = false;
                Image_ContrasenaIgualCheckIcon.IsVisible = true;
                Image_ContrasenaIgualUncheckIcon.IsVisible = false;
            }
            else
            {
                Label_ValidacionContrasenaIgual.IsVisible = true;
                Image_ContrasenaIgualCheckIcon.IsVisible = false;
                Image_ContrasenaIgualUncheckIcon.IsVisible = true;
            }
        }
        if (esContrasenavalida)
        {
            Label_ValidacionNuevaContrasena1.IsVisible = false;
            if (esParecidoUsuarioContrasena)
            {
                Label_ValidacionNuevaContrasena2.IsVisible = true;
                Image_NuevaContrasenaCheckIcon.IsVisible = false;
                Image_NuevaContrasenaUncheckIcon.IsVisible = true;
            }
            else
            {
                Label_ValidacionNuevaContrasena2.IsVisible = false;
                Image_NuevaContrasenaCheckIcon.IsVisible = true;
                Image_NuevaContrasenaUncheckIcon.IsVisible = false;
            }
        }
        else
        {
            Label_ValidacionNuevaContrasena1.IsVisible = true;
            Image_NuevaContrasenaCheckIcon.IsVisible = false;
            Image_NuevaContrasenaUncheckIcon.IsVisible = true;
            if (esParecidoUsuarioContrasena)
            {
                Label_ValidacionNuevaContrasena2.IsVisible = true;
            }
            else
            {
                Label_ValidacionNuevaContrasena2.IsVisible = false;
            }
        }


    }
    private async void Entry_ContrasenaIgual_TextChanged(object sender, TextChangedEventArgs e)
    {
        bool esIgual = await VerificarNuevaContrasenaIgualAsync();
        if (esIgual)
        {
            Label_ValidacionContrasenaIgual.IsVisible = false;
            Image_ContrasenaIgualCheckIcon.IsVisible = true;
            Image_ContrasenaIgualUncheckIcon.IsVisible = false;
        }
        else
        {
            Label_ValidacionContrasenaIgual.IsVisible = true;
            Image_ContrasenaIgualCheckIcon.IsVisible = false;
            Image_ContrasenaIgualUncheckIcon.IsVisible = true;
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
        if (string.IsNullOrEmpty(nuevoUsuario))
        {
            nuevoUsuario = "";
        }

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
                    nuevoUsuario.Length > 7 && Regex.IsMatch(nuevoUsuario, @"^(?!.*[\s�-�])[A-Za-z\d]*$"))
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
        if (string.IsNullOrEmpty(correo))
        {
            correo = "";
        }
        return Task.FromResult(Regex.IsMatch(correo, @"^(?!.*[\s�-�])[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$"));
    }
    private Task<bool> VerificarNuevaContrasenaAsync()
    {
        string nuevaContrasena = Entry_NuevaContrasena.Text;
        if (string.IsNullOrEmpty(nuevaContrasena))
        {
            nuevaContrasena = "";
        }
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
                Regex.IsMatch(nuevaContrasena, @"^(?!.*[\s�-�]).*[@$#.].*$") && tieneDigito &&
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
        if (string.IsNullOrEmpty(contrasena1))
        {
            contrasena1 = "";
        }
        if (string.IsNullOrEmpty(contrasena2))
        {
            contrasena2 = "";
        }

        return Task.FromResult(contrasena1 == contrasena2);
    }
    private Task<bool> VerificarParecidoUsuarioContrasenaAsync()
    {
        string usuario = Entry_NuevoUsuario.Text;
        string contrasena = Entry_NuevaContrasena.Text;
        if (string.IsNullOrEmpty(contrasena))
        {
            contrasena = "";
        }
        return Task.FromResult((usuario.Equals(contrasena) || contrasena.Contains(usuario)));
    }
    




    // BASE DE DATOS
}