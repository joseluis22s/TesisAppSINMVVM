using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.FirebaseDataBase.Repositories
{
    public class ProductoInventarioBodega_Repository
    {
        ProductoInventarioBodega_Repository() { }

        public static async Task GuardarProductosInventarioAsync(ProductoInventarioBodega productoInventarioBodega)
        {
            await CrossCloudFirestore.Current
                         .Instance
                         .Collection("PRODUCTOSINVENTARIO")
                         .AddAsync(productoInventarioBodega);
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
