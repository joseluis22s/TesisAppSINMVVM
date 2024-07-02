using SQLite;
using TesisAppSINMVVM.Database.Tables;

namespace TesisAppSINMVVM.Database.Respositories
{
    public class Tbl_Comprador_Repository
    {
        SQLiteAsyncConnection conn;
        public Tbl_Comprador_Repository() { }
        async Task InitAsync()
        {
            if (conn is not null)
                return;

            conn = new SQLiteAsyncConnection(Constantes.DatabasePath, Constantes.Flags);
            var resultado = await conn.CreateTableAsync<Tbl_Comprador>();
        }

        public async Task GuardarCompradorAsync(Tbl_Comprador comprador)
        {
            await InitAsync();
            await conn.InsertAsync(comprador);
        }

        public async Task<List<Tbl_Comprador>> ObtenerTblCompradoresAsync()
        {
            await InitAsync();
            return await conn.Table<Tbl_Comprador>().ToListAsync();
        }
    }
}
