using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.Database.Tables;
using TesisAppSINMVVM.FirebaseDataBase.Repositories;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.Views.VentaViews;

public partial class HistorialProductoSobrantePage : ContentPage
{
    private List<ProductoInventarioBodegaGroup> _grupoInventario {  get; set; } = new List<ProductoInventarioBodegaGroup>();
    public HistorialProductoSobrantePage()
	{
		InitializeComponent();
    }



    // NAVEGACIÓN
    #region NAVEGACIÓN
    private async Task HistorialProductoSobrantePagePopAsync()
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
        await CargarDatosCollectionView_Bodega();
    }
    private async void Button_Regresar_Clicked(object sender, EventArgs e)
    {
        RegistrarProductoSobranteBodegaPage._ejecutarAppearing = false;
        await HistorialProductoSobrantePagePopAsync();
    }
    protected override bool OnBackButtonPressed()
    {
        RegistrarProductoSobranteBodegaPage._ejecutarAppearing = false;
        HistorialProductoSobrantePagePopAsync().GetAwaiter();
        return true;
    }
    #endregion

    // LÓGICA PARA EVENTOS
    #region LÓGICA PARA EVENTOS
    private async Task CargarDatosCollectionView_Bodega()
    {
        List<ProductoInventarioBodega> inventario = await ObtenerInvetarioDBAsync();
        var gruposInvetarioProductos = inventario.GroupBy(_grs => _grs.DIAFECHAGUARDADO)
            .Select(g => new ProductoInventarioBodegaGroup(g.Key, g.ToList()));

        _grupoInventario.Clear();
        foreach (var grupo in gruposInvetarioProductos)
        {
            _grupoInventario.Add(grupo);
        }
        CollectionView_Bodega.ItemsSource = _grupoInventario;
    }
    #endregion

    // LÓGICA
    #region LÓGICA
    private Task PagePopAsync()
    {
        RegistrarProductoSobranteBodegaPage._ejecutarAppearing = false;
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
    #endregion

    // BASE DE DATOS
    #region BASE DE DATOS
    private async Task<List<ProductoInventarioBodega>> ObtenerInvetarioDBAsync()
    {
        return await ProductoInventarioBodega_Repository.ObtenerProductosInventarioAsync();
    }
    #endregion

    // LÓGICA DE COSAS VISUALES DE LA PÁGINA
    #region LÓGICA DE COSAS VISUALES DE LA PÁGINA

    #endregion
}