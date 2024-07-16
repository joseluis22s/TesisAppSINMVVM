using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using TesisAppSINMVVM.FirebaseDataBase.Repositories;
using TesisAppSINMVVM.LocalDatabase.Tables;
using TesisAppSINMVVM.Models;
using TesisAppSINMVVM.Views.ModalViews;

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
    private async void SwipeItem_Editar_Clicked(object sender, EventArgs e)
    {
        await EditarRegsitroChequeEmitido(sender);
    }
    private async void CheckBox_Cheque_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        await CheckBoxChangedActualizarAsync(sender, e);
    }
    private async void SwipeItem_Eliminar_Clicked(object sender, EventArgs e)
    {
        await ELiminarRegsitroChequeEmitido(sender);
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
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                CollectionView_HistorialChequesEmitidosConInternet.IsVisible = true;
                CollectionView_HistorialChequesEmitidosSinInternet.IsVisible = false;
            }
            else
            {
                CollectionView_HistorialChequesEmitidosSinInternet.IsVisible = true;
            }
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
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                CollectionView_HistorialChequesEmitidosConInternet.IsVisible = true;
                CollectionView_HistorialChequesEmitidosSinInternet.IsVisible = false;
                CollectionView_HistorialChequesEmitidosConInternet.ItemsSource = _grupoChequeEmitido.ToList(); ;
            }
            else
            {
                CollectionView_HistorialChequesEmitidosSinInternet.ItemsSource = _grupoChequeEmitido.ToList(); ;
                CollectionView_HistorialChequesEmitidosSinInternet.IsVisible = true;
                CollectionView_HistorialChequesEmitidosConInternet.IsVisible = false;
            }
            //CollectionView_HistorialChequesEmitidos.ItemsSource = _grupoChequeEmitido.ToList();
        }
    }
    private async Task EditarRegsitroChequeEmitido(object sender)
    {
        NetworkAccess accessType = Connectivity.Current.NetworkAccess;
        if (accessType == NetworkAccess.Internet)
        {
            SwipeItem item = sender as SwipeItem;
            Tbl_Cheque cheque = (Tbl_Cheque)item.BindingContext;
            //string msg = "NUMERO-" + cheque.NUMERO +
            //   "\nMONTO-" + cheque.MONTO +
            //   "\nPROVEEDOR-" + cheque.PROVEEDOR +
            //   "\nFECHACOBRO-" + cheque.FECHACOBRO +
            //   "\nFECHAEMISION-" + cheque.FECHAEMISION +
            //   "\nDIAFECHACOBRO-" + cheque.DIAFECHACOBRO +
            //   "\nCOBRADO-" + cheque.COBRADO;
            //await DisplayAlert("MENSAJE",msg, "ok");
            await Navigation.PushAsync(new EditarChequePage
            {
                BindingContext = cheque
            }); ;
        }
        else
        {
            await Toast.Make("Primero debe conectarse a internet", ToastDuration.Long).Show();
        }
    }
    private async Task ELiminarRegsitroChequeEmitido(object sender)
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
            await CargarDatosCollectionView_HistorialChequesEmitidos();
        }
        else
        {
            await Toast.Make("Primero debe conectarse a internet", ToastDuration.Long).Show();
        }
    }
    private async Task CheckBoxChangedActualizarAsync(object sender, CheckedChangedEventArgs e)
    {
        
        NetworkAccess accessType = Connectivity.Current.NetworkAccess;
        if (accessType == NetworkAccess.Internet)
        {
            CheckBox checkBox = (CheckBox)sender;
            Tbl_Cheque tblcheque = (Tbl_Cheque)checkBox.BindingContext;
            await CambiarCobradoRegistroChequeDBAsync(tblcheque.NUMERO, tblcheque.COBRADO);
        }
        else
        {
            await DisplayAlert("SIN CONEXIÓN", "Para registrar esta acción necesita conectarse a internet", "ok"); ;
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
    private async Task EliminarRegistroChequeDBAsync(Cheque cheque)
    {
        await _repoChequeEmitido.EliminarRegistroChequeAsync(cheque);
    }
    private async Task CambiarCobradoRegistroChequeDBAsync(int numeroCheque, bool cobrado)
    {
        await _repoChequeEmitido.CambiarCobradoRegistroChequeAsync(numeroCheque, cobrado);
    }

    #endregion


    // LÓGICA DE COSAS VISUALES DE LA PÁGINA
    #region LÓGICA DE COSAS VISUALES DE LA PÁGINA

    #endregion


}