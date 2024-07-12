using Plugin.CloudFirestore;
using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.LocalDatabase.Tables;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.FirebaseDataBase.Repositories
{
    public class Cheque_Repository
    {
        private Tbl_Cheque_Repository _repoTblCheque = new Tbl_Cheque_Repository();
        public Cheque_Repository() { }

        #region ESCRITURA
        public async Task GuardarNuevoResgitroChequesAsync(Cheque cheque)
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                await CrossCloudFirestore.Current
                         .Instance
                         .Collection("CHEQUE")
                         .AddAsync(cheque);
                await _repoTblCheque.GuardarChequeAsync(cheque);
            }
            else
            {
                await _repoTblCheque.GuardarChequeAsync(cheque);
            }
        }
        #endregion

        #region LECTURA
        public async Task<List<Tbl_Cheque>> ObtenerChequesAsync()
        {
            var documentos = await CrossCloudFirestore.Current
                                         .Instance
                                         .Collection("CHEQUE")
                                         .GetAsync();
            var chequesFirebase = documentos.ToObjects<Cheque>().ToList();

            List<Tbl_Cheque> chequesLocal = await _repoTblCheque.ObtenerChequesAsync();
            List<Tbl_Cheque> cheques = new List<Tbl_Cheque>(chequesLocal);

            var chequesToAdd = chequesFirebase
                .Select(p => new Tbl_Cheque 
                {
                    NUMERO = p.NUMERO,
                    MONTO = p.MONTO,
                    PROVEEDOR = p.PROVEEDOR,
                    FECHACOBRO = p.FECHACOBRO,
                    FECHAEMISION = p.FECHAEMISION,
                    DIAFECHACOBRO = p.DIAFECHACOBRO
                })
                .Where(p => !chequesLocal.Any(pl => pl.NUMERO == p.NUMERO));

            cheques.AddRange(chequesToAdd);

            return cheques;
        }


        public async Task<bool> VerificarExistenciaChequeAsync(int numeroCheque)
        {
            //var querys = await CrossCloudFirestore.Current
            //                         .Instance
            //                         .Collection("CHEQUE")
            //                         .WhereEqualsTo("NUMERO", numeroCheque)
            //                         .GetAsync();
            //var chequesContadosFirebase = query.ToObjects<Cheque>().ToList().Count();

            //var chequesLocaal = await _repoTblCheque.ObtenerChequesAsync();
            //int chequesContadosLocal = chequesLocal.Count();

            //List<Tbl_Cheque> chequess = new List<Tbl_Cheque>(chequesLocal);

            //if (chequesContadosFirebase != 0)
            //{
            //    return true;
            //}
            //return false;


            var query = await CrossCloudFirestore.Current
                                      .Instance
                                      .Collection("CHEQUE")
                                      .WhereEqualsTo("NUMERO", numeroCheque)
                                      .GetAsync();
            var chequesFirebaseIgualA = query.ToObjects<Cheque>().ToList();

            List<Tbl_Cheque> chequesLocal = await _repoTblCheque.ObtenerChequesAsync();
            List<Tbl_Cheque> chequesLocalIgualA = chequesLocal.Where(c => c.NUMERO == numeroCheque).ToList();
            List<Tbl_Cheque> cheques = new List<Tbl_Cheque> (); 

            foreach (var chqFirebase in chequesFirebaseIgualA)
            {
                if (!chequesLocalIgualA.Any(cl => cl.NUMERO == chqFirebase.NUMERO))
                {
                    chequesLocalIgualA.Add(new Tbl_Cheque
                    {
                        NUMERO = chqFirebase.NUMERO,
                        MONTO = chqFirebase.MONTO,
                        PROVEEDOR = chqFirebase.PROVEEDOR,
                        FECHACOBRO = chqFirebase.FECHACOBRO,
                        FECHAEMISION = chqFirebase.FECHAEMISION,
                        DIAFECHACOBRO = chqFirebase.DIAFECHACOBRO,
                    });
                }
            }

            if (chequesLocalIgualA.Any(c => c.NUMERO == numeroCheque))
            {
                return true;
            }
            return false;

            //List<Tbl_Cheque> cheques = new List<Tbl_Cheque>(chequesLocal);

            //var chequesToAdd = chequesFirebase
            //    .Select(p => new Tbl_Cheque
            //    {
            //        NUMERO = p.NUMERO,
            //        MONTO = p.MONTO,
            //        PROVEEDOR = p.PROVEEDOR,
            //        FECHACOBRO = p.FECHACOBRO,
            //        FECHAEMISION = p.FECHAEMISION,
            //        DIAFECHA = p.DIAFECHA
            //    })
            //    .Where(p => !chequesLocal.Any(pl => pl.NUMERO == p.NUMERO));

            //cheques.AddRange(chequesToAdd);

            //return cheques;
        }
        #endregion

    }
}
