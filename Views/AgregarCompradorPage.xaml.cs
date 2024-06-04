using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.Database.Tables;

namespace TesisAppSINMVVM.Views;

public partial class AgregarCompradorPage : ContentPage
{
	private Tbl_Comprador_Repository _repoComprador;
	public AgregarCompradorPage()
	{
		InitializeComponent();
        _repoComprador = new Tbl_Comprador_Repository();
    }



    // NAVEGACIÓN


    // EVENTOS
    private async void Button_GuardarComprador_Clicked(object sender, EventArgs e)
    {
        await GuardarCompardor();
    }


    // LOGICA PARA EVENTOS
    private async Task GuardarCompardor()
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


    // BASE DE DATOS
    private async Task GuardarCompradorDBAsync(Tbl_Comprador comprador)
    {
        await _repoComprador.GuardarCompradorAsync(comprador);
    }


    // LÓGICA DE COSAS VISUALES DE LA PÁGINA

}