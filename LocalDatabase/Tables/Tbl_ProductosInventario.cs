using SQLite;

namespace TesisAppSINMVVM.Database.Tables;
[Table("TBL_PRODUCTOSINVENTARIO")]
public class Tbl_ProductosInventario
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public string PRODUCTO { get; set; }
    public string MEDIDA { get; set; }
    public int CANTIDAD { get; set; }
    public string DESCRIPCION { get; set; }
    public string FECHAGUARDADO { get; set; }
    public string DIAFECHAGUARDADO { get; set; }
}
