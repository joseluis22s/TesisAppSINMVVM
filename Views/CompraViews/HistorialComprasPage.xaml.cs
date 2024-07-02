using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.Database.Tables;
using TesisAppSINMVVM.FirebaseDataBase.Repositories;
using TesisAppSINMVVM.Models;
using TesisAppSINMVVM.Models.Groups;

namespace TesisAppSINMVVM.Views.CompraViews;

public partial class HistorialComprasPage : ContentPage
{
    private HistorialCompras_Repository _repositoryHistorialCompras = new HistorialCompras_Repository();
    private Proveedor _proveedor;
    public List<HistorialComprasGroup> _historialCompras { get; private set; } = new List<HistorialComprasGroup>();

    public HistorialComprasPage()
	{
		InitializeComponent();
        _proveedor = (Proveedor)BindingContext;
    }


    // NAVEGACI�N
    #region NAVEGACI�N
    private async Task HistorialComprasPagePopAsync()
    {
        await Navigation.PopAsync();
    }
    private async Task RegistrarNuevaCompraPagePushAsync()
    {
        await NagevacionRegistrarNuevaCompraPage();
    }
    #endregion
    


    // EVENTOS
    #region EVENTOS
    private async void ContentPage_Appearing(object sender, EventArgs e)
    {
        base.OnAppearing();
        await CargarDatosCollectionView_HistorialCompras();
    }
    private async void Button_Regresar_Clicked(object sender, EventArgs e)
    {
        await HistorialComprasPagePopAsync();
    }
    private async void Button_NavegarAgregarNuevaCompra_Clicked(object sender, EventArgs e)
    {
        await RegistrarNuevaCompraPagePushAsync();
    }
    #endregion

    // L�GICA PARA EVENTOS
    #region L�GICA PARA EVENTOS
    private async Task CargarDatosCollectionView_HistorialCompras()
    {
        _proveedor = (Proveedor)BindingContext;
        var compras = await ObtenerHistorialProductosDBAsync(_proveedor.PROVEEDOR);
        var gruposComprasEmitido = compras.OrderByDescending(c => DateTime.Parse(c.FECHA))
            .GroupBy(_grs => _grs.DIAFECHA)
        .Select(g => new HistorialComprasGroup(g.Key, g.ToList()));
        //var gruposCompras = compras.GroupBy(_grs => _grs.DIAFECHA)
        //    .Select(g => new HistorialComprasGroup(g.Key, g.ToList()));

        //TODO: Solo mostar los registros desde hace un mes, agregar un
        //      boton para ver todos los regsitros (desde hace 3 meses)
        _historialCompras.Clear();
        foreach (var grupo in gruposComprasEmitido)
        {
            _historialCompras.Add(grupo);
        }
        CollectionView_HistorialCompras.ItemsSource = _historialCompras;
    }
    #endregion

    // L�GICA
    #region L�GICA
    private async Task NagevacionRegistrarNuevaCompraPage()
    {
        var stack = Navigation.NavigationStack.ToArray();
        var lastPage = stack[stack.Length - 2];
        if (lastPage is RegistrarNuevaCompraPage)
        {
            await Navigation.PopAsync();
        }
        else if (lastPage is CompraOpcionesPage)
        {
            await Navigation.PushAsync(new RegistrarNuevaCompraPage
            {
                BindingContext = _proveedor
            });
            Navigation.RemovePage(stack[stack.Length - 1]);
        }
    }
    #endregion

    // BASE DE DATOS
    #region BASE DE DATOS
    private async Task<List<Tbl_HistorialCompras>> ObtenerHistorialProductosDBAsync(string proveedor)
    {
        return await _repositoryHistorialCompras.ObtenerHistorialProductosAsync(proveedor);
    }

    #endregion

    // L�GICA DE COSAS VISUALES DE LA P�GINA
    #region L�GICA DE COSAS VISUALES DE LA P�GINA

    #endregion

    
}