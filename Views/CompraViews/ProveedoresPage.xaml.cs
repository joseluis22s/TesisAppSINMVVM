using TesisAppSINMVVM.Models;
using Plugin.CloudFirestore;
using TesisAppSINMVVM.Database.Tables;
using TesisAppSINMVVM.Database.Respositories;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace TesisAppSINMVVM.Views.CompraViews;

public partial class ProveedoresPage : ContentPage
{
    private List<Proveedor> _proveedores;
    private Proveedor _proveedor;
    private bool _enEjecucion;
    public static bool _permitirEjecucion = true;

    public ProveedoresPage()
	{
		InitializeComponent();
    }



    // NAVEGACIÓN
    #region NAVEGACIÓN
    private async Task RegistrarNuevaCompraPagePushAsync(Proveedor proveedor)
    {
        CollectionView_Proveedores.SelectedItem = null;
        _permitirEjecucion = false;
        await Navigation.PushAsync(new CompraOpcionesPage
        {
            BindingContext = proveedor
        });
    }
    private async Task ProveedoresPagePopAsync()
    {
        await Navigation.PopAsync();
    }

    #endregion


    // EVENTOS
    #region EVENTOS
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
        await ProveedorSelecionado();
    }
    private async void Button_Regresar_Clicked(object sender, EventArgs e)
    {
        await ProveedoresPagePopAsync();
    }
    protected override bool OnBackButtonPressed()
    {
        ProveedoresPagePopAsync().GetAwaiter();
        return true;
    }

    #endregion


    // LÓGICA PARA EVENTOS
    #region LÓGICA PARA EVENTOS
    private async Task CargarDatosCollectionView_Proveedores()
    {
        _proveedores = await ObtenerProveedoresAsync();
        CollectionView_Proveedores.ItemsSource = _proveedores;
    }
    private async Task ProveedorSelecionado()
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
            if (nuevoProveedor is not null)
            {
                if (nuevoProveedor != "")
                {
                    nuevoProveedor = nuevoProveedor.Trim().ToUpper();
                    bool existeProveedor = _proveedores.Any(p => p.PROVEEDOR == nuevoProveedor);
                    if (!existeProveedor)
                    {
                        Proveedor proveedor = new Proveedor();
                        proveedor.PROVEEDOR = nuevoProveedor;
                        await GuardarNuevoProveedorAsync(proveedor);
                        await CargarDatosCollectionView_Proveedores();
                        await Toast.Make("Se ha guardado el nuevo proveedor", ToastDuration.Long).Show();
                    }
                    else
                    {
                        await Toast.Make("Proveedor ya existente", ToastDuration.Long).Show();
                    }
                }
                else
                {
                    await Toast.Make("El campo no debe estar vacío").Show();
                }
            }
        } while (nuevoProveedor == "");
    }

    #endregion


    // LÓGICA
    #region LÓGICA
    #endregion


    // BASE DE DATOS
    #region BASE DE DATOS
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
    #endregion


    // LÓGICA DE COSAS VISUALES DE LA PÁGINA



}