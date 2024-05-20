using SQLite;
using TesisAppSINMVVM.Database.Tables;

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
        public async Task GuardarRegistroProductoAsync(Tbl_HistorialCompras registroCompra)
        {
            await InitAsync();
            await conn.InsertAsync(registroCompra);
        }
        public async Task<List<Tbl_HistorialCompras>> ObtenerHistorialProductosAsync(string nombreProveedor, string apellidoProveedor)
        {
            await InitAsync();
            return await conn.Table<Tbl_HistorialCompras>().Where(historial =>
            historial.NOMBRE.ToUpper() == nombreProveedor.ToUpper() &&
            historial.APELLIDO.ToUpper() == apellidoProveedor.ToUpper()
            ).ToListAsync();
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
