using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.Database.Tables;
using TesisAppSINMVVM.FirebaseDataBase.Repositories;
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

    // NAVEGACI�N
    #region NAVEGACI�N
    private async Task HistorialChequesPagePopAsync()
    {
        await Navigation.PopAsync();
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
    #endregion

    // L�GICA PARA EVENTOS
    #region L�GICA PARA EVENTOS
    private async Task CargarDatosCollectionView_HistorialCompras()
    {
        List<Tbl_Cheque> cheques = await ObtenerDBChequesAsync();

        var gruposFechaCobro = cheques.OrderByDescending(c => DateTime.Parse(c.FECHACOBRO))
            .GroupBy(c => c.DIAFECHACOBRO)
           .Select(g => new ChequesGroup(g.Key, g.ToList()));
        //TODO: HACER QUE SEA POR FECHA DE COBRO (NUEVA IMPLMENTACI�N)
        _grupoCheques.Clear();
        foreach (var grupo in gruposFechaCobro)
        {
            _grupoCheques.Add(grupo);
        }
        var a = _grupoCheques;
        CollectionView_HistorialCheques.ItemsSource = _grupoCheques;

    }
    #endregion

    // L�GICA
    #region L�GICA


    // BASE DE DATOS
    private async Task<List<Tbl_Cheque>> ObtenerDBChequesAsync()
    {
        return await _repoCheque.ObtenerChequesAsync();
    }
    #endregion

    // BASE DE DATOS
    #region BASE DE DATOS

    #endregion

    // L�GICA DE COSAS VISUALES DE LA P�GINA
    #region L�GICA DE COSAS VISUALES DE LA P�GINA

    #endregion
}