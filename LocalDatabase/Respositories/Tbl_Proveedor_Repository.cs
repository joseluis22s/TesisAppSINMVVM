using SQLite;
using TesisAppSINMVVM.LocalDatabase.Tables;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.Database.Respositories
{
    public class Tbl_Proveedor_Repository
    {
        SQLiteAsyncConnection conn;
        public Tbl_Proveedor_Repository() { }
        async Task InitAsync()
        {
            if (conn is not null)
                return;

            conn = new SQLiteAsyncConnection(Constantes.DatabasePath, Constantes.Flags);
            var resultado = await conn.CreateTableAsync<Tbl_Proveedor>();
        }
        #region ESCRITURA
        public async Task GuardarNuevoProveedorAsync(Proveedor proveedor)
        {
            await InitAsync();
            Tbl_Proveedor tbl_Proveedor = new()
            {
                PROVEEDOR = proveedor.PROVEEDOR
            };
            bool existeProveedor = await VerificarExistenciaProveedorAsync(proveedor.PROVEEDOR);
            if (!existeProveedor)
            {
                await conn.InsertAsync(tbl_Proveedor);
            }
        }
        #endregion

        #region LECTURA
        public async Task<List<Tbl_Proveedor>> ObtenerTblProveedoresAsync()
        {
            await InitAsync();
            return await conn.Table<Tbl_Proveedor>().ToListAsync();
        }
        public async Task<Tbl_Proveedor> ObtenerProveedorAsync(Proveedor proveedor)
        {
            await InitAsync();
            return await conn.Table<Tbl_Proveedor>().Where(p => p.PROVEEDOR == proveedor.PROVEEDOR).FirstAsync();
        }
        #endregion


        public async Task<bool> VerificarExistenciaProveedorAsync(string proveedor)
        {
            await InitAsync();
            int resultado = await conn.Table<Tbl_Proveedor>().Where(x =>
                x.PROVEEDOR == proveedor).CountAsync();
            if (resultado == 0)
            {
                return false;
            }
            return true;
        }

        
        public async Task BorrarTblProveedorAsync()
        {
            await InitAsync();
            await conn.DropTableAsync<Tbl_Proveedor>();
        }
        public async Task BorrarProveedorAsync(Proveedor proveedor)
        {
            await InitAsync();
            await conn.Table<Tbl_Proveedor>().Where(p => p.PROVEEDOR == proveedor.PROVEEDOR).DeleteAsync();
        }
        public async Task GuardarTodoNuevoProveedorAsync(List<Tbl_Proveedor> t)
        {
            await InitAsync();
            await conn.InsertAllAsync(t);
        }
        public async Task EditarProveedorAsync(Proveedor proveedor,string nuevoNombrePRoveedor)
        {
            await InitAsync();
            var p = await ObtenerProveedorAsync(proveedor);
            p.PROVEEDOR = nuevoNombrePRoveedor;
            await conn.UpdateAsync(p);
            //await conn.UpdateAsync(Tbl_Proveedor);
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

