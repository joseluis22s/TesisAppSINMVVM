using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using TesisAppSINMVVM.FirebaseDataBase.Repositories;
using TesisAppSINMVVM.LocalDatabase.Tables;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.Views.ChequesViews;

public partial class HistorialChequesEmitidosPage : ContentPage
{
    private Cheque_Repository _repoChequeEmitido = new Cheque_Repository();
    private List<ChequesGroup> _grupoChequeEmitido { get; set; } = new List<ChequesGroup>();
    private bool _enEjecucion;
    public HistorialChequesEmitidosPage()
	{
		InitializeComponent();
	}

    // NAVEGACIÓN
    #region NAVEGACIÓN
    private async Task HistorialChequesEmitidosPagePopAsync()
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
    private async Task NavegarPaginaPrincipalPagePopToRootAsync()
    {
        await Navigation.PopToRootAsync();
    }
    #endregion


    // EVENTOS
    #region EVENTOS
    private async void ContentPage_Appearing(object sender, EventArgs e)
    {
        base.OnAppearing();
        await CargarDatosCollectionView_HistorialChequesEmitidos();
    }
    private async void ImageButton_Home_Clicked(object sender, EventArgs e)
    {
        await NavegarPaginaPrincipalPagePopToRootAsync();
    }
    private async void Button_Regresar_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await HistorialChequesEmitidosPagePopAsync();
        _enEjecucion = false;
    }
    private async void Button_NavegarNuevoResgitroChequeEmitido_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await RegistrarChequePagePushAsync();
        _enEjecucion = false;
    }
    private void SwipeItem_Editar_Clicked(object sender, EventArgs e)
    {

    }
    private void CheckBox_Cheque_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {

    }
    private void SwipeItem_Eliminar_Clicked(object sender, EventArgs e)
    {

    }
    #endregion


    // LÓGICA PARA EVENTOS
    #region LÓGICA PARA EVENTOS
    private async Task CargarDatosCollectionView_HistorialChequesEmitidos()
    {
        List<Tbl_Cheque> chequesEmitidos = await ObtenerChequesDBAsync();
        if (chequesEmitidos.Count == 0)
        {
            VerticalStackLayout_EmptyView.IsVisible = true;
            CollectionView_HistorialChequesEmitidos.IsVisible = false;
        }
        else
        {
            VerticalStackLayout_EmptyView.IsVisible = false;
            var gruposFechaCobro = chequesEmitidos.OrderByDescending(c => DateTime.Parse(c.FECHACOBRO))
                .GroupBy(c => c.DIAFECHACOBRO)
                .Select(g => new ChequesGroup(
                    g.Key,
                    g.OrderByDescending(c => c.NUMERO).ToList()
                 ));
            _grupoChequeEmitido.Clear();
            foreach (var grupo in gruposFechaCobro)
            {
                _grupoChequeEmitido.Add(grupo);
            }
            CollectionView_HistorialChequesEmitidos.ItemsSource = _grupoChequeEmitido.ToList();
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


    // BASE DE DATOS
    #region BASE DE DATOS
    private async Task<List<Tbl_Cheque>> ObtenerChequesDBAsync()
    {
        return await _repoChequeEmitido.ObtenerChequesAsync();
    }






    #endregion


    // LÓGICA DE COSAS VISUALES DE LA PÁGINA
    #region LÓGICA DE COSAS VISUALES DE LA PÁGINA

    #endregion

    
}