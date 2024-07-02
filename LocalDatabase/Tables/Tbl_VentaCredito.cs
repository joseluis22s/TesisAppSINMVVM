using SQLite;

namespace TesisAppSINMVVM.Database.Tables;

[Table("TBL_VENTACREDITO")]

public class Tbl_VentaCredito
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public string COMPRADOR { get; set; }
    public double MONTOVENDIDO { get; set; }
    public string DESCRIPCION { get; set; }
    public string FECHAGUARDADO { get; set; }
    public string DIAFECHAGUARDADO { get; set; }
}
