using SQLite;
using TesisAppSINMVVM.Database;
using TesisAppSINMVVM.Database.Tables;
using TesisAppSINMVVM.LocalDatabase.Tables;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.LocalDatabase.Respositories
{
    public class Tbl_ProductoComprado_Repository
    {
        SQLiteAsyncConnection conn;

        public Tbl_ProductoComprado_Repository() { }

        async Task InitAsync()
        {
            if (conn is not null)
                return;

            conn = new SQLiteAsyncConnection(Constantes.DatabasePath, Constantes.Flags);
            var resultado = await conn.CreateTableAsync<Tbl_Comprador>();
        }

        public async Task<int> ObetenerProductoCompradoAsync()
        {
            await InitAsync();
            int resultado = await conn.Table<Tbl_ProductoComprado>
        }


    }
}
