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
            bool exiteVentaC = await VerificarExistenciaVentaCredito(ventaC);
            if (!exiteVentaC)
            {
                await conn.InsertAsync(tblVentaC);
            }
        }
        public async Task<bool> VerificarExistenciaVentaCredito(VentaCredito ventaC)
        {
            await InitAsync();
            int resultado = await conn.Table<Tbl_VentaCredito>().Where(x =>
                x.COMPRADOR == ventaC.COMPRADOR &&
                x.MONTOVENDIDO == ventaC.MONTOVENDIDO &&
                x.DESCRIPCION == ventaC.DESCRIPCION &&
                x.FECHAGUARDADO == ventaC.FECHAGUARDADO &&
                x.DIAFECHAGUARDADO == ventaC.DIAFECHAGUARDADO 
            ).CountAsync();
            if (resultado == 0)
            {
                return false;
            }
            return true;
        }
        public async Task<List<Tbl_VentaCredito>> ObtenerVentasCreditoAsync()
        {
            await InitAsync();
            return await conn.Table<Tbl_VentaCredito>().ToListAsync();
        }
        public async Task BorrarTblVentaCredito()
        {
            await InitAsync();
            await conn.DropTableAsync<Tbl_VentaCredito>();
        }
    }
}
