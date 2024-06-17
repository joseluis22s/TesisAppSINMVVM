using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.FirebaseDataBase.Repositories
{
    public class Producto_Repository
    {
        public Producto_Repository() { }

        public static async Task<List<Producto>> ObtenerProductosAsync()
        {
            var documentos = await CrossCloudFirestore.Current
                                         .Instance
                                         .Collection("PRODUCTO")
                                         .GetAsync();
            var productos = documentos.ToObjects<Producto>().ToList();
            return productos;
        }

        public static async Task GuardarNuevoProductoAsync(Producto producto)
        {
            await CrossCloudFirestore.Current
                         .Instance
                         .Collection("PRODUCTO")
                         .AddAsync(producto);
        }
    }
}
