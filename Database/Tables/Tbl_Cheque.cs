using SQLite;

namespace TesisAppSINMVVM.Database.Tables;

[Table("TBL_CHEQUE")]

public class Tbl_Cheque
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }

    public int NUMERO { get; set; }
    
    public double MONTO { get; set; }

    public string PROVEEDOR {  get; set; }

    public string FECHA { get; set; }
    
    public string DIAFECHA { get; set; }
}
