using SQLite;
using TesisAppSINMVVM.LocalDatabase.Tables;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.Database.Respositories
{
    public class Tbl_HistorialCompras_Repository
    {
        SQLiteAsyncConnection conn;
        public Tbl_HistorialCompras_Repository() { }
        async Task InitAsync()
        {
            if (conn is not null)
                return;

            conn = new SQLiteAsyncConnection(Constantes.DatabasePath, Constantes.Flags);
            var resultado = await conn.CreateTableAsync<Tbl_HistorialCompras>();
        }
        public async Task GuardarRegistroProductoAsync(HistorialCompras registroCompra)
        {
            await InitAsync();
            Tbl_HistorialCompras tbl_HistorialCompra = new Tbl_HistorialCompras()
            {
                DIAFECHA = registroCompra.DIAFECHA,
                PROVEEDOR = registroCompra.PROVEEDOR,
                PRODUCTO = registroCompra.PRODUCTO,
                MEDIDA = registroCompra.MEDIDA,
                FECHA = registroCompra.FECHA,
                CANTIDAD = registroCompra.CANTIDAD,
                PRECIO = registroCompra.PRECIO,
                TOTAL = registroCompra.TOTAL,
                //ABONO = registroCompra.ABONO,
                //SALDOPENDIENTE = registroCompra.SALDOPENDIENTE
            };
            bool existeHistorialCompra = await VerificarExistenciaHistorialCompraAsync(tbl_HistorialCompra);
            if (!existeHistorialCompra)
            {
                await conn.InsertAsync(tbl_HistorialCompra);
            }
        }
        public async Task<List<Tbl_HistorialCompras>> ObtenerHistorialProductosAsync(string nombreProveedor)
        {
            await InitAsync();
            return await conn.Table<Tbl_HistorialCompras>().Where(historial =>
            historial.PROVEEDOR.ToUpper() == nombreProveedor.ToUpper()
            ).ToListAsync();
        }
        public async Task<bool> VerificarExistenciaHistorialCompraAsync(Tbl_HistorialCompras compra)
        {
            await InitAsync();
            int resultado = await conn.Table<Tbl_HistorialCompras>().Where(x => 
                x.DIAFECHA == compra.DIAFECHA &&
                x.PROVEEDOR == compra.PROVEEDOR &&
                x.PRODUCTO == compra.PRODUCTO &&
                x.MEDIDA == compra.MEDIDA &&
                x.FECHA == compra.FECHA &&
                x.CANTIDAD == compra.CANTIDAD &&
                x.PRECIO == compra.PRECIO &&
                x.TOTAL == compra.TOTAL /*&&
                x.ABONO == compra.ABONO &&
                x.DIAFECHA == compra.DIAFECHA*/
            ).CountAsync();
            if (resultado == 0)
            {
                return false;
            }
            return true;
        }
        public async Task<List<Tbl_HistorialCompras>> ObtenerTodoHistorialProductosAsync()
        {
            await InitAsync();
            return await conn.Table<Tbl_HistorialCompras>().ToListAsync();
        }
        public async Task BorrarTblHistorialProductosAsync()
        {
            await InitAsync();
            await conn.DropTableAsync<Tbl_HistorialCompras>();
        }
        public async Task GuardarTodoListaHistorialCompras(List<Tbl_HistorialCompras> t)
        {
            await InitAsync();
            await conn.InsertAllAsync(t);
        }
    }
}
