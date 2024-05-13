using SQLite;

namespace TesisAppSINMVVM.Database.Tables;

[Table("TBL_PROVEEDOR")]
public class Tbl_Proveedor
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public string NOMBRE { get; set; }
    public string APELLIDO {  get; set; }
    public string TELEFONO {  get; set; }
}