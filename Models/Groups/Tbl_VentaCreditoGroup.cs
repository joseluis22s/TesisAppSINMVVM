using TesisAppSINMVVM.Database.Tables;

namespace TesisAppSINMVVM.Models
{
    public class Tbl_VentaCreditoGroup : List<Tbl_VentaCredito>
    {
        public string NombreGrupo { get; private set; }
        public Tbl_VentaCreditoGroup(string nombreGrupo, List<Tbl_VentaCredito> listaVentaCredito) : base(listaVentaCredito)
        {
            NombreGrupo = nombreGrupo;
        }
    }
}
