using SQLite;

namespace TesisAppSINMVVM.LocalDatabase.Tables;
[Table("TBL_PRODUCTOCOMPRADO")]
public class Tbl_ProductoComprado
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public int NUMEROCOMPRA { get; set; }
    public string DIAFECHA { get; set; }
    public string PRODUCTO { get; set; }
    public string MEDIDA { get; set; }
    public int CANTIDAD { get; set; }
    public double PRECIO { get; set; }
    public double TOTAL { get; set; }
    public string FECHEGUARDADO { get; set; }
    public string DIAFECHAGUARDADO { get; set; }
    public string PROVEEDOR { get; set; }
}
