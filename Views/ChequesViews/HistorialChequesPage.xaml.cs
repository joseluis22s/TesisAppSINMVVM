using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using TesisAppSINMVVM.FirebaseDataBase.Repositories;
using TesisAppSINMVVM.LocalDatabase.Tables;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.Views.ChequesViews;

public partial class HistorialChequesPage : ContentPage
{
    private Cheque_Repository _repoCheque = new Cheque_Repository();
    
    private List<ChequesGroup> _grupoCheques { get; set; } = new List<ChequesGroup>();
    private List<Cheque> _historialCheques;


    public HistorialChequesPage()
    {
        InitializeComponent();
    }

    // NAVEGACIÓN
    #region NAVEGACIÓN
    private async Task HistorialChequesPagePopAsync()
    {
        await Navigation.PopAsync();
    }
    private async Task RegistrarChequePagePushAsync()
    {
        NetworkAccess accessType = Connectivity.Current.NetworkAccess;
        if (accessType == NetworkAccess.Internet)
        {
            await NavegacionRegistrarChequePage();
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
        await CargarDatosCollectionView_HistorialCompras();
    }
    private async void Button_Regresar_Clicked(object sender, EventArgs e)
    {
        await HistorialChequesPagePopAsync();
    }
    private async void Button_NavegarAgregarNuevoRegistroCheque_Clicked(object sender, EventArgs e)
    {
        await RegistrarChequePagePushAsync();
    }
    #endregion

    // LÓGICA PARA EVENTOS
    #region LÓGICA PARA EVENTOS
    private async Task CargarDatosCollectionView_HistorialCompras()
    {
        List<Tbl_Cheque> cheques = await ObtenerDBChequesAsync();
        if (cheques.Count == 0)
        {
            VerticalStackLayout_EmptyView_HistorialCheques.IsVisible = true;
            CollectionView_HistorialCheques.IsVisible = false;
        }
        else
        {
            VerticalStackLayout_EmptyView_HistorialCheques.IsVisible = false;
            var gruposFechaCobro = cheques.OrderByDescending(c => DateTime.Parse(c.FECHACOBRO))
            .GroupBy(c => c.DIAFECHACOBRO)
           .Select(g => new ChequesGroup(g.Key, g.ToList()));
            _grupoCheques.Clear();
            foreach (var grupo in gruposFechaCobro)
            {
                _grupoCheques.Add(grupo);
            }
            var a = _grupoCheques;
            CollectionView_HistorialCheques.ItemsSource = _grupoCheques;
        }

    }

    #endregion

    // LÓGICA
    #region LÓGICA
    private async Task NavegacionRegistrarChequePage()
    {
        // TODO: Ver si esta bien esta lógica
        var stack = Navigation.NavigationStack.ToArray();
        var lastPage = stack[stack.Length - 2];
        if (lastPage is RegistrarChequePage)
        {
            await Navigation.PopAsync();
        }
        else if (lastPage is ChequesPage)
        {
            await Navigation.PushAsync(new RegistrarChequePage
            {
                BindingContext = this.BindingContext
            });
            Navigation.RemovePage(stack[stack.Length - 1]);
        }
    }
    #endregion

    #region BASE DE DATOS
    // BASE DE DATOS
    private async Task<List<Tbl_Cheque>> ObtenerDBChequesAsync()
    {
        return await _repoCheque.ObtenerChequesAsync();
    }
    #endregion

    // BASE DE DATOS
    #region BASE DE DATOS

    #endregion

    // LÓGICA DE COSAS VISUALES DE LA PÁGINA
    #region LÓGICA DE COSAS VISUALES DE LA PÁGINA

    #endregion

    
}