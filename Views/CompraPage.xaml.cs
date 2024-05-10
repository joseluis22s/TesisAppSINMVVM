using System.Collections.ObjectModel;
using TesisAppSINMVVM.Contents;
using TesisAppSINMVVM.Models;
namespace TesisAppSINMVVM.Views;

public partial class CompraPage : ContentPage
{
    private ContentView NuevaCompraView { get; set; }
    private ContentView HistorialComprasView { get; set; }
    private ObservableCollection<ProveedorItemModel> Proveedores = new ObservableCollection<ProveedorItemModel>();

    public CompraPage()
	{
		InitializeComponent();
        NuevaCompraView = new NuevaCompraContent();
        HistorialComprasView = new HistorialComprasContent();
        ContentView_CompraPageContenidoDinamico.Content = NuevaCompraView;
    }

    // NAVEGACIÓN
    // EVENTOS

    private void Button_NuevaCompra_Clicked(object sender, EventArgs e)
    {
        ContentView_CompraPageContenidoDinamico.Content = NuevaCompraView;
    }

    private void Button_Historial_Clicked(object sender, EventArgs e)
    {
        ContentView_CompraPageContenidoDinamico.Content = HistorialComprasView;
    }
    private void Button_AgregarProveedor_Clicked(object sender, EventArgs e)
    {

    }

    // LOGICA PARA EVENTOS
    //private Task AgregarNuevoProveedor()
    //{
    //    Proveedores.Add
    //}

    

    // LÓGICA


    // BASE DE DATOS

    // LÓGICA DE COSAS VISUALES DE LA PÁGINA
}