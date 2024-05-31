using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.Database.Tables;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.Contents;

public partial class HistorialComprasContentView : ContentView
{
    Tbl_HistorialCompras_Repository _repoHistorialCompras;
    public static bool _ejecutarBindingContextChanged;

    public List<Tbl_HistorialComprasGroupModel> historialCompras { get; private set; } = new List<Tbl_HistorialComprasGroupModel>();
    public HistorialComprasContentView()
    {
        InitializeComponent();
        _repoHistorialCompras = new Tbl_HistorialCompras_Repository();
    }


    // NAVEGACIÓN


    // EVENTOS
    private void ContentView_Loaded(object sender, EventArgs e)
    {
        CargarHistorialComprasGroup();
    }


    // LOGICA PARA EVENTOS
    private async void CargarHistorialComprasGroup()
    {
        if (_ejecutarBindingContextChanged)
        {
            Tbl_Proveedor proveedorBinding = (Tbl_Proveedor)BindingContext;
            var historial = await ObtenerHistorialProductosDBAsync(nameof(proveedorBinding.NOMBRE), nameof(proveedorBinding.APELLIDO));


            //Crear un grupo para cada fecha
            //foreach (var fecha in fechas)
            //{
            //    var grupo = new Tbl_HistorialComprasGroupModel(fecha, new List<Tbl_HistorialCompras>());

            //    Agregar los elementos del historial que corresponden a esta fecha al grupo
            //    foreach (var compra in historial)
            //    {
            //        if (compra.FECHA == fecha)
            //        {
            //            grupo.Add(compra);
            //        }
            //    }
            //    Agregar el grupo a la lista de grupos
            //    historialCompras.Add(grupo);
            //}
            CollectionView_HistorialCompras.ItemsSource = historialCompras;
            _ejecutarBindingContextChanged = true;
        }
    }
    // agregar 

    // BASE DE DATOS
    private async Task<List<Tbl_HistorialCompras>> ObtenerHistorialProductosDBAsync(string nombreProveedor, string apellidoProveedor)
    {
        return await _repoHistorialCompras.ObtenerHistorialProductosAsync(nombreProveedor, apellidoProveedor);
    }

    


    // LÓGICA DE COSAS VISUALES DE LA PÁGINA
}