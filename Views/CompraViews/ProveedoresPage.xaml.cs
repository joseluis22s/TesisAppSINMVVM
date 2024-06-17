using TesisAppSINMVVM.Models;
using Plugin.CloudFirestore;
using TesisAppSINMVVM.Database.Tables;
using TesisAppSINMVVM.Database.Respositories;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace TesisAppSINMVVM.Views.CompraViews;

public partial class ProveedoresPage : ContentPage
{
    //private List<Tbl_Proveedor> _proveedores;
    private List<Proveedor> _proveedores;
    private Proveedor _proveedor;
    //private Tbl_Proveedor_Respository _repoProveedor;
    //private Tbl_Proveedor _proveedor;
    private bool _enEjecucion;
    public static bool _permitirEjecucion = true;

    public ProveedoresPage()
	{
		InitializeComponent();
        //_repoProveedor = new Tbl_Proveedor_Respository();
    }



    // NAVEGACIÓN
    private async Task AgregarNuevoProveedorPagePushModalAsync()
    {
        await Navigation.PushModalAsync(new AgregarNuevoProveedorPage());
    }
    private async Task RegistrarNuevaCompraPagePushAsync(Proveedor proveedor)
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
        await AgregarNuevoProveedor();
        //await AgregarNuevoProveedorPagePushModalAsync();
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
        _proveedores = await ObtenerProveedoresAsync();
        CollectionView_Proveedores.ItemsSource = _proveedores;
    }
    private async Task SelectionChanged()
    {
        if(_permitirEjecucion)
        {
            _proveedor = (Proveedor)CollectionView_Proveedores.SelectedItem;
            await RegistrarNuevaCompraPagePushAsync(_proveedor);
        }
        _permitirEjecucion = true;
    }

    private async Task AgregarNuevoProveedor()
    {
        string nuevoProveedor;
        do
        {
            nuevoProveedor = await DisplayPromptAsync("NUEVO PROVEEDOR", "Ingrese el nombre del producto:", "AGREGAR", "CANCELAR", null, -1, Keyboard.Create(KeyboardFlags.CapitalizeCharacter));
            nuevoProveedor = nuevoProveedor.Trim().ToUpper();
            bool existeProveedor = _proveedores.Any(p => p.NOMBRE == nuevoProveedor);
            if (!existeProveedor)
            {
                Proveedor proveedor = new Proveedor();
                proveedor.NOMBRE = nuevoProveedor;
                await GuardarNuevoProveedorAsync(proveedor);
                _proveedores = await ObtenerProveedoresAsync();
                CollectionView_Proveedores.ItemsSource = _proveedores;
                await Toast.Make("Se ha guardado el nuevo proveedor",ToastDuration.Long).Show();
            }
        } while (nuevoProveedor == "" );
        
    }
    //private async Task AgregarNuevoProducto()
    //{
    //    string nuevoProducto;
    //    do
    //    {
    //        nuevoProducto = await _compraPage.DisplayPromptAsync("NUEVO PRODUCTO", "Ingrese el nombre del producto:", "AGREGAR", "CANCELAR", null, -1, Keyboard.Create(KeyboardFlags.CapitalizeCharacter));
    //        if (nuevoProducto is not null)
    //        {
    //            if (nuevoProducto != "")
    //            {
    //                //string nuevoProducto = await _compraPage.DisplayPromptAsync("NUEVO PRODUCTO", "Ingrese el nombre del producto:", "AGREGAR", "CANCELAR", null,-1,Keyboard.Create(KeyboardFlags.CapitalizeCharacter));
    //                // TODO: De la misma que el double y el int, contorlar los espasio en blanco
    //                bool existeProducto = await VerificarExistenciaProductoDBAsync(nuevoProducto);
    //                if (existeProducto)
    //                {
    //                    await Toast.Make("El producto ya existe").Show();
    //                }
    //                else
    //                {

    //                    await GuardarProductoDBAsync(nuevoProducto);
    //                    await CargarProductos();
    //                    await Toast.Make("el producto se ha guardado").Show();
    //                }
    //            }
    //            else
    //            {
    //                await Toast.Make("¡El campo no debe estar vacío!").Show();
    //            }

    //        }
    //    }
    //    while (nuevoProducto == "");

    //}



    // LÓGICA


    // BASE DE DATOS
    private async Task<List<Proveedor>> ObtenerProveedoresAsync()
    {
        var documentos = await CrossCloudFirestore.Current
                                     .Instance
                                     .Collection("PROVEEDOR")
                                     .GetAsync();
        var proveedores = documentos.ToObjects<Proveedor>().ToList();
        return proveedores;
    }
    private async Task GuardarNuevoProveedorAsync(Proveedor proveedor){
        await CrossCloudFirestore.Current
                         .Instance
                         .Collection("PROVEEDOR")
                         .AddAsync(proveedor);
    }
    //private async Task<List<Tbl_Proveedor>> ObtenerProveedoresDBAsync()
    //{
    //    return await _repoProveedor.ObtenerProveedoresAsync();
    //}



    // LÓGICA DE COSAS VISUALES DE LA PÁGINA

}