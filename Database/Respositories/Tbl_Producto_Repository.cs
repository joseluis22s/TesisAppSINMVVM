﻿using SQLite;
using TesisAppSINMVVM.Database.Tables;

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
        public async Task<bool> VerificarExistenciaProductoAsync(string productoNOmbre)
        {
            await InitAsync();
            int resultado = await conn.Table<Tbl_Producto>().CountAsync(producto => producto.PRODUCTO == productoNOmbre);
            if (resultado != 0)
            {
                return true;
            }
            return false;
        }
        public async Task GuardarProductoAsync(string producto)
        {
            await InitAsync();
            Tbl_Producto Producto = new Tbl_Producto()
            {
                PRODUCTO = producto
            };
            await conn.InsertAsync(Producto);
        }
        public async Task<List<Tbl_Producto>> ObtenerProdcutosAsync()
        {
            await InitAsync();
            return await conn.Table<Tbl_Producto>().ToListAsync();
        }


    }
}