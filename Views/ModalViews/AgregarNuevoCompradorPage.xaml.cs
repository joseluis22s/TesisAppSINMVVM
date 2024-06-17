using CommunityToolkit.Maui.Alerts;
using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.Database.Tables;
using TesisAppSINMVVM.Views.VentaViews;

namespace TesisAppSINMVVM.Views;

public partial class AgregarNuevoCompradorPage : ContentPage
{
	private Tbl_Comprador_Repository _repoComprador;
    private bool _enEjecucion;
    public AgregarNuevoCompradorPage()
	{
		InitializeComponent();
        _repoComprador = new Tbl_Comprador_Repository();
    }



    // NAVEGACIÓN
    private async Task AgregarNuevoCompradorPagePushModalAsync(bool mostrarAlerta)
    {
        RegistrarVentaCreditoPage._ejecutarAppearing = false;
        await PermitirPopModalAsyncNavegacion(mostrarAlerta);
    }

    // EVENTOS

    private async void Button_GuardarComprador_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await GuardarComprador();
        _enEjecucion = false;
    }
    private async void Button_Cancelar_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await AgregarNuevoCompradorPagePushModalAsync(true);
        _enEjecucion = false;
    }
    protected override bool OnBackButtonPressed()
    {
        AgregarNuevoCompradorPagePushModalAsync(true).GetAwaiter();
        return true;
    }

    // LOGICA PARA EVENTOS
    private async Task GuardarComprador()
    {
        string resultado = ControlarCampos();
        if (resultado == "true")
        {
            string nombre = Entry_NombreComprador.Text;
            string apellido = Entry_ApellidoComprador.Text;
            string telefono = Entry_TelefonoComprador.Text;

            Tbl_Comprador comprador = new Tbl_Comprador()
            {
                NOMBRE = nombre,
                APELLIDO = apellido,
                TELEFONO = telefono
            };

            await GuardarCompradorDBAsync(comprador);
            OcultarTecldo();
            await Toast.Make("Registro guardado").Show();
            await AgregarNuevoCompradorPagePushModalAsync(false);
        }
        else
        {
            await Toast.Make(resultado).Show();
        }
        
    }


    // LÓGICA
    private async Task PermitirPopModalAsyncNavegacion(bool mostrarAlerta)
    {
        //OcultarTeclado();
        if (mostrarAlerta)
        {
            //CompraPage._permitirEjecucion = false;
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
    private string ControlarCampos()
    {
        string nombre = Entry_NombreComprador.Text;
        string apellido = Entry_ApellidoComprador.Text;
        string telefono = Entry_TelefonoComprador.Text;
        bool tieneDigito = true;
        if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido) || 
            string.IsNullOrEmpty(telefono) )
        {
            return "Debe llenar todos los campos";
        }
        foreach (char n in telefono)
        {
            if (!Char.IsDigit(n))
            {
                tieneDigito = false;
                break;
            }
        }
        if (telefono.Length != 10 || tieneDigito == false)
        {
            return "Teléfono no válido";
        }
        return "true";
    }

    // BASE DE DATOS
    private async Task GuardarCompradorDBAsync(Tbl_Comprador comprador)
    {
        await _repoComprador.GuardarCompradorAsync(comprador);
    }

    
    // LÓGICA DE COSAS VISUALES DE LA PÁGINA
    private void OcultarTecldo()
    {
        Entry_NombreComprador.Unfocus();
        Entry_ApellidoComprador.Unfocus();
        Entry_TelefonoComprador.Unfocus();
    }

}