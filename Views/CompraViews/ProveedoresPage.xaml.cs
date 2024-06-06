using TesisAppSINMVVM.Database.Tables;
using TesisAppSINMVVM.Database.Respositories;

namespace TesisAppSINMVVM.Views.CompraViews;

public partial class ProveedoresPage : ContentPage
{
    private List<Tbl_Proveedor> _proveedores;
    private Tbl_Proveedor_Respository _repoProveedor;
    private Tbl_Proveedor _proveedor;

    public ProveedoresPage()
	{
		InitializeComponent();
        _repoProveedor = new Tbl_Proveedor_Respository();

    }



    // NAVEGACIÓN
    private async Task AgregarNuevoProveedorPagePushModalAsync()
    {
        await Navigation.PushModalAsync(new AgregarNuevoProveedorPage());
    }
    private async Task RegistrarNuevaCompraPagePushAsync(Tbl_Proveedor proveedor)
    {
        await Navigation.PushAsync(new RegistrarNuevaCompraPage
        {
            BindingContext = proveedor
        });
    }
    private async Task ProveedoresPagePopAsync()
    {
        await PagePopAsync();
    }


    // EVENTOS
    private void ContentPage_Appearing(object sender, EventArgs e)
    {
        base.OnAppearing();
        CargarDatosCollectionView_Proveedores();
    }
    private async void Button_AgregarProveedor_Clicked(object sender, EventArgs e)
    {
        await AgregarNuevoProveedorPagePushModalAsync();
    }
    private async void CollectionView_Proveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        await SelectionChanged();
    }
    private async void Button_Regresar_Clicked(object sender, EventArgs e)
    {
        await ProveedoresPagePopAsync();
    }



    // LOGICA PARA EVENTOS
    private async void CargarDatosCollectionView_Proveedores()
    {
        _proveedores = await ObtenerProveedoresDBAsync();
        CollectionView_Proveedores.ItemsSource = _proveedores;
    }
    private async Task SelectionChanged()
    {
        _proveedor = (Tbl_Proveedor)CollectionView_Proveedores.SelectedItem;
        await RegistrarNuevaCompraPagePushAsync(_proveedor);
    }



    // LÓGICA
    private Task PagePopAsync()
    {
        Dispatcher.Dispatch(async () =>
        {
            bool respuesta = await DisplayAlert("Alerta", "¿Desea regresar? Perderá el progreso realizado", "Confimar", "Cancelar");
            if (respuesta)
            {
                await Navigation.PopAsync();
            }
        });
        return Task.CompletedTask;
    }


    // BASE DE DATOS
    private async Task<List<Tbl_Proveedor>> ObtenerProveedoresDBAsync()
    {
        return await _repoProveedor.ObtenerProveedoresAsync();
    }



    // LÓGICA DE COSAS VISUALES DE LA PÁGINA

}