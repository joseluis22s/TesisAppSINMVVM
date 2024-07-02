using SQLite;
using TesisAppSINMVVM.Database.Tables;
using TesisAppSINMVVM.Models;

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

        public async Task GuardarCompradorAsync(Comprador comprador)
        {
            await InitAsync();
            Tbl_Comprador tblComprador = new()
            {
                COMPRADOR = comprador.COMPRADOR
            };
            bool exiteComprador = await VerificarExistenciaCompradoAsync(tblComprador.COMPRADOR);
            if (!exiteComprador)
            {
                await conn.InsertAsync(comprador);
            }
        }
        public async Task<bool> VerificarExistenciaCompradoAsync(string comprador)
        {
            await InitAsync();
            int resultado = await conn.Table<Tbl_Comprador>().Where(c =>
                c.COMPRADOR == comprador).CountAsync();
            if (resultado == 0)
            {
                return false;
            }
            return true;
        }

        public async Task<List<Tbl_Comprador>> ObtenerTblCompradoresAsync()
        {
            await InitAsync();
            return await conn.Table<Tbl_Comprador>().ToListAsync();
        }
    }
}
