using SQLite;
using TesisAppSINMVVM.LocalDatabase.Tables;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.Database.Respositories
{
    public class Tbl_Producto_Repository
    {
        SQLiteAsyncConnection conn;
        public Tbl_Producto_Repository() { }
        async Task InitAsync()
        {
            if (conn is not null)
                return;

            conn = new SQLiteAsyncConnection(Constantes.DatabasePath, Constantes.Flags);
            var resultado = await conn.CreateTableAsync<Tbl_Producto>();
        }
        public async Task<bool> VerificarExistenciaProductoAsync(string productoNOmbre, string medida)
        {
            await InitAsync();
            int resultado = await conn.Table<Tbl_Producto>().CountAsync(producto => 
                producto.PRODUCTO == productoNOmbre &&
                producto.MEDIDA == medida);
            if (resultado == 0)
            {
                return false;
            }
            return true;
        }
        public async Task GuardarProductoAsync(Producto producto)
        {
            await InitAsync();
            Tbl_Producto Producto = new Tbl_Producto()
            {
                PRODUCTO = producto.PRODUCTO,
                MEDIDA = producto.MEDIDA
            };
            bool existeProduto = await VerificarExistenciaProductoAsync(producto.PRODUCTO, producto.MEDIDA);
            if (!existeProduto)
            {
                await conn.InsertAsync(Producto);
            }
        }
        public async Task<List<Tbl_Producto>> ObtenerProductosAsync()
        {
            await InitAsync();
            return await conn.Table<Tbl_Producto>().ToListAsync();
        }

        public async Task BorrarTablaTbl_ProductoAsync()
        {
            await InitAsync();
            await conn.DropTableAsync<Tbl_Producto>();
        }
        public async Task GuardarTodoProductoAsync(List<Tbl_Producto> t)
        {
            await InitAsync();
            await conn.InsertAllAsync(t);
        }
    }
}
