using Plugin.CloudFirestore;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.FirebaseDataBase.Repositories
{
    public class Proveedor_Repository
    {
        public Proveedor_Repository() { }

        public static async Task<List<Proveedor>> ObtenerProveedoresAsync()
        {
            var documentos = await CrossCloudFirestore.Current
                                         .Instance
                                         .Collection("PROVEEDOR")
                                         .GetAsync();
            var proveedores = documentos.ToObjects<Proveedor>().ToList();
            return proveedores;
        }

        public static async Task GuardarNuevoProveedorAsync(Proveedor proveedor)
        {
            await CrossCloudFirestore.Current
                             .Instance
                             .Collection("PROVEEDOR")
                             .AddAsync(proveedor);
        }
    }
}
