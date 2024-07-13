using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using TesisAppSINMVVM.FirebaseDataBase.Repositories;
using TesisAppSINMVVM.LocalDatabase.Tables;
using TesisAppSINMVVM.Models;
using TesisAppSINMVVM.Views.ModalViews;

namespace TesisAppSINMVVM.Views.CompraViews;

public partial class RegistrarNuevaCompraPage : ContentPage
{
    private Producto_Repository _repoProducto = new Producto_Repository();
    private ProductoComprado_Repository _repoProductoComprado = new ProductoComprado_Repository();

    private string _nombreProveedor;
    private int _numeroCompra;
    private bool _enEjecucion;
    private double valorTotalVenta = 0;

    private ObservableCollection<AuxProducto> _auxProductos = new ObservableCollection<AuxProducto>();
    public ObservableCollection<AuxProducto> _AuxProductos
    {
        get { return _auxProductos; }
        set
        {
            _auxProductos = value;
        }
    }

    public RegistrarNuevaCompraPage()
    {
        InitializeComponent();
        Entry_ValorTotalVenta.Text = $"Valor total: {valorTotalVenta:F2}";
    }
    // NAVEGACIÓN
    #region NAVEGACIÓN
    private async Task RegistrarNuevaCompraPagePopAsync()
    {
        await PermitirPopAsyncNavegacion();
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
        // TODO: Verificar si se debe agregar un _ejecutarAppearing
        base.OnAppearing();
        await CargarNombreProveedor();
        await CargarProductos_CollectionView_Productos();
        await CargarNumeroCompra();
    }
    private async void Button_GuardarNuevaCompra_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await GuardarNuevaCompra();
        _enEjecucion = false;
    }
    private async void Button_Regresar_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await RegistrarNuevaCompraPagePopAsync();
        _enEjecucion = false;

    }
    protected override bool OnBackButtonPressed()
    {
        RegistrarNuevaCompraPagePopAsync().GetAwaiter();
        return true;
    }
    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        CheckBox checkBox = (CheckBox)sender;
        AuxProducto producto = (AuxProducto)checkBox.BindingContext;
        producto.ESSELECCIONADO = e.Value;
        CalcularTotal_TextChanged();
    }
    private void Entrys_TextChanged(object sender, TextChangedEventArgs e)
    {
        Entry entry = (Entry)sender;
        Entrys_TextoCambiado(entry);
    }
    private async void Button_AgregarProducto_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await AgregarNuevoProductoPagePushModalAsync();
        _enEjecucion = false;
    }

    #endregion


    // LÓGICA PARA EVENTOS
    #region LÓGICA PARA EVENTOS
    private async Task CargarNombreProveedor()
    {
        Proveedor proveedor = (Proveedor)this.BindingContext;
        _nombreProveedor = proveedor.PROVEEDOR;
    }
    private async Task CargarProductos_CollectionView_Productos()
    {
        var productos = await ObtenerProductosDBAsync();
        int aa = 0;
        if (/*productos.Count*/aa == 0)
        {
            VerticalStackLayout_EmptyView.IsVisible = true;
            CollectionView_Productos.IsVisible = false;
        }
        else
        {
            VerticalStackLayout_EmptyView.IsVisible = false;
            foreach (var p in productos)
            {
                _AuxProductos.Add(new AuxProducto
                {
                    PRODUCTO = p.PRODUCTO,
                    MEDIDA = p.MEDIDA
                });
            }
            CollectionView_Productos.ItemsSource = _AuxProductos;
        }
    }
    private async Task CargarNumeroCompra()
    {
        List<int> numerosComprasExistentes = await ObtenerNumeroComprasDBAsync(_nombreProveedor);
        int numeroMayor = numerosComprasExistentes.Max();
        _numeroCompra = numeroMayor + 1;
        Entry_NumeroCompra.Text = "Compra N°" + _numeroCompra;
    }
    private async Task GuardarNuevaCompra()
    {
        NetworkAccess accessType = Connectivity.Current.NetworkAccess;
        if (accessType == NetworkAccess.Internet)
        {
            List<ProductoComprado> productosComprados = new List<ProductoComprado>();
            _AuxProductos = (ObservableCollection<AuxProducto>)CollectionView_Productos.ItemsSource;
            foreach (var p in _AuxProductos)
            {
                if (p.ESSELECCIONADO == true)
                {
                    bool camposVacios = ComprobarCamposVaciosProductos(p);
                    if (camposVacios)
                    {
                        _enEjecucion = false;
                        await Toast.Make("Existen campos incompletos o erróneos", ToastDuration.Long).Show();
                        return;
                    }
                    else
                    {
                        Proveedor proveedor = (Proveedor)this.BindingContext;
                        productosComprados.Add(new ProductoComprado
                        {
                            NUMEROCOMPRA = _numeroCompra,
                            PRODUCTO = p.PRODUCTO,
                            MEDIDA = p.MEDIDA,
                            CANTIDAD = int.Parse(p.CANTIDAD),
                            PRECIO = double.Parse(p.PRECIOE + "." +p.PRECIOD),
                            TOTAL = double.Parse(p.TOTAL),
                            FECHAGUARDADO = DatePicker_FechaGuardado.Date.ToString("dd MMMM yyyy"),
                            DIAFECHAGUARDADO = DatePicker_FechaGuardado.Date.ToString("dddd, dd MMMM yyyy"),
                            PROVEEDOR = _nombreProveedor
                            // TODO: Verificar si se pasa el proveedor
                        });
                    }
                }
            }
            if (productosComprados.Count == 0)
            {
                await Toast.Make("Selecione al menos un producto", ToastDuration.Long).Show();
            }
            else
            {
                foreach (var p in _AuxProductos)
                {
                    p.CANTIDAD = "";
                    p.ESSELECCIONADO = false;
                }
                foreach (var productoComprado in productosComprados)
                {
                    await GuardarNuevaCompraProductoCompradoDBAsync(productoComprado);
                }
                await Toast.Make("¡Registro guardado!", ToastDuration.Long).Show();
            }
        }
        else
        {
            await Toast.Make("Primero debe conectarse a internet", ToastDuration.Long).Show();
        }

    }
    private void Entrys_TextoCambiado(Entry entry)
    {
        LimpiarPuntosEntry(entry);
        CalcularTotal_TextChanged();
    }
    #endregion
    private void CalcularTotal_TextChanged()
    {
        _AuxProductos = (ObservableCollection<AuxProducto>)CollectionView_Productos.ItemsSource;
        valorTotalVenta = 0.0;
        foreach (var p in _AuxProductos)
        {
            if (p.ESSELECCIONADO == true)
            {
                string precioCompleto = p.PRECIOE + "." + p.PRECIOD;
                if (!string.IsNullOrEmpty(p.CANTIDAD) && (double.TryParse(precioCompleto, out double precio)))
                {
                    int cantidad = int.Parse(p.CANTIDAD);
                    double total = precio * cantidad;
                    p.TOTAL = total.ToString("F2");
                    valorTotalVenta += total;
                }
                else
                {
                    p.TOTAL = "";
                }
            }
        }
        Entry_ValorTotalVenta.Text = $"Valor total: {valorTotalVenta:F2}";
    }
    // LÓGICA
    #region LÓGICA
    private bool ComprobarCamposVaciosProductos(AuxProducto auxProducto)
    {
        if (string.IsNullOrEmpty(auxProducto.CANTIDAD) || string.IsNullOrEmpty(auxProducto.PRECIOD) &&
            string.IsNullOrEmpty(auxProducto.PRECIOE))
        {
            return true;
        }
        return false;
    }
    private bool VerificarCamposVacios()
    {
        int contador = 0;
        _AuxProductos = (ObservableCollection<AuxProducto>)CollectionView_Productos.ItemsSource;
        foreach (var p in _AuxProductos)
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
    private async Task PermitirPopAsyncNavegacion()
    {
        int productosCargados = _AuxProductos.Count;
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
    #endregion


    // BASE DE DATOS
    #region BASE DE DATOS
    private async Task<List<Tbl_Producto>> ObtenerProductosDBAsync()
    {
        return await _repoProducto.ObtenerProductosAsync();
    }
    private async Task<List<int>> ObtenerNumeroComprasDBAsync(string nombreProveedor)
    {
        return await _repoProductoComprado.ObtenerNumerosComprasAsync(nombreProveedor);
    }
    private async Task GuardarNuevaCompraProductoCompradoDBAsync(ProductoComprado productoComprado)
    {
        await _repoProductoComprado.GuardarNuevaCompraProductoCompradoAsync(productoComprado);
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