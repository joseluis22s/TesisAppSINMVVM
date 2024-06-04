using SQLite;

namespace TesisAppSINMVVM.Database.Tables;

[Table("TBL_COMPRADOR")]
public class Tbl_Comprador
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }

    public string NOMBRE { get; set; }

    public string APELLIDO { get; set; }

    public string TELEFONO { get; set; }
}
