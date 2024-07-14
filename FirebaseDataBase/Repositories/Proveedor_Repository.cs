using Plugin.CloudFirestore;
using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.LocalDatabase.Tables;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.FirebaseDataBase.Repositories
{
    public class Proveedor_Repository
    {
        private Tbl_Proveedor_Repository _repoTblProveedor = new Tbl_Proveedor_Repository();
        #region ESCRITURA
        #endregion

        #region LECTURA
        #endregion

        public Proveedor_Repository() { }


        #region ESCRITURA
        public async Task GuardarNuevoProveedorAsync(Proveedor proveedor)
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                await CrossCloudFirestore.Current
                             .Instance
                             .Collection("PROVEEDOR")
                             .AddAsync(proveedor);
                await _repoTblProveedor.GuardarNuevoProveedorAsync(proveedor);
            }
            //else
            //{
            //    await _repoTblProveedor.GuardarNuevoProveedorAsync(proveedor);
            //}
        }
        public async Task BorrarProveedorAsync(Proveedor proveedor)
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                var documentoProveedor = await CrossCloudFirestore.Current
                    .Instance
                    .Collection("PROVEEDOR")
                    .WhereEqualsTo("PROVEEDOR", proveedor.PROVEEDOR).GetAsync();
                string idDocumento = documentoProveedor.Documents.First().Id;
                await CrossCloudFirestore.Current
                             .Instance
                             .Collection("PROVEEDOR")
                             .Document(idDocumento).DeleteAsync();
                await _repoTblProveedor.BorrarProveedorAsync(proveedor);
            }
        }
        public async Task EditarProveedorAsync(Proveedor proveedor, string nuevoNombreProveeedor)
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                var documentoProveedor = await CrossCloudFirestore.Current
                    .Instance
                    .Collection("PROVEEDOR")
                    .WhereEqualsTo("PROVEEDOR", proveedor.PROVEEDOR).GetAsync();
                string idDocumento = documentoProveedor.Documents.First().Id;

                await CrossCloudFirestore.Current
                             .Instance
                             .Collection("PROVEEDOR")
                             .Document(idDocumento)
                             .UpdateAsync(new {PROVEEDOR = nuevoNombreProveeedor});
                await _repoTblProveedor.EditarProveedorAsync(proveedor, nuevoNombreProveeedor);
            }
        }
        #endregion

        #region LECTURA
        public async Task<List<Tbl_Proveedor>> ObtenerProveedoresAsync()
        {
            var documentos = await CrossCloudFirestore.Current
                                         .Instance
                                         .Collection("PROVEEDOR")
                                         .GetAsync();
            var proveedoresFirebase = documentos.ToObjects<Proveedor>().ToList();

            List<Tbl_Proveedor> proveedoresLocal = await _repoTblProveedor.ObtenerTblProveedoresAsync();
            List<Tbl_Proveedor> proveedores = new List<Tbl_Proveedor>(proveedoresLocal);

            var proveedoresToAdd = proveedoresFirebase
                .Select(p => new Tbl_Proveedor { PROVEEDOR = p.PROVEEDOR })
                .Where(p => !proveedoresLocal.Any(pl => pl.PROVEEDOR == p.PROVEEDOR));

            proveedores.AddRange(proveedoresToAdd);

            return proveedores;
        }
        #endregion


    }
}
