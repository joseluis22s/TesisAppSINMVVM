using SQLite;

namespace TesisAppSINMVVM.Database.Tables;

[Table("TBL_USUARIO")]
public class Tbl_Usuario
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }

    public string USUARIO { get; set; }

    public string CONTRASENA { get; set; }

    public string CORREO { get; set; }

}
