using Plugin.CloudFirestore;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.FirebaseDataBase.Repositories
{
    public class HistorialCompras_Repository
    {
        public HistorialCompras_Repository() { }

        public static async Task GuardarRegistroProductoAsync(HistorialCompras registroCompra)
        {
            await CrossCloudFirestore.Current
                         .Instance
                         .Collection("HISTORIALCOMPRAS")
                         .AddAsync(registroCompra);
        }

        public static async Task<List<HistorialCompras>> ObtenerHistorialProductosAsync(string proveedor)
        {
            var query = await CrossCloudFirestore.Current
                                     .Instance
                                     .Collection("HISTORIALCOMPRAS")
                                     .WhereEqualsTo("PROVEEDOR", proveedor)
                                     .GetAsync();
            var compra = query.ToObjects<HistorialCompras>().ToList();

            return compra;
        }
    }
}
