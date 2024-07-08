using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesisAppSINMVVM.LocalDatabase.Respositories;
using TesisAppSINMVVM.LocalDatabase.Tables;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.FirebaseDataBase.Repositories
{
    public class ProductoComprado_Repository
    {
        private Tbl_ProductoComprado_Repository _repoTblProductoComprado = new Tbl_ProductoComprado_Repository();

        public ProductoComprado_Repository() { }

        #region LECTURA
        public async Task<int> ObtenerNumeroCompras()
        {
            var documentos = await CrossCloudFirestore.Current
                                        .Instance
                                        .Collection("PRODUCTOCOMPRADO")
                                        .GetAsync();
            var productosCompradosFirebase = documentos.ToObjects<ProductoComprado>().ToList();

            List<Tbl_ProductoComprado> productosCompradosLocal = await _repoTblProductoComprado.
        }
        #endregion

    }
}
