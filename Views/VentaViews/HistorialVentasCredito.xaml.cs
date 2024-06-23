using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.Database.Tables;
using TesisAppSINMVVM.FirebaseDataBase.Repositories;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.Views.VentaViews;

public partial class HistorialVentasCredito : ContentPage
{
    private List<VentaCreditoGroup> _grupoVentaCredito { get; set; } = new List<VentaCreditoGroup>();

	public HistorialVentasCredito()
	{
		InitializeComponent();
    }



    // NAVEGACI�N
    #region NAVEGACI�N
    private async Task HistorialVentasCreditoPagePopAsync()
    {
        await Navigation.PopAsync();
    }
    #endregion


    // EVENTOS
    #region EVENTOS
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
    #endregion


    // L�GICA PARA EVENTOS
    #region L�GICA PARA EVENTOS
    private async Task CargarDatosCollectionView_VentasCredito()
    {
        var ventasCredito = await ObtenerVentasCreditoDBAsync();
        var gruposVentaCredito = ventasCredito.GroupBy(_grs => _grs.DIAFECHAGUARDADO)
            .Select(g => new VentaCreditoGroup(g.Key, g.ToList()));

        _grupoVentaCredito.Clear();
        foreach (var grupo in gruposVentaCredito)
        {
            _grupoVentaCredito.Add(grupo);
        }
        CollectionView_VentasCredito.ItemsSource = _grupoVentaCredito;
    }
    #endregion


    // L�GICA
    #region L�GICA
    private Task PagePopAsync()
    {
        Dispatcher.Dispatch(async () =>
        {
            bool respuesta = await DisplayAlert("Alerta", "�Desea regresar? Perder� el progreso realizado", "Confimar", "Cancelar");
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
    private async Task<List<VentaCredito>> ObtenerVentasCreditoDBAsync()
    {
        return await VentaCredito_Repository.ObtenerVentasCreditoAsync();
    }
    #endregion


    // L�GICA DE COSAS VISUALES DE LA P�GINA
    #region L�GICA DE COSAS VISUALES DE LA P�GINA

    #endregion
}