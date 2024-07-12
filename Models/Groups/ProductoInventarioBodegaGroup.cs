namespace TesisAppSINMVVM.Models
{
    public class ProductoInventarioBodegaGroup : List<ProductoInventarioBodega>
    {
        public string DiaFecha {  get; private set; }
        public ProductoInventarioBodegaGroup(string diaFecha, List<ProductoInventarioBodega> inventarioProductos) : base (inventarioProductos)
        {
            DiaFecha = diaFecha;
        }
    }
}
