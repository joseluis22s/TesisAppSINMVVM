using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.FirebaseDataBase.Repositories
{
    public class Cheque_Repository
    {
        public Cheque_Repository() { }

        public static async Task<List<Cheque>> ObtenerChequesAsync()
        {
            var documentos = await CrossCloudFirestore.Current
                                         .Instance
                                         .Collection("CHEQUE")
                                         .GetAsync();
            var cheques = documentos.ToObjects<Cheque>().ToList();
            return cheques;
        }

        public static async Task GuardarNuevoResgitroChequesAsync(Cheque cheque)
        {
            await CrossCloudFirestore.Current
                         .Instance
                         .Collection("CHEQUE")
                         .AddAsync(cheque);
        }
        public static async Task<bool> VerificarExistenciaChequeAsync(int numeroCheque)
        {
            var query = await CrossCloudFirestore.Current
                                     .Instance
                                     .Collection("CHEQUE")
                                     .WhereEqualsTo("NUMERO", numeroCheque)
                                     .GetAsync();
            var q = query.ToObjects<Cheque>().ToList().Count();
            if (q != 0)
            {
                return true;
            }
            return false;
        }
    }
}
