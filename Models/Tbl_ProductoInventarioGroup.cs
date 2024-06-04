using TesisAppSINMVVM.Database.Tables;

namespace TesisAppSINMVVM.Models
{
    public class Tbl_ProductoInventarioGroup : List<Tbl_ProductosInventario>
    {
        public string DiaFecha {  get; private set; }
        public Tbl_ProductoInventarioGroup(string diaFecha, List<Tbl_ProductosInventario> inventario) : base (inventario)
        {
            DiaFecha = diaFecha;
        }
    }
}
