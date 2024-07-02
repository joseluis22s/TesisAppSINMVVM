using SQLite;
using TesisAppSINMVVM.Database.Tables;

namespace TesisAppSINMVVM.Database.Respositories
{
    public class Tbl_ProductosInventario_Repository
    {
        SQLiteAsyncConnection conn;
        public Tbl_ProductosInventario_Repository() { }
        async Task InitAsync()
        {
            if (conn is not null)
                return;

            conn = new SQLiteAsyncConnection(Constantes.DatabasePath, Constantes.Flags);
            var resultado = await conn.CreateTableAsync<Tbl_ProductosInventario>();
        }

        public async Task GuardarProductosInventarioAsync(List<Tbl_ProductosInventario> t)
        {
            await InitAsync();
            await conn.InsertAllAsync(t);
        }
        
        public async Task<List<Tbl_ProductosInventario>> ObtenerInvetarioAsync()
        {
            await InitAsync();
            return await conn.Table<Tbl_ProductosInventario>().ToListAsync();
        }
    }
}
