
using SQLite;
using TesisAppSINMVVM.Database.Tables;

namespace TesisAppSINMVVM.Database
{
    public class Repositorio
    {
        SQLiteAsyncConnection conn;
        public Repositorio() { }
        async Task InitAsync()
        {
            if (conn is not null)
                return;

            conn = new SQLiteAsyncConnection(Constantes.DatabasePath, Constantes.Flags);
            var resultado = await conn.CreateTableAsync<Tbl_Usuario>();
        }
        // TBL_USUARIO
        public async Task<bool> VerificarExistenciaUsuarioAsync(string nombreUsuario)
        {
            await InitAsync();
            int resultado = await conn.Table<Tbl_Usuario>().CountAsync(usuario => usuario.USUARIO == nombreUsuario);
            if (resultado != 0)
            {
                return true;
            }
            return false;
        }
        public async Task GuardarNuevoUsuarioAsync(string usuario, string contrasena, string correo)
        {
            await InitAsync();
            Tbl_Usuario Usuario = new()
            {
                USUARIO = usuario,
                CONTRASENA = contrasena,
                CORREO = correo,
            };
            await conn.InsertAsync(Usuario);
        }

        public async Task<bool> VerificarContrasenaCorrectaAsync(string nombreUsuario, string contrasena)
        {
            await InitAsync();
            string query = "SELECT CONTRASENA FROM TBL_USUARIO WHERE USUARIO = ?";
            var a = await conn.FindWithQueryAsync<Tbl_Usuario>(query, nombreUsuario);
            if (a != null)
            {
                return a.CONTRASENA.Equals(contrasena);
            }
            return false;
        }
        //Borrar usuarios 'Tbl_Usuario'
        public async Task<int> BorrarUsuarios()
        {
            await InitAsync();
            return await conn.DeleteAllAsync<Tbl_Usuario>();
        }
        //Borrar tabla 'Tbl_Usuario'
        public async Task<int> BorrarTablaUsuario()
        {
            await InitAsync();
            return await conn.DropTableAsync<Tbl_Usuario>();
        }

    }
}

