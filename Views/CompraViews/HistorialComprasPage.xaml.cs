using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.Database.Tables;
using TesisAppSINMVVM.FirebaseDataBase.Repositories;
using TesisAppSINMVVM.Models;
using TesisAppSINMVVM.Models.Groups;

namespace TesisAppSINMVVM.Views.CompraViews;

public partial class HistorialComprasPage : ContentPage
{
    private Proveedor _proveedor;
    //Tbl_HistorialCompras_Repository _repoHistorialCompras;
    public List<HistorialComprasGroup> _historialCompras { get; private set; } = new List<HistorialComprasGroup>();
    //private Tbl_Proveedor _proveedor;

    public HistorialComprasPage()
	{
		InitializeComponent();
        _proveedor = (Proveedor)BindingContext;
        //_repoHistorialCompras = new Tbl_HistorialCompras_Repository();
    }


    // NAVEGACI�N
    #region NAVEGACI�N
    private async Task HistorialComprasPagePopAsync()
    {
        //await PagePopAsync();
        await Navigation.PopAsync();
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
    #endregion

    // L�GICA PARA EVENTOS
    #region L�GICA PARA EVENTOS
    private async Task CargarDatosCollectionView_HistorialCompras()
    {
        _proveedor = (Proveedor)BindingContext;
        var compras = await ObtenerHistorialProductosDBAsync(_proveedor.PROVEEDOR);
        var gruposCompras = compras.GroupBy(_grs => _grs.DIAFECHA)
            .Select(g => new HistorialComprasGroup(g.Key, g.ToList()));
        _historialCompras.Clear();
        foreach (var grupo in gruposCompras)
        {
            _historialCompras.Add(grupo);
        }
        CollectionView_HistorialCompras.ItemsSource = _historialCompras;
    }
    #endregion

    // L�GICA
    #region L�GICA

    #endregion

    // BASE DE DATOS
    #region BASE DE DATOS
    private async Task<List<HistorialCompras>> ObtenerHistorialProductosDBAsync(string proveedor)
    {
        return await HistorialCompras_Repository.ObtenerHistorialProductosAsync(proveedor);
    }
    #endregion

    // L�GICA DE COSAS VISUALES DE LA P�GINA
    #region L�GICA DE COSAS VISUALES DE LA P�GINA

    #endregion


}