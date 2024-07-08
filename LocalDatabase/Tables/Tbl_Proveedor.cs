using SQLite;

namespace TesisAppSINMVVM.LocalDatabase.Tables;

[Table("TBL_PROVEEDOR")]
public class Tbl_Proveedor
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public string PROVEEDOR { get; set; }
}