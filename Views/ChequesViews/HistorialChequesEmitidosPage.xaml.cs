using TesisAppSINMVVM.FirebaseDataBase.Repositories;
using TesisAppSINMVVM.LocalDatabase.Tables;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.Views.ChequesViews;

public partial class HistorialChequesEmitidosPage : ContentPage
{
    private Cheque_Repository _repoChequeEmitido = new Cheque_Repository();
    private List<ChequesGroup> _grupoChequeEmitido { get; set; } = new List<ChequesGroup>();    
    public HistorialChequesEmitidosPage()
	{
		InitializeComponent();
	}

    // NAVEGACIÓN
    #region NAVEGACIÓN


    #endregion


    // EVENTOS
    #region EVENTOS
    private async void ContentPage_Appearing(object sender, EventArgs e)
    {
        base.OnAppearing();
        await CargarDatosCollectionView_HistorialChequesEmitidos();
    }
    private void ImageButton_Home_Clicked(object sender, EventArgs e)
    {

    }

    private void Button_Regresar_Clicked(object sender, EventArgs e)
    {

    }
    private void Button_NavegarNuevoResgitroChequeEmitido_Clicked(object sender, EventArgs e)
    {

    }
    private void SwipeItem_Editar_Clicked(object sender, EventArgs e)
    {

    }
    private void CheckBox_Cheque_CheckedChanged(object sender, CheckedChangedEventArgs e)
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