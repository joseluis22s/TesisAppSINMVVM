using SQLite;

namespace TesisAppSINMVVM.Database.Tables;
public class Tbl_Producto
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public string PRODUCTO { get; set; }
}
