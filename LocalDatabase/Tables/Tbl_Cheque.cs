using SQLite;

namespace TesisAppSINMVVM.LocalDatabase.Tables;

[Table("TBL_CHEQUE")]

public class Tbl_Cheque
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public int NUMERO { get; set; }
    public double MONTO { get; set; }
    public string PROVEEDOR { get; set; }
    public string FECHACOBRO { get; set; }
    public string FECHAEMISION { get; set; }
    public string DIAFECHACOBRO { get; set; }
}
