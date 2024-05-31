using CommunityToolkit.Maui.Alerts;
using TesisAppSINMVVM.Database.Respositories;

namespace TesisAppSINMVVM.Views;

public partial class AgregarNuevoProveedorPage : ContentPage
{

    private Tbl_Proveedor_Respository _repoProveedor;
    private bool _enEjecucion;

    public AgregarNuevoProveedorPage()
	{
		InitializeComponent();
        _repoProveedor =  new Tbl_Proveedor_Respository();
	}

    // NAVEGACIÓN
    private async Task AgregarNuevaCuentaPagePopModalAsync(bool mostrarAlerta)
    {
        await PermitirPopModalAsyncNavegacion(mostrarAlerta);
    }


    // EVENTOS
    protected override bool OnBackButtonPressed()
    {
        Dispatcher.Dispatch(async () =>
        {
            await AgregarNuevaCuentaPagePopModalAsync(true);
        });
        return true;
    }
    private async void Button_Guardar_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await PermitirGuardarProveedor();
        _enEjecucion = false;
    }
    private async void Button_Cancelar_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await AgregarNuevaCuentaPagePopModalAsync(true);
        _enEjecucion = false;
    }
    
    
    // LOGICA PARA EVENTOS
    private async Task PermitirPopModalAsyncNavegacion(bool mostrarAlerta)
    {
        OcultarTeclado();
        if (mostrarAlerta)
        {
            CompraPage._permitirEjecucion = false;
            bool respuesta = await DisplayAlert("Alerta", "¿Desea regresar? Perderá el progreso realizado", "Confimar", "Cancelar");
            if (respuesta)
            {
                await Navigation.PopModalAsync();
            }
        }
        else
        {
            await Navigation.PopModalAsync();
        }
    }
    private string ValidarCampos()
    {
        //TODO: Eliminar los espacios en blanco al inicio y al final asi como se hizo con el double y el int
        string nombre = Entry_NombreNuevoProveedor.Text;
        string apellido = Entry_ApellidoNuevoProveedor.Text;
        string telefono = Entry_TelefonoNuevoProveedor.Text;
        if (String.IsNullOrWhiteSpace(nombre) || 
            String.IsNullOrWhiteSpace(apellido) ||
            String.IsNullOrWhiteSpace(telefono))
        {
            return "Deber llenar todos los campos";
        }
        foreach (char letra in nombre)
        {
            if (!Char.IsLetter(letra))
            {
                return "Nombre no válido";
            }
        }
        foreach (char letra in apellido)
        {
            if (!Char.IsLetter(letra))
            {
                return "Apellido no válido";
            }
        }
        foreach (char digito in telefono)
        {
            if (!Char.IsDigit(digito))
            {
                return "Teléfono no válido";
            }
        }
        if (telefono.Length != 10)
        {
            return "Teléfono no válido";
        }
        return "true";
    }
    private async Task PermitirGuardarProveedor()
    {
        string resultado = ValidarCampos();
        OcultarTeclado();
        if (resultado == "true")
        {
            bool existeProveedor = await VerificarExistenciaProveedorDBAsync();
            if (existeProveedor)
            {
                await Toast.Make("Contacto ya existente").Show();
            }
            else
            {
                await GuardarNuevoProveedorDBAsync();
                await Toast.Make("¡Contacto guardado!").Show();
                await AgregarNuevaCuentaPagePopModalAsync(false);
            }
        }
        else
        {
            await Toast.Make(resultado).Show();
        }
    }


    // BASE DE DATOS
    private async Task<bool> VerificarExistenciaProveedorDBAsync()
    {
        return await _repoProveedor.VerificarExistenciaProveedorAsync(Entry_NombreNuevoProveedor.Text, Entry_ApellidoNuevoProveedor.Text);
    }
    private async Task GuardarNuevoProveedorDBAsync()
    {
        string nombre = Entry_NombreNuevoProveedor.Text;
        string apellido = Entry_ApellidoNuevoProveedor.Text;
        string telefono = Entry_TelefonoNuevoProveedor.Text;
        await _repoProveedor.GuardarNuevoProveedorAsync(nombre,apellido,telefono);
    }


    // LÓGICA DE COSAS VISUALES DE LA PÁGINA
    private void OcultarTeclado()
    {
        Entry_NombreNuevoProveedor.Unfocus();
        Entry_ApellidoNuevoProveedor.Unfocus();
        Entry_TelefonoNuevoProveedor.Unfocus();
    }
}