using Plugin.CloudFirestore;
using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.FirebaseDataBase.Repositories;
using TesisAppSINMVVM.LocalDatabase.Tables;
using TesisAppSINMVVM.Models;

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
        private Comprador_Repository _repoComprador = new Comprador_Repository();
        private Tbl_Comprador_Repository _repoTblComprador = new Tbl_Comprador_Repository();
        private Producto_Repository _repoProducto = new Producto_Repository();
        private Tbl_Producto_Repository _repoTblProducto = new Tbl_Producto_Repository();
        private ProductoInventarioBodega_Repository _repoProductoInventarioBodega = new ProductoInventarioBodega_Repository();
        private Tbl_ProductosInventario_Repository _repoTblProductosInventario = new Tbl_ProductosInventario_Repository();
        //private Usuario_Repository _repoUsuario = new Usuario_Repository();
        private Tbl_Usuario_Repository _repoTblUsuario = new Tbl_Usuario_Repository();
        private VentaCredito_Repository _repoVentaCredito = new VentaCredito_Repository();
        private Tbl_VentaCredito_Repository _repoTblVentaCredito = new Tbl_VentaCredito_Repository();
        

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
            await SincronizacionCheques();
            await SincronizacionCompradores();
            await SincronizacionHistorialCompras();
            await SincronizacionProductos();
            await SincronizacionProductosInventario();
            await SincronizacionProveedores();

            await SincronizacionVentaCredito();
        }
        #region CHEQUES
        private async Task SincronizacionCheques()
        {
            var documentos = await CrossCloudFirestore.Current
                                         .Instance
                                         .Collection("CHEQUE")
                                         .GetAsync();
            var chequesFirebase = documentos.ToObjects<Cheque>().ToList();
            List<Tbl_Cheque> chequesLocal = await _repoTblCheque.ObtenerChequesAsync();

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

        #region COMPRADORES
        private async Task SincronizacionCompradores()
        {
            var documentos = await CrossCloudFirestore.Current
                                         .Instance
                                         .Collection("COMPRADOR")
                                         .GetAsync();
            var compradoresFirebase = documentos.ToObjects<Comprador>().ToList();
            List<Tbl_Comprador> compradoresLocal = await _repoTblComprador.ObtenerTblCompradoresAsync();

            List<Tbl_Comprador> compradoresLocalParaAgregar = new List<Tbl_Comprador>();
            List<Comprador> compradoresFirebaseParaAgregar = new List<Comprador>();

            // Agregar compradores de Firebase a la lista local
            foreach (var c in compradoresFirebase)
            {
                if (!compradoresLocal.Any(cl => cl.COMPRADOR == c.COMPRADOR))
                {
                    compradoresLocalParaAgregar.Add(new Tbl_Comprador { COMPRADOR = c.COMPRADOR });
                }
            }
            // Agregar proveedores de la lista local a Firebase
            foreach (var c in compradoresLocal)
            {
                if (!compradoresFirebase.Any(cf => cf.COMPRADOR == c.COMPRADOR))
                {
                    compradoresFirebaseParaAgregar.Add(new Comprador { COMPRADOR = c.COMPRADOR });
                }
            }

            foreach (var c in compradoresFirebaseParaAgregar)
            {
                await _repoComprador.GuardarNuevoCompradorAsync(c);
            }
            foreach (var c in compradoresLocalParaAgregar)
            {
                Comprador comprador = new()
                {
                    COMPRADOR = c.COMPRADOR
                };
                await _repoTblComprador.GuardarCompradorAsync(comprador);
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
            // Agregar historialCompras de la lista local a Firebase
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

        #region PRODUCTO
        private async Task SincronizacionProductos()
        {
            var documentos = await CrossCloudFirestore.Current
                                         .Instance
                                         .Collection("PRODUCTO")
                                         .GetAsync();
            var productosFirebase = documentos.ToObjects<Producto>().ToList();
            List<Tbl_Producto> productosLocal = await _repoTblProducto.ObtenerProductosAsync();

            List<Tbl_Producto> productosLocalParaAgregar = new List<Tbl_Producto>();
            List<Producto> productosFirebaseParaAgregar = new List<Producto>();

            // Agregar proveedores de Firebase a la lista local
            foreach (var producto in productosFirebase)
            {
                if (!productosLocal.Any(pl =>
                    pl.PRODUCTO == producto.PRODUCTO &&
                    pl.MEDIDA == producto.MEDIDA
                ))
                {
                    productosLocalParaAgregar.Add(new Tbl_Producto
                    {
                        PRODUCTO = producto.PRODUCTO,
                        MEDIDA = producto.MEDIDA
                    });
                }
            }
            // Agregar proveedores de la lista local a Firebase
            foreach (var producto in productosLocal)
            {
                if (!productosFirebase.Any(pf =>
                    pf.PRODUCTO == producto.PRODUCTO &&
                    pf.MEDIDA == producto.MEDIDA
                ))
                {
                    productosFirebaseParaAgregar.Add(new Producto
                    {
                        PRODUCTO = producto.PRODUCTO,
                        MEDIDA = producto.MEDIDA
                    });
                }
            }

            foreach (var p in productosFirebaseParaAgregar)
            {
                await _repoProducto.GuardarNuevoProductoAsync(p);
            }
            foreach (var p in productosLocalParaAgregar)
            {
                Producto producto = new()
                {
                        PRODUCTO = p.PRODUCTO,
                        MEDIDA = p.MEDIDA
                };
                await _repoTblProducto.GuardarProductoAsync(producto);
            }
        }
        #endregion

        #region PRODUCTOSINVENTARIO
        private async Task SincronizacionProductosInventario()
        {
            var documentos = await CrossCloudFirestore.Current
                                         .Instance
                                         .Collection("PRODUCTOSINVENTARIO")
                                         .GetAsync();
            var productosinventarioFirebase = documentos.ToObjects<ProductoInventarioBodega>().ToList();
            List<Tbl_ProductosInventario> productosinventarioLocal = await _repoTblProductosInventario.ObtenerInvetarioAsync();

            List<Tbl_ProductosInventario> productosinventarioLocalParaAgregar = new List<Tbl_ProductosInventario>();
            List<ProductoInventarioBodega> productosinventarioFirebaseParaAgregar = new List<ProductoInventarioBodega>();

            // Agregar productos inventario de Firebase a la lista local
            foreach (var productosinventario in productosinventarioFirebase)
            {
                if (!productosinventarioLocal.Any(pil =>
                    pil.PRODUCTO == productosinventario.PRODUCTO &&
                    pil.MEDIDA == productosinventario.MEDIDA &&
                    pil.CANTIDAD == productosinventario.CANTIDAD &&
                    pil.DESCRIPCION == productosinventario.DESCRIPCION &&
                    pil.FECHAGUARDADO == productosinventario.FECHAGUARDADO &&
                    pil.DIAFECHAGUARDADO == productosinventario.DIAFECHAGUARDADO
                ))
                {
                    productosinventarioLocalParaAgregar.Add(new Tbl_ProductosInventario
                    {
                        PRODUCTO = productosinventario.PRODUCTO,
                        MEDIDA = productosinventario.MEDIDA,
                        CANTIDAD = productosinventario.CANTIDAD,
                        DESCRIPCION = productosinventario.DESCRIPCION,
                        FECHAGUARDADO = productosinventario.FECHAGUARDADO,
                        DIAFECHAGUARDADO = productosinventario.DIAFECHAGUARDADO
                    });
                }
            }
            // Agregar proveedores de la lista local a Firebase
            foreach (var productosinventario in productosinventarioLocal)
            {
                if (!productosinventarioFirebase.Any(pif =>
                    pif.PRODUCTO == productosinventario.PRODUCTO &&
                    pif.MEDIDA == productosinventario.MEDIDA &&
                    pif.CANTIDAD == productosinventario.CANTIDAD &&
                    pif.DESCRIPCION == productosinventario.DESCRIPCION &&
                    pif.FECHAGUARDADO == productosinventario.FECHAGUARDADO &&
                    pif.DIAFECHAGUARDADO == productosinventario.DIAFECHAGUARDADO
                ))
                {
                    productosinventarioFirebaseParaAgregar.Add(new ProductoInventarioBodega
                    {
                        PRODUCTO = productosinventario.PRODUCTO,
                        MEDIDA = productosinventario.MEDIDA,
                        CANTIDAD = productosinventario.CANTIDAD,
                        DESCRIPCION = productosinventario.DESCRIPCION,
                        FECHAGUARDADO = productosinventario.FECHAGUARDADO,
                        DIAFECHAGUARDADO = productosinventario.DIAFECHAGUARDADO
                    });
                }
            }

            foreach (var p in productosinventarioFirebaseParaAgregar)
            {
                await _repoProductoInventarioBodega.GuardarProductosInventarioAsync(p);
            }
            foreach (var pIB in productosinventarioLocalParaAgregar)
            {
                ProductoInventarioBodega productosinventario = new()
                {
                    PRODUCTO = pIB.PRODUCTO,
                    MEDIDA = pIB.MEDIDA,
                    CANTIDAD = pIB.CANTIDAD,
                    DESCRIPCION = pIB.DESCRIPCION,
                    FECHAGUARDADO = pIB.FECHAGUARDADO,
                    DIAFECHAGUARDADO = pIB.DIAFECHAGUARDADO
                };
                await _repoTblProductosInventario.GuardarProductosInventarioAsync(productosinventario);
            }
        }
        #endregion

        #region PROVEEDORES
        private async Task SincronizacionProveedores()
        {
            var documentos = await CrossCloudFirestore.Current
                                         .Instance
                                         .Collection("PROVEEDOR")
                                         .GetAsync();
            var proveedoresFirebase = documentos.ToObjects<Proveedor>().ToList();
            List<Tbl_Proveedor> proveedoresLocal = await _repoProveedor.ObtenerProveedoresAsync();

            List<Tbl_Proveedor> proveedoresLocalParaAgregar = new List<Tbl_Proveedor>();
            List<Proveedor> proveedoresFirebaseParaAgregar = new List<Proveedor>();

            // Agregar proveedores de Firebase a la lista local
            foreach (var proveedor in proveedoresFirebase)
            {
                if (!proveedoresLocal.Any(pl => pl.PROVEEDOR == proveedor.PROVEEDOR))
                {
                    proveedoresLocalParaAgregar.Add(new Tbl_Proveedor { PROVEEDOR = proveedor.PROVEEDOR });
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
            foreach (var p in proveedoresLocalParaAgregar)
            {
                Proveedor proveedor = new()
                {
                    PROVEEDOR = p.PROVEEDOR
                };
                await _repoTblProveedor.GuardarNuevoProveedorAsync(proveedor);
            }
        }
        #endregion


        #region VENTACREDITO
        private async Task SincronizacionVentaCredito()
        {
            var documentos = await CrossCloudFirestore.Current
                                         .Instance
                                         .Collection("VENTACREDITO")
                                         .GetAsync();
            var ventasCreditoFirebase = documentos.ToObjects<VentaCredito>().ToList();
            List<Tbl_VentaCredito> ventasCreditoLocal = await _repoTblVentaCredito.ObtenerVentasCreditoAsync();

            List<Tbl_VentaCredito> ventasCreditoLocalParaAgregar = new List<Tbl_VentaCredito>();
            List<VentaCredito> ventasCreditoFirebaseParaAgregar = new List<VentaCredito>();

            // Agregar ventas credito de Firebase a la lista local
            foreach (var ventaCredito in ventasCreditoFirebase)
            {
                if (!ventasCreditoLocal.Any(vcl =>
                    vcl.COMPRADOR == ventaCredito.COMPRADOR &&
                    vcl.MONTOVENDIDO == ventaCredito.MONTOVENDIDO &&
                    vcl.DESCRIPCION == ventaCredito.DESCRIPCION &&
                    vcl.FECHAGUARDADO == ventaCredito.FECHAGUARDADO &&
                    vcl.DIAFECHAGUARDADO == ventaCredito.DIAFECHAGUARDADO
                ))
                {
                    ventasCreditoLocalParaAgregar.Add(new Tbl_VentaCredito
                    {
                        COMPRADOR = ventaCredito.COMPRADOR,
                        MONTOVENDIDO = ventaCredito.MONTOVENDIDO,
                        DESCRIPCION = ventaCredito.DESCRIPCION,
                        FECHAGUARDADO = ventaCredito.FECHAGUARDADO,
                        DIAFECHAGUARDADO = ventaCredito.DIAFECHAGUARDADO
                    });
                }
            }

            // Agregar ventas credito de la lista local a Firebase
            foreach (var ventaCredito in ventasCreditoLocal)
            {
                if (!ventasCreditoFirebase.Any(vcf =>
                    vcf.COMPRADOR == ventaCredito.COMPRADOR &&
                    vcf.MONTOVENDIDO == ventaCredito.MONTOVENDIDO &&
                    vcf.DESCRIPCION == ventaCredito.DESCRIPCION &&
                    vcf.FECHAGUARDADO == ventaCredito.FECHAGUARDADO &&
                    vcf.DIAFECHAGUARDADO == ventaCredito.DIAFECHAGUARDADO
                ))
                {
                    ventasCreditoFirebaseParaAgregar.Add(new VentaCredito
                    {
                        COMPRADOR = ventaCredito.COMPRADOR,
                        MONTOVENDIDO = ventaCredito.MONTOVENDIDO,
                        DESCRIPCION = ventaCredito.DESCRIPCION,
                        FECHAGUARDADO = ventaCredito.FECHAGUARDADO,
                        DIAFECHAGUARDADO = ventaCredito.DIAFECHAGUARDADO
                    });

                    foreach (var v in ventasCreditoFirebaseParaAgregar)
                    {
                        await _repoVentaCredito.GuardarVentaCreditoAsync(v);
                    }
                    foreach (var vc in ventasCreditoLocalParaAgregar)
                    {
                        VentaCredito ventacredito = new()
                        {
                            COMPRADOR = vc.COMPRADOR,
                            MONTOVENDIDO = vc.MONTOVENDIDO,
                            DESCRIPCION = vc.DESCRIPCION,
                            FECHAGUARDADO = vc.FECHAGUARDADO,
                            DIAFECHAGUARDADO = vc.DIAFECHAGUARDADO
                        };
                        await _repoTblVentaCredito.GuardarVentaCreditoAsync(ventacredito);
                    }
                }
            }
        }
        #endregion










    }
}
