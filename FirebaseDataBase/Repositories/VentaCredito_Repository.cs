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
    public class VentaCredito_Repository
    {
        private Tbl_VentaCredito_Repository _repoTblVentaCredito = new Tbl_VentaCredito_Repository();
        public VentaCredito_Repository() { }

        public static async Task<List<VentaCredito>> ObtenerVentasCreditoAsync()
        {
            var documentos = await CrossCloudFirestore.Current
                                         .Instance
                                         .Collection("VENTACREDITO")
                                         .GetAsync();
            var ventasCredito = documentos.ToObjects<VentaCredito>().ToList();
            return ventasCredito;
        }
        public async Task GuardarVentaCreditoAsync(VentaCredito ventaCredito)
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                await CrossCloudFirestore.Current
                             .Instance
                             .Collection("VENTACREDITO")
                             .AddAsync(ventaCredito);
                await _repoTblVentaCredito.GuardarVentaCreditoAsync(ventaCredito);
            }
            else
            {
                await _repoTblVentaCredito.GuardarVentaCreditoAsync(ventaCredito);
            }
        }
    }
}
