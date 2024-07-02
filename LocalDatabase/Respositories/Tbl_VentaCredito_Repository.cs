using SQLite;
using TesisAppSINMVVM.Database.Tables;
using TesisAppSINMVVM.Models;

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

        public async Task GuardarVentaCreditoAsync(VentaCredito ventaC)
        {
            await InitAsync();
            Tbl_VentaCredito tblVentaC = new()
            {
                COMPRADOR = ventaC.COMPRADOR,
                MONTOVENDIDO = ventaC.MONTOVENDIDO,
                DESCRIPCION = ventaC.DESCRIPCION,
                FECHAGUARDADO = ventaC.FECHAGUARDADO,
                DIAFECHAGUARDADO = ventaC.DIAFECHAGUARDADO
            };
            await conn.InsertAsync(ventaC);
        }
        public async Task<List<Tbl_VentaCredito>> ObtenerVentasCreditoAsync()
        {
            await InitAsync();
            return await conn.Table<Tbl_VentaCredito>().ToListAsync();
        }
    }
}
