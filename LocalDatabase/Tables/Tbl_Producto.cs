using SQLite;

namespace TesisAppSINMVVM.LocalDatabase.Tables;
public class Tbl_Producto
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public string PRODUCTO { get; set; }
    public string MEDIDA { get; set; }
}
