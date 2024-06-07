using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.Database.Tables;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.Views.CompraViews;

public partial class HistorialComprasPage : ContentPage
{

    Tbl_HistorialCompras_Repository _repoHistorialCompras;
    public List<Tbl_HistorialComprasGroupModel> _historialCompras { get; private set; } = new List<Tbl_HistorialComprasGroupModel>();
    private Tbl_Proveedor _proveedor;

    public HistorialComprasPage()
	{
		InitializeComponent();
        _repoHistorialCompras = new Tbl_HistorialCompras_Repository();
    }


    // NAVEGACIÓN
    private async Task HistorialComprasPagePopAsync()
    {
        //await PagePopAsync();
        await Navigation.PopAsync();
    }


    // EVENTOS
    private async void ContentPage_Appearing(object sender, EventArgs e)
    {
        base.OnAppearing();
        await CargarDatosCollectionView_HistorialCompras();
    }
    private async void Button_Regresar_Clicked(object sender, EventArgs e)
    {
        //if (_enEjecucion)
        //{
        //    return;
        //}
        //_enEjecucion = true;
        await HistorialComprasPagePopAsync();
        //_enEjecucion = false;
    }
    //protected override bool OnBackButtonPressed()
    //{
    //    HistorialComprasPagePopAsync().GetAwaiter();
    //    return true;
    //}



    // LOGICA PARA EVENTOS
    private async Task CargarDatosCollectionView_HistorialCompras()
    {
        _proveedor = (Tbl_Proveedor)BindingContext;
        var compras = await ObtenerHistorialProductosDBAsync(_proveedor.NOMBRE, _proveedor.APELLIDO);
        var gruposCompras = compras.GroupBy(_grs => _grs.DIAFECHA)
            .Select(g => new Tbl_HistorialComprasGroupModel(g.Key, g.ToList()));
        _historialCompras.Clear();
        foreach (var grupo in gruposCompras)
        {
            _historialCompras.Add(grupo);
        }
        CollectionView_HistorialCompras.ItemsSource = _historialCompras;
    }

    // LÓGICA
    //private Task PagePopAsync()
    //{
    //    Dispatcher.Dispatch(async () =>
    //    {
    //        bool respuesta = await DisplayAlert("Alerta", "¿Desea regresar? Perderá el progreso realizado", "Confimar", "Cancelar");
    //        if (respuesta)
    //        {
    //            await Navigation.PopAsync();
    //        }
    //    });
    //    return Task.CompletedTask;
    //}


    // BASE DE DATOS
    private async Task<List<Tbl_HistorialCompras>> ObtenerHistorialProductosDBAsync(string nombreProveedor, string apellidoProveedor)
    {
        return await _repoHistorialCompras.ObtenerHistorialProductosAsync(nombreProveedor, apellidoProveedor);
    }

    


    // LÓGICA DE COSAS VISUALES DE LA PÁGINA

}