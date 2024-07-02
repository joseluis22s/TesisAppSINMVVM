using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.FirebaseDataBase.Repositories
{
    public class ProductoInventarioBodega_Repository
    {
        private Tbl_ProductosInventario_Repository _repoProductosInventario = new Tbl_ProductosInventario_Repository();
        public ProductoInventarioBodega_Repository() { }

        public async Task GuardarProductosInventarioAsync(ProductoInventarioBodega productoInventarioBodega)
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                await CrossCloudFirestore.Current
                             .Instance
                             .Collection("PRODUCTOSINVENTARIO")
                             .AddAsync(productoInventarioBodega);
                await _repoProductosInventario.GuardarProductosInventarioAsync(productoInventarioBodega);
            }
            else
            {
                await _repoProductosInventario.GuardarProductosInventarioAsync(productoInventarioBodega);
            }
        }
        public static async Task<List<ProductoInventarioBodega>> ObtenerProductosInventarioAsync()
        {
            var documentos = await CrossCloudFirestore.Current
                                        .Instance
                                        .Collection("PRODUCTOSINVENTARIO")
                                        .GetAsync();
            var inventarioProductos = documentos.ToObjects<ProductoInventarioBodega>().ToList();
            return inventarioProductos;
        }

    }
}
