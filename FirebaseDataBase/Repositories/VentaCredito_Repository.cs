using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.FirebaseDataBase.Repositories
{
    public class VentaCredito_Repository
    {
        VentaCredito_Repository() { }

        public static async Task<List<VentaCredito>> ObtenerVentasCreditoAsync()
        {
            var documentos = await CrossCloudFirestore.Current
                                         .Instance
                                         .Collection("VENTACREDITO")
                                         .GetAsync();
            var ventasCredito = documentos.ToObjects<VentaCredito>().ToList();
            return ventasCredito;
        }
        public static async Task GuardarVentaCreditoAsync(VentaCredito ventaCredito)
        {
            await CrossCloudFirestore.Current
                         .Instance
                         .Collection("VENTACREDITO")
                         .AddAsync(ventaCredito);
        }
    }
}
