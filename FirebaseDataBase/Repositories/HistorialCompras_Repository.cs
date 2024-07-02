using Plugin.CloudFirestore;
using System.Collections.Generic;
using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.Database.Tables;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.FirebaseDataBase.Repositories
{
    public class HistorialCompras_Repository
    {
        private Tbl_HistorialCompras_Repository _repoTblHistorialCompra = new Tbl_HistorialCompras_Repository();
        public HistorialCompras_Repository() { }

        #region ESCRITURA
        public async Task GuardarRegistroProductoAsync(HistorialCompras registroCompra)
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                await CrossCloudFirestore.Current
                         .Instance
                         .Collection("HISTORIALCOMPRAS")
                         .AddAsync(registroCompra);

                await _repoTblHistorialCompra.GuardarRegistroProductoAsync(registroCompra);
            }
            else
            {
                await _repoTblHistorialCompra.GuardarRegistroProductoAsync(registroCompra);
            }
        }
        #endregion


        #region LECTURA
        public async Task<List<Tbl_HistorialCompras>> ObtenerHistorialProductosAsync(string proveedor)
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                var documentos = await CrossCloudFirestore.Current
                                         .Instance
                                         .Collection("HISTORIALCOMPRAS")
                                         .WhereEqualsTo("PROVEEDOR", proveedor)
                                         .GetAsync();
                var comprasFirebase = documentos.ToObjects<HistorialCompras>().ToList();

                List<Tbl_HistorialCompras> historialComprasLocal = await _repoTblHistorialCompra.ObtenerHistorialProductosAsync(proveedor);
                List<Tbl_HistorialCompras> historialCompras = new List<Tbl_HistorialCompras>(historialComprasLocal);

                var historialProductosToAdd = comprasFirebase
                    .Select(p => new Tbl_HistorialCompras
                    {
                        DIAFECHA = p.DIAFECHA,
                        PROVEEDOR = p.PROVEEDOR,
                        PRODUCTO = p.PRODUCTO,
                        MEDIDA = p.MEDIDA,
                        FECHA = p.FECHA,
                        CANTIDAD = p.CANTIDAD,
                        PRECIO = p.PRECIO,
                        TOTAL = p.TOTAL,
                        ABONO = p.ABONO,
                        SALDOPENDIENTE = p.SALDOPENDIENTE,
                    })
                    .Where(p => !historialComprasLocal.Any(pl => pl.PRODUCTO == p.PRODUCTO));

                historialCompras.AddRange(historialProductosToAdd);

                return historialCompras;
            }
            else
            {
                return await _repoTblHistorialCompra.ObtenerHistorialProductosAsync(proveedor);
            }
        }
        #endregion
    }
}
