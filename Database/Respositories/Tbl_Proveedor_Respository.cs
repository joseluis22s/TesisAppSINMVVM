using SQLite;
using TesisAppSINMVVM.Database.Tables;

namespace TesisAppSINMVVM.Database.Respositories
{
    public class Tbl_Proveedor_Respository
    {
        SQLiteAsyncConnection conn;
        public Tbl_Proveedor_Respository() { }
        async Task InitAsync()
        {
            if (conn is not null)
                return;

            conn = new SQLiteAsyncConnection(Constantes.DatabasePath, Constantes.Flags);
            var resultado = await conn.CreateTableAsync<Tbl_Proveedor>();
        }
        public async Task GuardarNuevoProveedorAsync(string nombre, string apellido, string telefono)
        {
            await InitAsync();
            Tbl_Proveedor Proveedor = new()
            {
                NOMBRE = nombre,
                APELLIDO = apellido,
                TELEFONO = telefono
            };
            await conn.InsertAsync(Proveedor);
        }

        public async Task<bool> VerificarExistenciaProveedorAsync(string nombre, string apellido)
        {
            await InitAsync();
            int resultado = await conn.Table<Tbl_Proveedor>().Where(x =>
                x.NOMBRE == nombre &&
                x.APELLIDO == apellido
                ).CountAsync();
            if (resultado == 0)
            {
                return false;
            }
            return true;
        }

        public async Task<List<Tbl_Proveedor>> ObtenerProveedores()
        {
            await InitAsync();
            return await conn.Table<Tbl_Proveedor>().ToListAsync();
        }
        public async Task BorrarTblProveedorAsync()
        {
            await InitAsync();
            await conn.DropTableAsync<Tbl_Proveedor>();
        }
        public async Task GuardarTodoNuevoProveedorAsync(List<Tbl_Proveedor> t)
        {
            await InitAsync();
            await conn.InsertAllAsync(t);
        }

        ////Borrar usuarios 'Tbl_Usuario'
        //public async Task<int> BorrarUsuarios()
        //{
        //    await InitAsync();
        //    return await conn.DeleteAllAsync<Tbl_Usuario>();
        //}
        ////Borrar tabla 'Tbl_Usuario'
        //public async Task<int> BorrarTablaUsuario()
        //{
        //    await InitAsync();
        //    return await conn.DropTableAsync<Tbl_Usuario>();
        //}

    }




}

