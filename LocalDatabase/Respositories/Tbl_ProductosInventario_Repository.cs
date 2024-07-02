using SQLite;
using TesisAppSINMVVM.Database.Tables;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.Database.Respositories
{
    public class Tbl_ProductosInventario_Repository
    {
        SQLiteAsyncConnection conn;
        public Tbl_ProductosInventario_Repository() { }
        async Task InitAsync()
        {
            if (conn is not null)
                return;

            conn = new SQLiteAsyncConnection(Constantes.DatabasePath, Constantes.Flags);
            var resultado = await conn.CreateTableAsync<Tbl_ProductosInventario>();
        }

        public async Task GuardarProductosInventarioAsync(ProductoInventarioBodega productoInventarioBodega)
        {
            await InitAsync();
            Tbl_ProductosInventario tblProductoInventarioBodega = new Tbl_ProductosInventario()
            {
                PRODUCTO = productoInventarioBodega.PRODUCTO,
                MEDIDA = productoInventarioBodega.MEDIDA,
                CANTIDAD = productoInventarioBodega.CANTIDAD,
                DESCRIPCION = productoInventarioBodega.DESCRIPCION,
                FECHAGUARDADO = productoInventarioBodega.FECHAGUARDADO,
                DIAFECHAGUARDADO = productoInventarioBodega.DIAFECHAGUARDADO,
            };
            bool existeProductoInventarioBodega = await VerificarExistenciaProductoInventarioBodegaAsync(tblProductoInventarioBodega);
            if (!existeProductoInventarioBodega)
            {
                await conn.InsertAsync(tblProductoInventarioBodega);
            }
        }
        
        public async Task<List<Tbl_ProductosInventario>> ObtenerInvetarioAsync()
        {
            await InitAsync();
            return await conn.Table<Tbl_ProductosInventario>().ToListAsync();
        }
        public async Task<bool> VerificarExistenciaProductoInventarioBodegaAsync(Tbl_ProductosInventario tblpIBodega)
        {
            await InitAsync();
            int resultado = await conn.Table<Tbl_ProductosInventario>().Where(x => 
            x.PRODUCTO == tblpIBodega.PRODUCTO &&
            x.MEDIDA == tblpIBodega.MEDIDA &&
            x.CANTIDAD == tblpIBodega.CANTIDAD &&
            x.DESCRIPCION == tblpIBodega.DESCRIPCION &&
            x.FECHAGUARDADO == tblpIBodega.FECHAGUARDADO &&
            x.DIAFECHAGUARDADO == tblpIBodega.DIAFECHAGUARDADO
            ).CountAsync();
            if (resultado == 0)
            {
                return false;
            }
            return true;
        }
        public async Task BorrarTblProductoInveario()
        {
            await InitAsync();
            await conn.DropTableAsync<Tbl_ProductosInventario>();
        }
    }
}
