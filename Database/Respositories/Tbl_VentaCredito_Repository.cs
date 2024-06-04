using SQLite;
using TesisAppSINMVVM.Database.Tables;

namespace TesisAppSINMVVM.Database.Respositories
{
    public class Tbl_VentaCredito_Repository
    {
        SQLiteAsyncConnection conn;
        public Tbl_VentaCredito_Repository() { }
        async Task InitAsync()
        {
            if (conn is not null)
                return;

            conn = new SQLiteAsyncConnection(Constantes.DatabasePath, Constantes.Flags);
            var resultado = await conn.CreateTableAsync<Tbl_VentaCredito>();
        }

        public async Task GuardarVEntaCredito(Tbl_VentaCredito t)
        {
            await InitAsync();
            await conn.InsertAsync(t);
        }
    }
}
