using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using TesisAppSINMVVM.FirebaseDataBase.Repositories;
using TesisAppSINMVVM.Models;
using TesisAppSINMVVM.Views.VentaViews.VentaCreditoViews;

namespace TesisAppSINMVVM.Views.VentaViews;

public partial class HistorialVentasCreditoPage : ContentPage
{
    private List<VentaCreditoGroup> _grupoVentaCredito { get; set; } = new List<VentaCreditoGroup>();

	public HistorialVentasCreditoPage()
	{
		InitializeComponent();
    }



    // NAVEGACIÓN
    #region NAVEGACIÓN
    private async Task HistorialVentasCreditoPagePopAsync()
    {
        await Navigation.PopAsync();
    }
    private async Task RegistrarNuevaVentaCreditoPagePushAsync()
    {
        NetworkAccess accessType = Connectivity.Current.NetworkAccess;
        if (accessType == NetworkAccess.Internet)
        {
            await NavegacionRegistrarNuevaVentaCreditoPage();
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
        base.OnAppearing();
        await CargarDatosCollectionView_VentasCredito();
    }
    private async void Button_NavegaRegistrarNuevaVentaCredito_Clicked(object sender, EventArgs e)
    {
        await RegistrarNuevaVentaCreditoPagePushAsync();
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


    // LÓGICA PARA EVENTOS
    #region LÓGICA PARA EVENTOS
    private async Task CargarDatosCollectionView_VentasCredito()
    {
        var ventasCredito = await ObtenerVentasCreditoDBAsync();
        if (ventasCredito.Count == 0)
        {
            VerticalStackLayout_EmptyView_HistorialCheques.IsVisible = true;
            CollectionView_VentasCredito.IsVisible = false;
        }
        else
        {
            VerticalStackLayout_EmptyView_HistorialCheques.IsVisible = false;
            var gruposVentaCredito = ventasCredito.OrderByDescending(vc => DateTime.Parse(vc.FECHAGUARDADO))
            .GroupBy(_grs => _grs.DIAFECHAGUARDADO)
            .Select(g => new VentaCreditoGroup(g.Key, g.ToList()));

            _grupoVentaCredito.Clear();
            foreach (var grupo in gruposVentaCredito)
            {
                _grupoVentaCredito.Add(grupo);
            }
            CollectionView_VentasCredito.ItemsSource = _grupoVentaCredito;
        }
        //var gruposVentaCredito = ventasCredito.OrderByDescending(vc => DateTime.Parse(vc.FECHAGUARDADO))
        //    .GroupBy(_grs => _grs.DIAFECHAGUARDADO)
        //    .Select(g => new VentaCreditoGroup(g.Key, g.ToList()));

        //_grupoVentaCredito.Clear();
        //foreach (var grupo in gruposVentaCredito)
        //{
        //    _grupoVentaCredito.Add(grupo);
        //}
        //CollectionView_VentasCredito.ItemsSource = _grupoVentaCredito;
    }
    #endregion


    // LÓGICA
    #region LÓGICA
    private async Task NavegacionRegistrarNuevaVentaCreditoPage()
    {
        // TODO: Ver si esta bien esta lógica
        var stack = Navigation.NavigationStack.ToArray();
        var lastPage = stack[stack.Length - 2];
        if (lastPage is RegistrarNuevaVentaCreditoPage)
        {
            await Navigation.PopAsync();
        }
        else if (lastPage is OpcionesVentaCreditoPage)
        {
            await Navigation.PushAsync(new RegistrarNuevaVentaCreditoPage
            {
                BindingContext = this.BindingContext
            });
            Navigation.RemovePage(stack[stack.Length - 1]);
        }
    }
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
    #endregion


    // BASE DE DATOS
    #region BASE DE DATOS
    private async Task<List<VentaCredito>> ObtenerVentasCreditoDBAsync()
    {
        return await VentaCredito_Repository.ObtenerVentasCreditoAsync();
    }
    #endregion


    // LÓGICA DE COSAS VISUALES DE LA PÁGINA
    #region LÓGICA DE COSAS VISUALES DE LA PÁGINA

    #endregion

    
}