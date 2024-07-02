using TesisAppSINMVVM.Database.Tables;

namespace TesisAppSINMVVM.Models.Groups
{
    public class HistorialComprasGroup : List<Tbl_HistorialCompras>
    {
        public string NombreGrupo { get; private set; }
        public HistorialComprasGroup(string nombreGrupo, List<Tbl_HistorialCompras> listaHistorialCompras) : base(listaHistorialCompras)
        {
            NombreGrupo = nombreGrupo;
        }
    }
}
