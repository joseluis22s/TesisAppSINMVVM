using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.Database.Tables;

namespace TesisAppSINMVVM.Views;

public partial class AgregarCompradorPage : ContentPage
{
	private Tbl_Comprador_Repository _repoComprador;
    private bool _enEjecucion;
    public AgregarCompradorPage()
	{
		InitializeComponent();
        _repoComprador = new Tbl_Comprador_Repository();
    }



    // NAVEGACIÓN
    private async Task AgregarNuevoCompradorPagePushModalAsync(bool mostrarAlerta)
    {
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


    // LOGICA PARA EVENTOS
    private async Task GuardarComprador()
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
    }


    // LÓGICA
    private async Task PermitirPopModalAsyncNavegacion(bool mostrarAlerta)
    {
        //OcultarTeclado();
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


    // BASE DE DATOS
    private async Task GuardarCompradorDBAsync(Tbl_Comprador comprador)
    {
        await _repoComprador.GuardarCompradorAsync(comprador);
    }

    


    // LÓGICA DE COSAS VISUALES DE LA PÁGINA

}