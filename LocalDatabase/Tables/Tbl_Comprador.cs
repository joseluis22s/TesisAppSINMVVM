using SQLite;

namespace TesisAppSINMVVM.LocalDatabase.Tables;

[Table("TBL_COMPRADOR")]
public class Tbl_Comprador
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public string COMPRADOR { get; set; }
}
