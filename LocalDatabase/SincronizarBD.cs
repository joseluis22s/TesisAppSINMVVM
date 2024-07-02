
using Plugin.CloudFirestore;
using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.Database.Tables;
using TesisAppSINMVVM.FirebaseDataBase.Repositories;
using TesisAppSINMVVM.Models;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace TesisAppSINMVVM.LocalDatabase
{
    public class SincronizarBD
    {
        private Proveedor_Repository _repoProveedor = new Proveedor_Repository();
        private Tbl_Proveedor_Repository _repoTblProveedor = new Tbl_Proveedor_Repository();
        private Cheque_Repository _repoCheque = new Cheque_Repository();
        private Tbl_Cheque_Repository _repoTblCheque = new Tbl_Cheque_Repository(); 
        private HistorialCompras_Repository _repoHistorialCompras = new HistorialCompras_Repository();
        private Tbl_HistorialCompras_Repository _repoTblHistorialCompras = new Tbl_HistorialCompras_Repository();

        public SincronizarBD() =>
        Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

        ~SincronizarBD() =>
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;

        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    Sincronizacion().GetAwaiter();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }
        // TODO: Suscribir al evento cuando se reestablece conexión
        //       a internet para sincronizar las BD, comparando que
        //       registros son diferentes y guardarlos en un auxiliar y
        //       llenar en el lado que falta BD local o BD  firebase
        private async Task Sincronizacion()
        {
            await SincronizacionProveedores();
            await SincronizacionHistorialCompras();
            await SincronizacionCheques();
        }
        #region PROVEEDORES
        private async Task SincronizacionProveedores()
        {
            var documentos = await CrossCloudFirestore.Current
                                         .Instance
                                         .Collection("PROVEEDOR")
                                         .GetAsync();
            var proveedoresFirebase = documentos.ToObjects<Proveedor>().ToList();
            List<Tbl_Proveedor> proveedoresLocal = await _repoTblProveedor.ObtenerTblProveedoresAsync();

            List<Tbl_Proveedor> proveedoresLocalesParaAgregar = new List<Tbl_Proveedor>();
            List<Proveedor> proveedoresFirebaseParaAgregar = new List<Proveedor>();

            // Agregar proveedores de Firebase a la lista local
            foreach (var proveedor in proveedoresFirebase)
            {
                if (!proveedoresLocal.Any(pl => pl.PROVEEDOR == proveedor.PROVEEDOR))
                {
                    proveedoresLocalesParaAgregar.Add(new Tbl_Proveedor { PROVEEDOR = proveedor.PROVEEDOR });
                }
            }
            // Agregar proveedores de la lista local a Firebase
            foreach (var proveedor in proveedoresLocal)
            {
                if (!proveedoresFirebase.Any(pf => pf.PROVEEDOR == proveedor.PROVEEDOR))
                {
                    proveedoresFirebaseParaAgregar.Add(new Proveedor { PROVEEDOR = proveedor.PROVEEDOR });
                }
            }

            foreach (var p in proveedoresFirebaseParaAgregar)
            {
                await _repoProveedor.GuardarNuevoProveedorAsync(p);
            }
            foreach (var p in proveedoresLocalesParaAgregar)
            {
                Proveedor proveedor = new()
                {
                    PROVEEDOR = p.PROVEEDOR
                };
                await _repoTblProveedor.GuardarNuevoProveedorAsync(proveedor);
            }
        }
        #endregion

        #region HISTORIALCOMPRAS
        private async Task SincronizacionHistorialCompras()
        {
            var documentos = await CrossCloudFirestore.Current
                                         .Instance
                                         .Collection("HISTORIALCOMPRAS")
                                         .GetAsync();
            var historialComprasFirebase = documentos.ToObjects<HistorialCompras>().ToList();
            List<Tbl_HistorialCompras> historialCompraLocal = await _repoTblHistorialCompras.ObtenerTodoHistorialProductosAsync();

            List<Tbl_HistorialCompras> historialComprasLocalParaAgregar = new List<Tbl_HistorialCompras>();
            List<HistorialCompras> historialComprasFirebaseParaAgregar = new List<HistorialCompras>();

            // Agregar historial compras de Firebase a la lista local
            foreach (var historialCompras in historialComprasFirebase)
            {
                if (!historialCompraLocal.Any(hcl =>
                    hcl.PRODUCTO == historialCompras.PRODUCTO &&
                    hcl.DIAFECHA == historialCompras.DIAFECHA &&
                    hcl.ABONO == historialCompras.ABONO &&
                    hcl.CANTIDAD == historialCompras.CANTIDAD &&
                    hcl.FECHA == historialCompras.FECHA &&
                    hcl.MEDIDA == historialCompras.MEDIDA &&
                    hcl.PRECIO == historialCompras.PRECIO &&
                    hcl.PROVEEDOR == historialCompras.PROVEEDOR &&
                    hcl.SALDOPENDIENTE == historialCompras.SALDOPENDIENTE &&
                    hcl.TOTAL == historialCompras.TOTAL
                ))
                {
                    historialComprasLocalParaAgregar.Add(new Tbl_HistorialCompras 
                    {
                        PRODUCTO = historialCompras.PRODUCTO,
                        DIAFECHA = historialCompras.DIAFECHA,
                        ABONO = historialCompras.ABONO,
                        CANTIDAD = historialCompras.CANTIDAD,
                        FECHA = historialCompras.FECHA,
                        MEDIDA = historialCompras.MEDIDA,
                        PRECIO = historialCompras.PRECIO,
                        PROVEEDOR = historialCompras.PROVEEDOR,
                        SALDOPENDIENTE = historialCompras.SALDOPENDIENTE,
                        TOTAL = historialCompras.TOTAL
                    });
                }
            }
            // Agregar proveedores de la lista local a Firebase
            foreach (var historialCompras in historialCompraLocal)
            {
                if (!historialComprasFirebase.Any(hcf =>
                 hcf.PRODUCTO == historialCompras.PRODUCTO &&
                    hcf.DIAFECHA == historialCompras.DIAFECHA &&
                    hcf.ABONO == historialCompras.ABONO &&
                    hcf.CANTIDAD == historialCompras.CANTIDAD &&
                    hcf.FECHA == historialCompras.FECHA &&
                    hcf.MEDIDA == historialCompras.MEDIDA &&
                    hcf.PRECIO == historialCompras.PRECIO &&
                    hcf.PROVEEDOR == historialCompras.PROVEEDOR &&
                    hcf.SALDOPENDIENTE == historialCompras.SALDOPENDIENTE &&
                    hcf.TOTAL == historialCompras.TOTAL
                ))
                {
                    historialComprasFirebaseParaAgregar.Add(new HistorialCompras
                    {
                        PRODUCTO = historialCompras.PRODUCTO,
                        DIAFECHA = historialCompras.DIAFECHA,
                        ABONO = historialCompras.ABONO,
                        CANTIDAD = historialCompras.CANTIDAD,
                        FECHA = historialCompras.FECHA,
                        MEDIDA = historialCompras.MEDIDA,
                        PRECIO = historialCompras.PRECIO,
                        PROVEEDOR = historialCompras.PROVEEDOR,
                        SALDOPENDIENTE = historialCompras.SALDOPENDIENTE,
                        TOTAL = historialCompras.TOTAL
                    });
                }
            }

            foreach (var p in historialComprasFirebaseParaAgregar)
            {
                await _repoHistorialCompras.GuardarRegistroProductoAsync(p);
            }
            foreach (var hc in historialComprasLocalParaAgregar)
            {
                HistorialCompras historialCompra = new()
                {
                    PRODUCTO = hc.PRODUCTO,
                    DIAFECHA = hc.DIAFECHA,
                    ABONO = hc.ABONO,
                    CANTIDAD = hc.CANTIDAD,
                    FECHA = hc.FECHA,
                    MEDIDA = hc.MEDIDA,
                    PRECIO = hc.PRECIO,
                    PROVEEDOR = hc.PROVEEDOR,
                    SALDOPENDIENTE = hc.SALDOPENDIENTE,
                    TOTAL = hc.TOTAL
                };
                await _repoTblHistorialCompras.GuardarRegistroProductoAsync(historialCompra);
            }
        }
        #endregion

        #region CHEQUES
        private async Task SincronizacionCheques()
        {
            var documentos = await CrossCloudFirestore.Current
                                         .Instance
                                         .Collection("CHEQUE")
                                         .GetAsync();
            var chequesFirebase = documentos.ToObjects<Cheque>().ToList();
            List<Tbl_Cheque> chequesLocal = await _repoCheque.ObtenerChequesAsync();

            List<Tbl_Cheque> chequesLocalParaAgregar = new List<Tbl_Cheque>();
            List<Cheque> chequesFirebaseParaAgregar = new List<Cheque>();

            // Agregar cheques de Firebase a la lista local
            foreach (var cheque in chequesFirebase)
            {
                if (!chequesLocal.Any(cl => cl.NUMERO == cheque.NUMERO))
                {
                    chequesLocalParaAgregar.Add(new Tbl_Cheque
                    {
                        NUMERO = cheque.NUMERO,
                        MONTO = cheque.MONTO,
                        PROVEEDOR = cheque.PROVEEDOR,
                        FECHACOBRO = cheque.FECHACOBRO,
                        FECHAEMISION = cheque.FECHAEMISION,
                        DIAFECHACOBRO = cheque.DIAFECHACOBRO,
                    });
                }
            }
            // Agregar proveedores de la lista local a Firebase
            foreach (var cheque in chequesLocal)
            {
                if (!chequesFirebase.Any(cf => cf.NUMERO == cheque.NUMERO))
                {
                    chequesFirebaseParaAgregar.Add(new Cheque 
                    {
                        NUMERO = cheque.NUMERO,
                        MONTO = cheque.MONTO,
                        PROVEEDOR = cheque.PROVEEDOR,
                        FECHACOBRO = cheque.FECHACOBRO,
                        FECHAEMISION = cheque.FECHAEMISION,
                        DIAFECHACOBRO = cheque.DIAFECHACOBRO,
                    });
                }
            }

            foreach (var p in chequesFirebaseParaAgregar)
            {
                await _repoCheque.GuardarNuevoResgitroChequesAsync(p);
            }
            foreach (var c in chequesLocalParaAgregar)
            {
                Cheque cheque = new()
                {
                    NUMERO = c.NUMERO,
                    MONTO = c.MONTO,
                    PROVEEDOR = c.PROVEEDOR,
                    FECHACOBRO = c.FECHACOBRO,
                    FECHAEMISION = c.FECHAEMISION,
                    DIAFECHACOBRO = c.DIAFECHACOBRO,
                };
                await _repoTblCheque.GuardarChequeAsync(cheque);
            }
        }
        #endregion



    }
}
