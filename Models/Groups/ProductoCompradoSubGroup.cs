using TesisAppSINMVVM.LocalDatabase.Tables;

namespace TesisAppSINMVVM.Models.Groups
{
    public class ProductoCompradoSubGroup : List<Tbl_ProductoComprado>
    {
        public int NumeroCompra { get; private set; }

        public ProductoCompradoSubGroup(int numeroCompra, List<Tbl_ProductoComprado> productos) : base(productos)
        {
            NumeroCompra = numeroCompra;
        }
    }
}
