
using SQLite;
using TesisAppSINMVVM.Database.Tables;

namespace TesisAppSINMVVM.Database
{
    public class Repositorio
    {
        SQLiteAsyncConnection conn;
        public Repositorio()
        {

        }
        async Task InitAsync()
        {
            if (conn is not null)
                return;

            conn = new SQLiteAsyncConnection(Constantes.DatabasePath, Constantes.Flags);
            var resultado = await conn.CreateTableAsync<Tbl_Usuario>();
        }

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

    }
}
