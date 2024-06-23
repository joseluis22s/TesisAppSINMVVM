using Plugin.CloudFirestore;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.FirebaseDataBase.Repositories
{
    public class Comprador_Repository
    {
        public Comprador_Repository() { }

        public static async Task<List<Comprador>> ObtenerCompradoresAsync()
        {
            var documentos = await CrossCloudFirestore.Current
                                         .Instance
                                         .Collection("COMPRADOR")
                                         .GetAsync();
            var compradores = documentos.ToObjects<Comprador>().ToList();
            return compradores;
        }
        public static async Task GuardarNuevoCompradorAsync(Comprador comprador)
        {
            await CrossCloudFirestore.Current
                         .Instance
                         .Collection("COMPRADOR")
                         .AddAsync(comprador);
        }
    }
}
