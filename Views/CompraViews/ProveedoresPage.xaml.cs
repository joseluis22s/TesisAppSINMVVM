using TesisAppSINMVVM.Database.Tables;
using TesisAppSINMVVM.Database.Respositories;

namespace TesisAppSINMVVM.Views.CompraViews;

public partial class ProveedoresPage : ContentPage
{
    private List<Tbl_Proveedor> _proveedores;
    private Tbl_Proveedor_Respository _repoProveedor;
    private Tbl_Proveedor _proveedor;
    private bool _enEjecucion;
    public static bool _permitirEjecucion = true;

    public ProveedoresPage()
	{
		InitializeComponent();
        _repoProveedor = new Tbl_Proveedor_Respository();
    }



    // NAVEGACI�N
    private async Task AgregarNuevoProveedorPagePushModalAsync()
    {
        await Navigation.PushModalAsync(new AgregarNuevoProveedorPage());
    }
    private async Task RegistrarNuevaCompraPagePushAsync(Tbl_Proveedor proveedor)
    {
        CollectionView_Proveedores.SelectedItem = null;
        _permitirEjecucion = false;
        await Navigation.PushAsync(new RegistrarNuevaCompraPage
        {
            BindingContext = proveedor
        });
    }
    private async Task ProveedoresPagePopAsync()
    {
        await Navigation.PopAsync();
    }


    // EVENTOS
    private void ContentPage_Appearing(object sender, EventArgs e)
    {
        if (_permitirEjecucion)
        {
            base.OnAppearing();
            CargarDatosCollectionView_Proveedores();
        }
        _permitirEjecucion = true;
    }
    private async void Button_AgregarProveedor_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await AgregarNuevoProveedorPagePushModalAsync();
        _enEjecucion = false;
    }
    private async void CollectionView_Proveedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        await SelectionChanged();
    }
    private async void Button_Regresar_Clicked(object sender, EventArgs e)
    {
        //if (_enEjecucion)
        //{
        //    return;
        //}
        //_enEjecucion = true;
        await ProveedoresPagePopAsync();
        //_enEjecucion = false;
    }
    protected override bool OnBackButtonPressed()
    {
        ProveedoresPagePopAsync().GetAwaiter();
        return true;
    }

    // LOGICA PARA EVENTOS
    private async void CargarDatosCollectionView_Proveedores()
    {
        _proveedores = await ObtenerProveedoresDBAsync();
        CollectionView_Proveedores.ItemsSource = _proveedores;
    }
    private async Task SelectionChanged()
    {
        if(_permitirEjecucion)
        {
            _proveedor = (Tbl_Proveedor)CollectionView_Proveedores.SelectedItem;
            await RegistrarNuevaCompraPagePushAsync(_proveedor);
        }
        _permitirEjecucion = true;
    }



    // L�GICA
    //private Task PagePopAsync()
    //{
    //    Dispatcher.Dispatch(async () =>
    //    {
    //        bool respuesta = await DisplayAlert("Alerta", "�Desea regresar? Perder� el progreso realizado", "Confimar", "Cancelar");
    //        if (respuesta)
    //        {
    //            await Navigation.PopAsync();
    //        }
    //    });
    //    return Task.CompletedTask;
    //}


    // BASE DE DATOS
    private async Task<List<Tbl_Proveedor>> ObtenerProveedoresDBAsync()
    {
        return await _repoProveedor.ObtenerProveedoresAsync();
    }



    // L�GICA DE COSAS VISUALES DE LA P�GINA

}