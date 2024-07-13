using TesisAppSINMVVM.Models;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using TesisAppSINMVVM.FirebaseDataBase.Repositories;
using TesisAppSINMVVM.LocalDatabase.Tables;

namespace TesisAppSINMVVM.Views.CompraViews;

public partial class ProveedoresPage : ContentPage
{
    private Proveedor_Repository _repoProveedores = new Proveedor_Repository();
    private List<Tbl_Proveedor> _tblProveedores = new List<Tbl_Proveedor>();
    private Proveedor _proveedor = new Proveedor();
    private Tbl_Proveedor _tblproveedor = new Tbl_Proveedor();
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
    private async void ContentPage_Appearing(object sender, EventArgs e)
    {
        if (_permitirEjecucion)
        {
            base.OnAppearing();
            await CargarDatosCollectionView_Proveedores();
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
    private async void Button_EliminarProveedor_Clicked(object sender, EventArgs e)
    {
        await EliminarProveedor(sender);
    }

    private async void Button_EditarProveedor_Clicked(object sender, EventArgs e)
    {
        await EditarProveedor(sender);
    }
    #endregion

    
    // LÓGICA PARA EVENTOS
    #region LÓGICA PARA EVENTOS
    
    private async Task CargarDatosCollectionView_Proveedores()
    {
        _tblProveedores = await ObtenerProveedoresDBAsync();
        int aa = 0;
        if (_tblProveedores.Count == 0)
        {
            VerticalStackLayout_EmptyView.IsVisible = true;
            CollectionView_Proveedores.IsVisible = false;
        }
        else
        {
            VerticalStackLayout_EmptyView.IsVisible = false;
            CollectionView_Proveedores.ItemsSource = _tblProveedores;
        }
        //_tblProveedores = await ObtenerProveedoresDBAsync();
        //CollectionView_Proveedores.ItemsSource = _tblProveedores;

    }
    private async Task ProveedorSelecionado()
    {
        if (_permitirEjecucion)
        {
            _tblproveedor = (Tbl_Proveedor)CollectionView_Proveedores.SelectedItem;
            _proveedor.PROVEEDOR = _tblproveedor.PROVEEDOR;
            await RegistrarNuevaCompraPagePushAsync(_proveedor);
        }
        _permitirEjecucion = true;
    }
    private async Task AgregarNuevoProveedor()
    {
        NetworkAccess accessType = Connectivity.Current.NetworkAccess;
        if (accessType == NetworkAccess.Internet)
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
                        bool existeProveedor = _tblProveedores.Any(p => p.PROVEEDOR == nuevoProveedor);
                        if (!existeProveedor)
                        {
                            Proveedor proveedor = new Proveedor();
                            proveedor.PROVEEDOR = nuevoProveedor;
                            await GuardarNuevoProveedorDBAsync(proveedor);
                            await Toast.Make("Se ha guardado el nuevo proveedor", ToastDuration.Long).Show();
                            await CargarDatosCollectionView_Proveedores();
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
        else
        {
            await Toast.Make("Primero debe conectarse a internet", ToastDuration.Long).Show();
        }
    }
    private async Task EliminarProveedor(object sender)
    {
        NetworkAccess accessType = Connectivity.Current.NetworkAccess;
        if (accessType == NetworkAccess.Internet)
        {
            Button button = (Button)sender;
            Tbl_Proveedor tblProveedor = (Tbl_Proveedor)button.BindingContext;
            Proveedor proveedor = new Proveedor()
            {
                PROVEEDOR = tblProveedor.PROVEEDOR
            };
            await BorrarProveedorDBAsync(proveedor);
            await CargarDatosCollectionView_Proveedores();
        }
        else
        {
            await Toast.Make("Para eliminar un proveedor debe conectarse a internet", ToastDuration.Long).Show();
        }
    }
    private async Task EditarProveedor(object sender)
    {
        NetworkAccess accessType = Connectivity.Current.NetworkAccess;
        if (accessType == NetworkAccess.Internet)
        {
            Button button = (Button)sender;
            Tbl_Proveedor tblProveedor = (Tbl_Proveedor)button.BindingContext;
            Proveedor proveedorActual = new Proveedor()
            {
                PROVEEDOR = tblProveedor.PROVEEDOR
            };
            string nuevoNombreProveeedor;
            do
            {
                nuevoNombreProveeedor = await DisplayPromptAsync("CAMBIAR NOMBRE DE PROVEEDOR", "Ingrese el nuevo nombre para: " + proveedorActual.PROVEEDOR, "AGREGAR", "CANCELAR", null, -1, Keyboard.Create(KeyboardFlags.CapitalizeCharacter));
                if (nuevoNombreProveeedor is not null)
                {
                    if (nuevoNombreProveeedor != "")
                    {
                        nuevoNombreProveeedor = nuevoNombreProveeedor.Trim().ToUpper();
                        bool existeProveedor = _tblProveedores.Any(p => p.PROVEEDOR == nuevoNombreProveeedor);
                        if (!existeProveedor)
                        {
                            //    Proveedor proveedor = new Proveedor();
                            //    proveedor.PROVEEDOR = nuevoProveedor;
                            //await GuardarNuevoProveedorDBAsync(proveedor);
                            await EditarProveedorDBAsync(proveedorActual, nuevoNombreProveeedor);
                            await Toast.Make("Se ha cambiado " + proveedorActual.PROVEEDOR + " a " + nuevoNombreProveeedor, ToastDuration.Long).Show();
                            await CargarDatosCollectionView_Proveedores();
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
            } while (nuevoNombreProveeedor == "");
        }
        else
        {
            await Toast.Make("Para editar un proveedor debe conectarse a internet", ToastDuration.Long).Show();
        }
    }
    #endregion


    // LÓGICA
    #region LÓGICA
    #endregion


    // BASE DE DATOS
    #region BASE DE DATOS
    private async Task<List<Tbl_Proveedor>> ObtenerProveedoresDBAsync()
    {
        return await _repoProveedores.ObtenerProveedoresAsync();
    }
    private async Task GuardarNuevoProveedorDBAsync(Proveedor proveedor)
    {
        await _repoProveedores.GuardarNuevoProveedorAsync(proveedor);
    }
    private async Task BorrarProveedorDBAsync(Proveedor proveedor)
    {
        await _repoProveedores.BorrarProveedorAsync(proveedor);
    }
    private async Task EditarProveedorDBAsync(Proveedor proveedor, string nuevoNombreProveeedor)
    {
        await _repoProveedores.EditarProveedorAsync(proveedor,nuevoNombreProveeedor);
    }
    #endregion

}