using SQLite;
using TesisAppSINMVVM.LocalDatabase.Tables;
using TesisAppSINMVVM.Models;

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
            if (resultado == 0)
            {
                return false;
            }
            return true;
        }
        public async Task GuardarChequeAsync(Cheque cheque)
        {
            await InitAsync();
            Tbl_Cheque tbl_Cheque = new Tbl_Cheque()
            {
                NUMERO = cheque.NUMERO,
                MONTO = cheque.MONTO,
                PROVEEDOR = cheque.PROVEEDOR,
                FECHACOBRO = cheque.FECHACOBRO,
                FECHAEMISION = cheque.FECHAEMISION,
                DIAFECHACOBRO = cheque.DIAFECHACOBRO,
            };
            bool existeCheque = await VerificarExistenciaChequeAsync(tbl_Cheque.NUMERO);
            if (!existeCheque)
            {
                await conn.InsertAsync(tbl_Cheque);
            }
        }
        public async Task BorrarChequeAsync(int numeroCheque)
        {
            await InitAsync();
            await conn.Table<Tbl_Cheque>().Where(p => p.NUMERO == numeroCheque).DeleteAsync();
        }
        public async Task EditarChequeAsync(Cheque cheque, Cheque nuevoCheque)
        {
            await InitAsync();
            var p = await ObtenerChequeAsync(cheque);
            p.MONTO = nuevoCheque.MONTO;
            p.PROVEEDOR = nuevoCheque.PROVEEDOR;
            p.FECHACOBRO = nuevoCheque.FECHACOBRO;
            p.FECHAEMISION = nuevoCheque.FECHAEMISION;
            p.DIAFECHACOBRO = nuevoCheque.DIAFECHACOBRO;
            await conn.UpdateAsync(p);
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
        public async Task<Tbl_Cheque> ObtenerChequeAsync(Cheque cheque)
        {
            await InitAsync();
            return await conn.Table<Tbl_Cheque>().Where(c => c.NUMERO == cheque.NUMERO).FirstAsync();
        }

    }
}
