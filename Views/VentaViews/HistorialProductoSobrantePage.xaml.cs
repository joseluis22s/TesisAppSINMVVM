using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.Database.Tables;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.Views.VentaViews;

public partial class HistorialProductoSobrantePage : ContentPage
{
	private Tbl_ProductosInventario_Repository _repoInventario; 
    private List<Tbl_ProductoInventarioGroup> _grupoInventario {  get; set; } = new List<Tbl_ProductoInventarioGroup>();
    public HistorialProductoSobrantePage()
	{
		InitializeComponent();
        _repoInventario = new Tbl_ProductosInventario_Repository();

    }



    // NAVEGACIÓN
    private async Task HistorialProductoSobrantePagePopAsync()
    {
        //await PagePopAsync();
        await Navigation.PopAsync();
    }


    // EVENTOS
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


    // LOGICA PARA EVENTOS
    private async Task CargarDatosCollectionView_Bodega()
    {
        List<Tbl_ProductosInventario> inventario = await ObtenerInvetarioDBAsync();
        var grupos = inventario.GroupBy(_grs => _grs.DIAFECHAGUARDADO)
            .Select(g => new Tbl_ProductoInventarioGroup(g.Key, g.ToList()));

        _grupoInventario.Clear();
        foreach (var grupo in grupos)
        {
            _grupoInventario.Add(grupo);
        }
        CollectionView_Bodega.ItemsSource = _grupoInventario;
    }
    

    // LÓGICA
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



    // BASE DE DATOS
    private async Task<List<Tbl_ProductosInventario>> ObtenerInvetarioDBAsync()
    {
        return await _repoInventario.ObtenerInvetarioAsync();
    }

    


    // LÓGICA DE COSAS VISUALES DE LA PÁGINA
}