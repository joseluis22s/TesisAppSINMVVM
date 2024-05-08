using TesisAppSINMVVM.Contents;
namespace TesisAppSINMVVM.Views;

public partial class CompraPage : ContentPage
{
    public ContentView NuevaCompraView { get; set; }
    public ContentView HistorialComprasView { get; set; }

    public CompraPage()
	{
		InitializeComponent();
        NuevaCompraView = new NuevaCompraContent();
        HistorialComprasView = new HistorialComprasContent();
        //Content = NuevaCompraView;
    }   

    private void Button_NuevaCompra_Clicked(object sender, EventArgs e)
    {
        ContentView_CompraPageContenidoDinamico.Content = NuevaCompraView;
    }

    private void Button_Historial_Clicked(object sender, EventArgs e)
    {
        ContentView_CompraPageContenidoDinamico.Content = HistorialComprasView;
    }
    // NAVEGACIÓN


    // EVENTOS

    // LOGICA PARA EVENTOS

    // BASE DE DATOS

    // LÓGICA DE COSAS VISUALES DE LA PÁGINA
}