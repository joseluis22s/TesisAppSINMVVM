using SQLite;
using TesisAppSINMVVM.Database.Tables;

namespace TesisAppSINMVVM.Database.Respositories
{
    public class Tbl_Cheque_Repository
    {
        SQLiteAsyncConnection conn;
        public Tbl_Cheque_Repository() { }
        async Task InitAsync()
        {
            if (conn is not null)
                return;

            conn = new SQLiteAsyncConnection(Constantes.DatabasePath, Constantes.Flags);
            var resultado = await conn.CreateTableAsync<Tbl_Cheque>();
        }

        public async Task<bool> VerificarExistenciaChequeAsync(int numeroCheque)
        {
            await InitAsync();
            int resultado = await conn.Table<Tbl_Cheque>().Where(x => x.NUMERO == numeroCheque).CountAsync();
            if (resultado > 0)
            {
                return true;
            }
            return false;
        }
        public async Task GuardarChequeAsync(Tbl_Cheque cheque)
        {
            await InitAsync();
            await conn.InsertAsync(cheque);
        }
        public async Task BorarTbl_Cheque()
        {
            await InitAsync();
            await conn.DropTableAsync<Tbl_Cheque>();
        }
        public async Task<List<Tbl_Cheque>> ObtenerChequesAsync()
        {
            await InitAsync();
            return await conn.Table<Tbl_Cheque>().ToListAsync();
        }

    }
}
