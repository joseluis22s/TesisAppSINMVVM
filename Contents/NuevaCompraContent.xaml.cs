using CommunityToolkit.Maui.Alerts;
using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.Database.Tables;

namespace TesisAppSINMVVM.Contents;

public partial class NuevaCompraContent : ContentView
{
    private Tbl_Producto_Repository Tbl_Producto_repo = new Tbl_Producto_Repository();
    private List<Tbl_Producto> productos;
    NavigationPage navigationPage = Application.Current.MainPage as NavigationPage;
    ContentPage currentPage; 
    public NuevaCompraContent()
	{
		InitializeComponent();
        currentPage = (ContentPage?)navigationPage.CurrentPage;
    }

    // NAVEGACIÓN
    // EVENTOS
    private async void Button_GuardarNuevaCompra_Clicked(object sender, EventArgs e)
    {
        await AgregarNuevaCompra();
    }
    private async void Button_AgregarNuevoProductoPicker_Clicked(object sender, EventArgs e)
    {
        string nuevoProducto = await currentPage.DisplayPromptAsync("Agregar producto", "Ingrese el nombre del producto:", "ACEPTAR", "CANCELAR");
        bool resultado = await VerificarExistenciaProductoDBAsync(nuevoProducto);
        if (resultado)
        {
            await Toast.Make("Producto ya existente").Show();
        }
        else
        {
            await GuardarProductoDBAsync(nuevoProducto);
            await Toast.Make("Producto guardado con éxito").Show();
        }
    }
    private async void ContentView_Loaded(object sender, EventArgs e)
    {
        productos = await ObtenerProdcutosDBAsync();
        Picker_Producto.ItemsSource = productos;
    }
    private async  void Picker_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        productos = await ObtenerProdcutosDBAsync();
        Picker_Producto.ItemsSource = productos;
        bool resultado = productos.Count == 0;
        if (resultado)
        {
            await Toast.Make("Agregue al meno un producto").Show();
        }
        else
        {
            Picker_Producto.Focus();
        }
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
    private async Task<bool> VerificarExistenciaProductoDBAsync(string resultado)
    {
        return await Tbl_Producto_repo.VerificarExistenciaProductoAsync(resultado);
    }
    private async Task GuardarProductoDBAsync(string producto)
    {
        await Tbl_Producto_repo.GuardarProductoAsync(producto);
    }
    private async Task<List<Tbl_Producto>> ObtenerProdcutosDBAsync()
    {
        return await Tbl_Producto_repo.ObtenerProdcutosAsync();
    }
    






    // LÓGICA DE COSAS VISUALES DE LA PÁGINA


}