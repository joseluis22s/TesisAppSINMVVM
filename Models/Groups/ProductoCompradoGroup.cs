using TesisAppSINMVVM.LocalDatabase.Tables;

namespace TesisAppSINMVVM.Models.Groups
{
    public class ProductoCompradoGroup : List<Tbl_ProductoComprado>
    {
        public string DiaFecha { get; private set; }
        public List<ProductoCompradoSubGroup> SubGrupos { get; set; }

        public ProductoCompradoGroup(string diaFecha, List<Tbl_ProductoComprado> prodcutoComprado) : base(prodcutoComprado)
        {
            DiaFecha = diaFecha;
        }
    }
}
