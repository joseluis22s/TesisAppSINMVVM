namespace TesisAppSINMVVM.Models
{
    public class ProductoComprado
    {
        // TODO: Hacer la sincronización con firebase y local
        public int NUMEROCOMPRA {  get; set; }
        public string PRODUCTO { get; set; }
        public string MEDIDA { get; set; }
        public int CANTIDAD { get; set; }
        public double PRECIO { get; set; }
        public double TOTAL { get; set; }
        public string FECHAGUARDADO { get; set; }
        public string DIAFECHAGUARDADO { get; set; }
        public string PROVEEDOR { get; set; }
    }
}
