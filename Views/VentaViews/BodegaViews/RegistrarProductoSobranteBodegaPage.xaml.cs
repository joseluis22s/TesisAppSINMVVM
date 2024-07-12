using System.Collections.ObjectModel;
using TesisAppSINMVVM.Models;
using CommunityToolkit.Maui.Alerts;
using System.Text.RegularExpressions;
using TesisAppSINMVVM.FirebaseDataBase.Repositories;
using CommunityToolkit.Maui.Core;
using TesisAppSINMVVM.LocalDatabase.Tables;
using TesisAppSINMVVM.Views.ModalViews;

namespace TesisAppSINMVVM.Views.VentaViews;

public partial class RegistrarProductoSobranteBodegaPage : ContentPage
{
    private Producto_Repository _repoProducto = new Producto_Repository();
    private ProductoInventarioBodega_Repository _repoProductoInventarioBodega = new ProductoInventarioBodega_Repository();
    private ObservableCollection<AuxProductoInventarioBodega> _auxProductosInventario = new ObservableCollection<AuxProductoInventarioBodega>();
    private bool _enEjecucion;
    public static bool _ejecutarAppearing = true;
    public ObservableCollection<AuxProductoInventarioBodega> _AuxProductosInventario
    {
        get { return _auxProductosInventario; }
        set
        {
            _auxProductosInventario = value;
        }
    }
    //public ObservableCollection<AuxProductoInventarioBodega> _AuxProductosInventario { get; set; }

    public RegistrarProductoSobranteBodegaPage()
    {
        InitializeComponent();
    }



    // NAVEGACIÓN
    #region NAVEGACIÓN
    private async Task RegistrarProductoSobranteBodegaPagePopAsync()
    {
        await PermitirPopAsyncNavegacion();
    }
    private async Task HistorialRegistroProductoSobrantePagePushAsync()
    {
        await Navigation.PushAsync(new HistorialProductoSobrantePage());
    }
    private async Task AgregarNuevoProductoPagePushModalAsync()
    {
        NetworkAccess accessType = Connectivity.Current.NetworkAccess;
        if (accessType == NetworkAccess.Internet)
        {
            await Navigation.PushModalAsync(new AgregarNuevoProductoPage());
        }
        else
        {
            await Toast.Make("Primero debe conectarse a internet", ToastDuration.Long).Show();
        }
    }
    #endregion

