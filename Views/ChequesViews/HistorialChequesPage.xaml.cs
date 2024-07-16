using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Collections.ObjectModel;
using TesisAppSINMVVM.FirebaseDataBase.Repositories;
using TesisAppSINMVVM.LocalDatabase.Tables;
using TesisAppSINMVVM.Models;
using TesisAppSINMVVM.Views.ModalViews;

namespace TesisAppSINMVVM.Views.ChequesViews;

public partial class HistorialChequesPage : ContentPage
{
    private Cheque_Repository _repoCheque = new Cheque_Repository();
    //private ObservableCollection<ChequesGroup> _grupoCheques { get; set; } = new ObservableCollection<ChequesGroup>();
    private List<ChequesGroup> _grupoCheques { get; set; } = new List<ChequesGroup>();
    private bool _enEjecucion;


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
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await HistorialChequesPagePopAsync();
        _enEjecucion = false;
    }
    private async void Button_NavegarAgregarNuevoRegistroCheque_Clicked(object sender, EventArgs e)
    {
        if (_enEjecucion)
        {
            return;
        }
        _enEjecucion = true;
        await RegistrarChequePagePushAsync();
        _enEjecucion = false;
    }
    private async void ImageButton_Home_Clicked(object sender, EventArgs e)
    {
        await NavegarPaginaPrincipalPagePopToRootAsync();
    }
    private async void SwipeItem_Editar_Clicked(object sender, EventArgs e)
    {
        NetworkAccess accessType = Connectivity.Current.NetworkAccess;
        if (accessType == NetworkAccess.Internet)
        {
            SwipeItem item = sender as SwipeItem;
            Tbl_Cheque cheque = (Tbl_Cheque)item.BindingContext;
            await Navigation.PushModalAsync(new EditarChequePage
            {
                BindingContext = /*this.BindingContext*/cheque
            });
        }
        else
        {
            await Toast.Make("Primero debe conectarse a internet", ToastDuration.Long).Show();
        }

    }
    private async void SwipeItem_Eliminar_Clicked(object sender, EventArgs e)
    {
        NetworkAccess accessType = Connectivity.Current.NetworkAccess;
        if (accessType == NetworkAccess.Internet)
        {
            SwipeItem item = sender as SwipeItem;
            Tbl_Cheque tblcheque = (Tbl_Cheque)item.BindingContext;
            Cheque cheque = new Cheque();
            cheque.NUMERO = tblcheque.NUMERO;
            cheque.MONTO = tblcheque.MONTO;
            cheque.PROVEEDOR = tblcheque.PROVEEDOR;
            cheque.FECHACOBRO = tblcheque.FECHACOBRO;
            cheque.FECHAEMISION = tblcheque.FECHAEMISION;
            cheque.DIAFECHACOBRO = tblcheque.DIAFECHACOBRO;

            await EliminarRegistroChequeDBAsync(cheque);
            await CargarDatosCollectionView_HistorialCompras();
        }
        else
        {
            await Toast.Make("Primero debe conectarse a internet", ToastDuration.Long).Show();
        }
    }
    private async void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        await CheckBoxChangedActualizarAsync(sender, e);
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
                CollectionView_HistorialCheques.ItemsSource = _grupoCheques.ToList();

        }

    }
    private async Task NavegarPaginaPrincipalPagePopToRootAsync()
    {
        await Navigation.PopToRootAsync();
    }
    private async Task CheckBoxChangedActualizarAsync(object sender, CheckedChangedEventArgs e)
    {
        CheckBox checkBox = (CheckBox)sender;
        Tbl_Cheque tblcheque = (Tbl_Cheque)checkBox.BindingContext;
        await CambiarCobradoRegistroChequeDBAsync(tblcheque.NUMERO, tblcheque.COBRADO);

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
    private async Task EliminarRegistroChequeDBAsync(Cheque cheque)
    {
        await _repoCheque.EliminarRegistroChequeAsync(cheque);
    }
    private async Task CambiarCobradoRegistroChequeDBAsync(int numeroCheque, bool cobrado)
    {
        await _repoCheque.CambiarCobradoRegistroChequeAsync(numeroCheque, cobrado);
    }

    #endregion

    // LÓGICA DE COSAS VISUALES DE LA PÁGINA
    #region LÓGICA DE COSAS VISUALES DE LA PÁGINA

    #endregion


}