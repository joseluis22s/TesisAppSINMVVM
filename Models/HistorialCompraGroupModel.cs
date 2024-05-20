
namespace TesisAppSINMVVM.Models
{
    public class HistorialCompraGroupModel : List<HistorialCompraModel>
    {
        public string NombreGrupo { get; private set; }
        public HistorialCompraGroupModel(string nombreGrupo, List<HistorialCompraModel> listahistorialCompras)  : base(listahistorialCompras)
        {
            NombreGrupo = nombreGrupo;
        }
    }
}
