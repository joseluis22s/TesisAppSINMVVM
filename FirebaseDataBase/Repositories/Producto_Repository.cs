using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.Database.Tables;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.FirebaseDataBase.Repositories
{
    public class Producto_Repository
    {
        private Tbl_Producto_Repository _repoTblProducto = new Tbl_Producto_Repository();
        public Producto_Repository() { }

        public async Task<List<Tbl_Producto>> ObtenerProductosAsync()
        {
            var documentos = await CrossCloudFirestore.Current
                                         .Instance
                                         .Collection("PRODUCTO")
                                         .GetAsync();
            var productosFirebase = documentos.ToObjects<Producto>().ToList();

            List<Tbl_Producto> productosLocal = await _repoTblProducto.ObtenerProductosAsync();
            List<Tbl_Producto> productos = new List<Tbl_Producto>(productosLocal);

            var productosToAdd = productosFirebase.
               Select(p => new Tbl_Producto
               {
                   PRODUCTO = p.PRODUCTO,
                   MEDIDA = p.MEDIDA
               }).Where(p => !productosLocal.Any(pl =>
               pl.PRODUCTO == p.PRODUCTO &&
               pl.MEDIDA == p.MEDIDA
               ));
            productos.AddRange(productosToAdd);
            return productos;
        }

        public async Task GuardarNuevoProductoAsync(Producto producto)
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                await CrossCloudFirestore.Current
                             .Instance
                             .Collection("PRODUCTO")
                             .AddAsync(producto);
                await _repoTblProducto.GuardarProductoAsync(producto);
            }
            else
            {
                await _repoTblProducto.GuardarProductoAsync(producto);
            }
        }
    }
}
