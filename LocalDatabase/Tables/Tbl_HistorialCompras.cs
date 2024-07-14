using SQLite;

namespace TesisAppSINMVVM.LocalDatabase.Tables;
public class Tbl_HistorialCompras
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public string DIAFECHA { get; set; }
    public string PROVEEDOR { get; set; }
    public string PRODUCTO { get; set; }
    public string MEDIDA { get; set; }
    public string FECHA { get; set; }
    public int CANTIDAD { get; set; }
    public double PRECIO { get; set; }
    public double TOTAL { get; set; }
    //public double ABONO { get; set; }
    //public double SALDOPENDIENTE { get; set; }
}
