using SQLite;
using TesisAppSINMVVM.Database;
using TesisAppSINMVVM.LocalDatabase.Tables;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.LocalDatabase.Respositories
{
    public class Tbl_ProductoComprado_Repository
    {
        SQLiteAsyncConnection conn;

        public Tbl_ProductoComprado_Repository() { }

        async Task InitAsync()
        {
            if (conn is not null)
                return;

            conn = new SQLiteAsyncConnection(Constantes.DatabasePath, Constantes.Flags);
            var resultado = await conn.CreateTableAsync<Tbl_ProductoComprado>();
        }

        public async Task<List<Tbl_ProductoComprado>> ObtenerProductosCompradosAsync(string nombreProveedor)
        {
            await InitAsync();
            return await conn.Table<Tbl_ProductoComprado>().Where(historial =>
            historial.PROVEEDOR.ToUpper() == nombreProveedor.ToUpper()
            ).ToListAsync();
        }

        public async Task<int> ObtenerNumeroProductosCompradosAsync(string nombreProveedor)
        {
            await InitAsync();
            return await conn.Table<Tbl_ProductoComprado>().Where(pC =>
            pC.PROVEEDOR.ToUpper() == nombreProveedor.ToUpper()
            ).CountAsync();
        }
        public async Task GuardarNuevaCompraProductoCompradoAsync(ProductoComprado pC)
        {
            await InitAsync();
            Tbl_ProductoComprado tblProductoComprado = new Tbl_ProductoComprado()
            {
                NUMEROCOMPRA = pC.NUMEROCOMPRA,
                PRODUCTO = pC.PRODUCTO,
                MEDIDA = pC.MEDIDA,
                CANTIDAD = pC.CANTIDAD,
                PRECIO = pC.PRECIO,
                TOTAL = pC.TOTAL,
                FECHAGUARDADO = pC.FECHAGUARDADO,
                DIAFECHAGUARDADO = pC.DIAFECHAGUARDADO,
                PROVEEDOR = pC.PROVEEDOR
            };
            bool existeProductoComprado = await VerificarExistenciaProductoCompradoAsync(tblProductoComprado);
            if (!existeProductoComprado)
            {
                await conn.InsertAsync(tblProductoComprado);
            }
        }
        public async Task<bool> VerificarExistenciaProductoCompradoAsync(Tbl_ProductoComprado p)
        {
            await InitAsync();
            int resultado = await conn.Table<Tbl_ProductoComprado>().Where(x =>
                x.NUMEROCOMPRA == p.NUMEROCOMPRA).CountAsync();
            if (resultado == 0)
            {
                return false;
            }
            return true;
        }

        public async Task BorrarTblProductoComprado()
        {
            await InitAsync();
            await conn.DropTableAsync<Tbl_ProductoComprado>();
        }

    }
}
