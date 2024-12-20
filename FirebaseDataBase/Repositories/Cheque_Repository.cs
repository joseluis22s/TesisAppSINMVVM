﻿using Plugin.CloudFirestore;
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
                //await _repoTblCheque.GuardarChequeAsync(cheque);
            }
            //else
            //{
            //    await _repoTblCheque.GuardarChequeAsync(cheque);
            //}
        }
        public async Task EliminarRegistroChequeAsync(Cheque cheque)
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                var documentoCheque = await CrossCloudFirestore.Current
                                      .Instance
                                      .Collection("CHEQUE")
                                      .WhereEqualsTo("NUMERO", cheque.NUMERO)
                                      .GetAsync();
                string idDocumento = documentoCheque.Documents.First().Id;
                await CrossCloudFirestore.Current
                             .Instance
                             .Collection("CHEQUE")
                             .Document(idDocumento).DeleteAsync();
                //await _repoTblCheque.BorrarChequeAsync(cheque.NUMERO);
            }
        }
        public async Task EditarRegistroCheque(Cheque cheque1, Cheque cheque2 )
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                var documentoCheque = await CrossCloudFirestore.Current
                                      .Instance
                                      .Collection("CHEQUE")
                                      .WhereEqualsTo("NUMERO", cheque1.NUMERO)
                                      .GetAsync();
                string idDocumento = documentoCheque.Documents.First().Id;
                await CrossCloudFirestore.Current
                             .Instance
                             .Collection("CHEQUE")
                             .Document(idDocumento)
                             .UpdateAsync(new { 
                                 MONTO = cheque2.MONTO,
                                 PROVEEDOR = cheque2.PROVEEDOR,
                                 FECHACOBRO = cheque2.FECHACOBRO,
                                 FECHAEMISION = cheque2.FECHAEMISION,
                                 DIAFECHACOBRO = cheque2.DIAFECHACOBRO
                             });

                //await _repoTblCheque.EditarChequeAsync(cheque1, cheque2);
            }
        }
        public async Task CambiarCobradoRegistroChequeAsync(int numeroCheque, bool cobrado)
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                var documentoCheque = await CrossCloudFirestore.Current
                                      .Instance
                                      .Collection("CHEQUE")
                                      .WhereEqualsTo("NUMERO", numeroCheque)
                                      .GetAsync();
                string idDocumento = documentoCheque.Documents.First().Id;
                await CrossCloudFirestore.Current
                             .Instance
                             .Collection("CHEQUE")
                             .Document(idDocumento)
                             .UpdateAsync(new
                             {
                                 COBRADO = cobrado
                             });

                //await _repoTblCheque.EditarChequeAsync(cheque1, cheque2);
            }
        }
        public async Task EditarProveedorEnCheques(string nombreProveedor, string nuevoNombre)
        {
            List<string> ids = await ObtenerIDChequesPorProveedorAsync(nombreProveedor);
            foreach (var id in ids)
            {
                await CrossCloudFirestore.Current
                            .Instance
                            .Collection("CHEQUE")
                            .Document(id)
                            .UpdateAsync(new { PROVEEDOR = nuevoNombre });
                //await _repoTblCheque.EditarProveedorEnCheque(nombreProveedor, nuevoNombre);
            }
        }
        #endregion

        #region LECTURA
        public async Task<List<string>> ObtenerIDChequesPorProveedorAsync(string nombreProveedor)
        {
            var query = await CrossCloudFirestore.Current
                                      .Instance
                                      .Collection("CHEQUE")
                                      .WhereEqualsTo("PROVEEDOR", nombreProveedor)
                                      .GetAsync();
            var documentos = query.Documents.ToList();

            List<string> idsDocumentos = new List<string> ();

            foreach (var id in documentos)
            {
                idsDocumentos.Add(id.Id);
            }
            return idsDocumentos;
        }
        
        public async Task<List<Tbl_Cheque>> ObtenerChequesAsync()
        {
            var documentos = await CrossCloudFirestore.Current
                                         .Instance
                                         .Collection("CHEQUE")
                                         .GetAsync();
            var chequesFirebase = documentos.ToObjects<Tbl_Cheque>().ToList();

            return chequesFirebase;
            //var documentos = await CrossCloudFirestore.Current
            //                             .Instance
            //                             .Collection("CHEQUE")
            //                             .GetAsync();
            //var chequesFirebase = documentos.ToObjects<Cheque>().ToList();

            //List<Tbl_Cheque> chequesLocal = await _repoTblCheque.ObtenerChequesAsync();
            //List<Tbl_Cheque> cheques = new List<Tbl_Cheque>(chequesLocal);

            //var chequesToAdd = chequesFirebase
            //    .Select(p => new Tbl_Cheque 
            //    {
            //        NUMERO = p.NUMERO,
            //        MONTO = p.MONTO,
            //        PROVEEDOR = p.PROVEEDOR,
            //        FECHACOBRO = p.FECHACOBRO,
            //        FECHAEMISION = p.FECHAEMISION,
            //        DIAFECHACOBRO = p.DIAFECHACOBRO
            //    })
            //    .Where(p => !chequesLocal.Any(pl => pl.NUMERO == p.NUMERO));

            //cheques.AddRange(chequesToAdd);

            //return cheques;
        }

        public async Task<bool> VerificarExistenciaChequeAsync(int numeroCheque)
        {
            var query = await CrossCloudFirestore.Current
                                      .Instance
                                      .Collection("CHEQUE")
                                      .WhereEqualsTo("NUMERO", numeroCheque)
                                      .GetAsync();
            var chequesFirebaseIgualA = query.ToObjects<Tbl_Cheque>().ToList();

            if (chequesFirebaseIgualA.Any(c => c.NUMERO == numeroCheque))
            {
                return true;
            }
            return false;
            //var query = await CrossCloudFirestore.Current
            //                          .Instance
            //                          .Collection("CHEQUE")
            //                          .WhereEqualsTo("NUMERO", numeroCheque)
            //                          .GetAsync();
            //var chequesFirebaseIgualA = query.ToObjects<Cheque>().ToList();

            //List<Tbl_Cheque> chequesLocal = await _repoTblCheque.ObtenerChequesAsync();
            //List<Tbl_Cheque> chequesLocalIgualA = chequesLocal.Where(c => c.NUMERO == numeroCheque).ToList();
            //List<Tbl_Cheque> cheques = new List<Tbl_Cheque> (); 

            //foreach (var chqFirebase in chequesFirebaseIgualA)
            //{
            //    if (!chequesLocalIgualA.Any(cl => cl.NUMERO == chqFirebase.NUMERO))
            //    {
            //        chequesLocalIgualA.Add(new Tbl_Cheque
            //        {
            //            NUMERO = chqFirebase.NUMERO,
            //            MONTO = chqFirebase.MONTO,
            //            PROVEEDOR = chqFirebase.PROVEEDOR,
            //            FECHACOBRO = chqFirebase.FECHACOBRO,
            //            FECHAEMISION = chqFirebase.FECHAEMISION,
            //            DIAFECHACOBRO = chqFirebase.DIAFECHACOBRO,
            //        });
            //    }
            //}

            //if (chequesLocalIgualA.Any(c => c.NUMERO == numeroCheque))
            //{
            //    return true;
            //}
            //return false;
        }
        #endregion

    }
}
