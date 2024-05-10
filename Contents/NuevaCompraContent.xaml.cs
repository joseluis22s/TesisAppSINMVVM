
namespace TesisAppSINMVVM.Contents;

public partial class NuevaCompraContent : ContentView
{
    public ContentPage ParentPage { get; set; }
    public NuevaCompraContent()
	{
		InitializeComponent();
        BindingContext = this;
        ParentPage = (ContentPage)Parent;
    }

    // NAVEGACIÓN
    // EVENTOS
    private async void Button_GuardarNuevaCompra_Clicked(object sender, EventArgs e)
    {
        await AgregarNuevaCompra();
    }
    private async void Button_AgregarProductoPicker_Clicked(object sender, EventArgs e)
    {
        // TODO: Hacer una actionsheet
        //await AgregarProductoListado();
    }
    // LOGICA PARA EVENTOS
    private Task AgregarNuevaCompra()
    {
        string fecha = DateTime.Now.ToString("dd/MM/yyyy");
        string producto = Picker_Producto.SelectedItem.ToString();
        double precio = double.Parse(Entry_Precio.Text);
        int cantidad = int.Parse(Entry_Cantidad.Text);
        Label_ValorTotal.Text = (precio * cantidad).ToString();
        
        return Task.CompletedTask;
    }

    //private async Task AgregarProductoListado()
    //{
    //    //await ParentPage.DisplayPromptAsync("Agregar producto", "Ingrese el nombre del producto:");
    //    await 
    //}
    


    // BASE DE DATOS
    // LÓGICA DE COSAS VISUALES DE LA PÁGINA


}