using Plugin.CloudFirestore;
using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.LocalDatabase.Tables;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.FirebaseDataBase.Repositories
{
    public class Comprador_Repository
    {
        private Tbl_Comprador_Repository _repoTblComprador = new Tbl_Comprador_Repository();
        public Comprador_Repository() { }

        #region ESCRITURA
        public async Task GuardarNuevoCompradorAsync(Comprador comprador)
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                await CrossCloudFirestore.Current
                         .Instance
                         .Collection("COMPRADOR")
                         .AddAsync(comprador);
                await _repoTblComprador.GuardarCompradorAsync(comprador);
            }
            //else
            //{
            //    await _repoTblComprador.GuardarCompradorAsync(comprador);
            //}
        }
        #endregion

        #region LECTURA
        public async Task<List<Tbl_Comprador>> ObtenerCompradoresAsync()
        {
            var documentos = await CrossCloudFirestore.Current
                                         .Instance
                                         .Collection("COMPRADOR")
                                         .GetAsync();
            var compradoresFirebase = documentos.ToObjects<Comprador>().ToList();

            List<Tbl_Comprador> compradoresLocal = await _repoTblComprador.ObtenerTblCompradoresAsync();
            List<Tbl_Comprador> compradores = new List<Tbl_Comprador>(compradoresLocal);

            var compradoresToAdd = compradoresFirebase
                .Select(c => new Tbl_Comprador { COMPRADOR = c.COMPRADOR })
                .Where(c => !compradoresLocal.Any(cl => cl.COMPRADOR == c.COMPRADOR));

            compradores.AddRange(compradoresToAdd);

            return compradores;
        }
        #endregion
    }
}
