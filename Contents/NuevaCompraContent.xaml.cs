using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.Contents;

public partial class NuevaCompraContent : ContentView
{
    public List<CompraGroupModel> Compras { get; private set; } = new List<CompraGroupModel>();

    public NuevaCompraContent()
	{
		InitializeComponent();
	}

    // NAVEGACI�N
    // EVENTOS
    // LOGICA PARA EVENTOS
    private void AgregarNuevaCompra()
    {
        string fecha = DateTime.Now.ToString("dd/MM/yyyy");
        Compras.Add(new CompraGroupModel(fecha, new List<CompraModel>
        {
            new CompraModel
            {

            }
        }));
    }
    // BASE DE DATOS
    // L�GICA DE COSAS VISUALES DE LA P�GINA


}