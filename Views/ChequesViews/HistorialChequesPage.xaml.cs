using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.Database.Tables;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.Views.ChequesViews;

public partial class HistorialChequesPage : ContentPage
{
    private Tbl_Cheque_Repository _repoCheque;
    private List<ChequeGroup> _grupoCheques { get; set; } = new List<ChequeGroup>();
    private List<Tbl_Cheque> _historialCheques;


    public HistorialChequesPage()
    {
        InitializeComponent();
        _repoCheque = new Tbl_Cheque_Repository();
    }



    // NAVEGACIÓN
    private async Task HistorialChequesPagePopAsync()
    {
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
        await HistorialChequesPagePopAsync();
    }


    // LOGICA PARA EVENTOS


    //LÓGICA
    private async Task CargarDatosCollectionView_HistorialCompras()
    {
        List<Tbl_Cheque> cheques = await ObtenerDBChequesAsync();

        var grupos = cheques
           .GroupBy(c => c.DIAFECHA)
           .Select(g => new ChequeGroup(g.Key, g.ToList()));

        _grupoCheques.Clear();
        foreach (var grupo in grupos)
        {
            _grupoCheques.Add(grupo);
        }
        var a = _grupoCheques;
        CollectionView_HistorialCheques.ItemsSource = _grupoCheques;
        
    }

    // BASE DE DATOS
    private async Task<List<Tbl_Cheque>> ObtenerDBChequesAsync()
    {
        return await _repoCheque.ObtenerChequesAsync();
    }


    // LÓGICA DE COSAS VISUALES DE LA PÁGINA

}