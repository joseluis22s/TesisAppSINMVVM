using TesisAppSINMVVM.Database.Tables;

namespace TesisAppSINMVVM.Models
{
    public class VentaCreditoGroup : List<VentaCredito>
    {
        public string NombreGrupo { get; private set; }
        public VentaCreditoGroup(string nombreGrupo, List<VentaCredito> listaVentaCredito) : base(listaVentaCredito)
        {
            NombreGrupo = nombreGrupo;
        }
    }
}