    // EVENTOS
    #region EVENTOS
    private async void ContentPage_Appearing(object sender, EventArgs e)
    {
        if (_ejecutarAppearing)
        {
            base.OnAppearing();
            await CargarProductosInventario();
        }
        _ejecutarAppearing = true;
    }
    private async void Button_NavegarAgregarNuevoProducto_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await AgregarNuevoProductoPagePushModalAsync();
        _enEjecucion = false;
    }
    private async void Button_GuardarRegistroProductoSobrante_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await GuardarProductoSobranteAsync();
        _enEjecucion = false;
    }
    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        CheckBox checkBox = (CheckBox)sender;
        AuxProductoInventarioBodega producto = (AuxProductoInventarioBodega)checkBox.BindingContext;
        producto.ESSELECCIONADO = e.Value;
    }
    private async void Button_HistorialRegistroProductoSobrante_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await HistorialRegistroProductoSobrantePagePushAsync();
        _enEjecucion = false;
    }
    private void Entry_TextChanged_Cantidad(object sender, TextChangedEventArgs e)
    {
        Entry entry = (Entry)sender;
        LimpiarPuntosEntry(entry);
    }
    private async void Button_Regresar_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await RegistrarProductoSobranteBodegaPagePopAsync();
        _enEjecucion = false;
    }
    protected override bool OnBackButtonPressed()
    {
        RegistrarProductoSobranteBodegaPagePopAsync().GetAwaiter();
        return true;
    }

    #endregion

    // LÓGICA PARA EVENTOS
    #region LÓGICA PARA EVENTOS
    private async Task CargarProductosInventario()
    {
        var productos = await ObtenerProductosDBAsync();
        foreach (var p in productos)
        {
            _AuxProductosInventario.Add(new AuxProductoInventarioBodega
            {
                PRODUCTO = p.PRODUCTO,
                MEDIDA = p.MEDIDA
            });
        }
        CollectionView_RegistrarProductoSobranteBodega.ItemsSource = _AuxProductosInventario;
    }
    private async Task GuardarProductoSobranteAsync()
    {
        List<ProductoInventarioBodega> inventarioProductosBodega = new List<ProductoInventarioBodega>();
        _AuxProductosInventario = (ObservableCollection<AuxProductoInventarioBodega>)CollectionView_RegistrarProductoSobranteBodega.ItemsSource;

        foreach (var p in _AuxProductosInventario)
        {
            if (p.ESSELECCIONADO == true)
            {
                if (string.IsNullOrEmpty(p.CANTIDAD))
                {
                    _enEjecucion = false;
                    await Toast.Make("¡Los campos de cantidad no deben estar vacíos!", ToastDuration.Long).Show();
                    return;
                }
                else
                {
                    inventarioProductosBodega.Add(new ProductoInventarioBodega
                    {
                        PRODUCTO = p.PRODUCTO,
                        MEDIDA = p.MEDIDA,
                        CANTIDAD = int.Parse(p.CANTIDAD),
                        DESCRIPCION = p.DESCRIPCION,
                        FECHAGUARDADO = DateTime.Now.ToString("dd/MM/yyyy"),
                        DIAFECHAGUARDADO = DateTime.Now.ToString("dddd, dd MMMM")
                    });
                }
            }
        }
        if (inventarioProductosBodega.Count == 0)
        {
            await Toast.Make("Selecione al menos un producto", ToastDuration.Long   ).Show();
        }
        else
        {
            foreach (var p in _AuxProductosInventario)
            {
                p.CANTIDAD = "";
                p.DESCRIPCION = "";
                p.ESSELECCIONADO = false;
            }
            foreach (var productoInventarioBodega in inventarioProductosBodega)
            {
                await GuardarProductosInventarioDBAsync(productoInventarioBodega);
            }
            await Toast.Make("¡Registro guardado!",ToastDuration.Long).Show();
        }
    }
    #endregion

    // LÓGICA
    #region LÓGICA
    private async Task PermitirPopAsyncNavegacion()
    {
        int productosCargados = _AuxProductosInventario.Count;
        if (productosCargados != 0)
        {
            bool camposVacios = VerificarCamposVacios();
            if (!camposVacios)
            {
                bool respuesta = await DisplayAlert("Alerta", "¿Desea regresar? Perderá el progreso realizado", "Confimar", "Cancelar");
                if (respuesta)
                {
                    await Navigation.PopAsync();
                }
            }
            else
            {
                await Navigation.PopAsync();
            }
        }
        else
        {
            await Navigation.PopAsync();
        }
    }
    private bool VerificarCamposVacios()
    {
        int contador = 0;
        _AuxProductosInventario = (ObservableCollection<AuxProductoInventarioBodega>)CollectionView_RegistrarProductoSobranteBodega.ItemsSource;
        foreach (var p in _AuxProductosInventario)
        {
            if (p.ESSELECCIONADO == true)
            {
                contador++;
            }
        }
        if (contador == 0)
        {
            return true;
        }
        return false;
    }
    #endregion

    // BASE DE DATOS
    #region BASE DE DATOS
    private async Task<List<Tbl_Producto>> ObtenerProductosDBAsync()
    {
        return await _repoProducto.ObtenerProductosAsync();
    }
    private async Task GuardarProductosInventarioDBAsync(ProductoInventarioBodega productoInventarioBodega)
    {
        await _repoProductoInventarioBodega.GuardarProductosInventarioAsync(productoInventarioBodega);
    }
    #endregion

    // LÓGICA DE COSAS VISUALES DE LA PÁGINA
    #region LÓGICA DE COSAS VISUALES DE LA PÁGINA
    private void LimpiarPuntosEntry(Entry entry)
    {
        if (entry.Text.Contains("."))
        {
            entry.Text = Regex.Replace(entry.Text, @"\.", string.Empty);
        }
    }
    #endregion

    
}