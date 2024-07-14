using SQLite;
using TesisAppSINMVVM.LocalDatabase.Tables;

namespace TesisAppSINMVVM.Database.Respositories
{
    public class Tbl_Usuario_Repository
    {
        SQLiteAsyncConnection conn;
        public Tbl_Usuario_Repository() { }
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

            bool existeNombreUsuario = await VerificarExistenciaNombreUsuarioAsync(usuario);
            if (!existeNombreUsuario)
            {
                await conn.InsertAsync(Usuario);
            }
        }
        public async Task<List<Tbl_Usuario>> ObtenerUsuariosAsync()
        {
            await InitAsync();
            return await conn.Table<Tbl_Usuario>().ToListAsync();
        }
        public async Task<bool> VerificarExistenciaNombreUsuarioAsync(string usuario)
        {
            await InitAsync();
            int resultado = await conn.Table<Tbl_Usuario>().Where(x =>
                x.USUARIO == usuario).CountAsync();
            if (resultado == 0)
            {
                return false;
            }
            return true;
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
        public async Task<int> BorrarTblUsuarioAsync()
        {
            await InitAsync();
            return await conn.DropTableAsync<Tbl_Usuario>();
        }

    }
}
