using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.Database.Tables;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.Views.VentaViews;

public partial class HistorialVentasCredito : ContentPage
{
    private Tbl_VentaCredito_Repository _repoVentaCredito;
    private List<Tbl_VentaCreditoGroup> _grupoVentaCredito { get; set; } = new List<Tbl_VentaCreditoGroup>();

	public HistorialVentasCredito()
	{
		InitializeComponent();
        _repoVentaCredito = new Tbl_VentaCredito_Repository();

    }





    // NAVEGACIÓN
    private async Task HistorialVentasCreditoPagePopAsync()
    {
        await PagePopAsync();
    }


    // EVENTOS
    private async void ContentPage_Appearing(object sender, EventArgs e)
    {
        base.OnAppearing();
        await CargarDatosCollectionView_VentasCredito();
    }
    protected override bool OnBackButtonPressed()
    {
        HistorialVentasCreditoPagePopAsync().GetAwaiter();
        return true;
    }
    private async void Button_Regresar_Clicked(object sender, EventArgs e)
    {
        await HistorialVentasCreditoPagePopAsync();
    }


    // LOGICA PARA EVENTOS
    private async Task CargarDatosCollectionView_VentasCredito()
    {
        var ventasCredito = await ObtenerVentasCreditoDBAsync();
        var gruposVentaCredito = ventasCredito.GroupBy(_grs => _grs.DIAFECHAGUARDADO)
            .Select(g => new Tbl_VentaCreditoGroup(g.Key, g.ToList()));
        
        _grupoVentaCredito.Clear();
        foreach (var grupo in gruposVentaCredito)
        {
            _grupoVentaCredito.Add(grupo);
        }
        CollectionView_VentasCredito.ItemsSource = _grupoVentaCredito;
    }


    // LÓGICA
    private Task PagePopAsync()
    {
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
    private async Task<List<Tbl_VentaCredito>> ObtenerVentasCreditoDBAsync()
    {
        return await _repoVentaCredito.ObtenerVentasCreditoAsync();
    }

    


    // LÓGICA DE COSAS VISUALES DE LA PÁGINA
}