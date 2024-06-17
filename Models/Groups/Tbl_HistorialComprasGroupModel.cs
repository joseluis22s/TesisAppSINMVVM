using TesisAppSINMVVM.Database.Tables;

namespace TesisAppSINMVVM.Models
{
    public class Tbl_HistorialComprasGroupModel : List<Tbl_HistorialCompras>
    {
        public string NombreGrupo { get; private set; }
        public Tbl_HistorialComprasGroupModel(string nombreGrupo, List<Tbl_HistorialCompras> listaHistorialCompras) : base(listaHistorialCompras)
        {
            NombreGrupo = nombreGrupo;
        }
    }
}
