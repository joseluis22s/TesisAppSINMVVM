using SQLite;

namespace TesisAppSINMVVM.Database.Tables;
public class Tbl_HistorialCompras
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public string PRODUCTO { get; set; }
    public string FECHA { get; set; }
    public string DIA { get; set; }
    public int CANTIDAD { get; set; }
    public double PRECIO {  get; set; }
    public double TOTAL { get; set; }
    public double SALDOPENDIENTE { get; set; }
}
