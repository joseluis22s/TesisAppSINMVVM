﻿using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesisAppSINMVVM.LocalDatabase.Respositories;
using TesisAppSINMVVM.LocalDatabase.Tables;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.FirebaseDataBase.Repositories
{
    public class ProductoComprado_Repository
    {
        private Tbl_ProductoComprado_Repository _repoTblProductoComprado = new Tbl_ProductoComprado_Repository();

        public ProductoComprado_Repository() { }

        #region ESCRITURA
        public async Task GuardarNuevaCompraProductoCompradoAsync(ProductoComprado productoComprado)
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                await CrossCloudFirestore.Current
                             .Instance
                             .Collection("PRODUCTOCOMPRADO")
                             .AddAsync(productoComprado);
                await _repoTblProductoComprado.GuardarNuevaCompraProductoCompradoAsync(productoComprado);
            }
            else
            {
                await _repoTblProductoComprado.GuardarNuevaCompraProductoCompradoAsync(productoComprado);
            }
        }
        #endregion

        #region LECTURA
        public async Task<List<int>> ObtenerNumerosComprasAsync(string nombreProveedor)
        {
            var documentos = await CrossCloudFirestore.Current
                                        .Instance
                                        .Collection("PRODUCTOCOMPRADO")
                                        .WhereEqualsTo("PROVEEDOR", nombreProveedor)
                                        .GetAsync();
            var productosCompradosFirebase = documentos.ToObjects<ProductoComprado>().ToList();

            List<Tbl_ProductoComprado> productosCompradosLocal = await _repoTblProductoComprado.ObtenerProductosCompradosAsync(nombreProveedor);
            List<int> numerosCompraLocal = productosCompradosLocal.Select(p => p.NUMEROCOMPRA).Distinct().ToList();
            List<int> numerosCompraFirebase = productosCompradosFirebase.Select(p => p.NUMEROCOMPRA).Distinct().ToList();

            List<int> numerosCompra = numerosCompraFirebase.Except(numerosCompraLocal).ToList();
            numerosCompra.Sort((x, y) => y.CompareTo(x));
            return numerosCompra;
            


        }
        public async Task<List<Tbl_ProductoComprado>> ObtenerProductosCompradosAsync(string nombreProveedor)
        {
            var documentos = await CrossCloudFirestore.Current
                                        .Instance
                                        .Collection("PRODUCTOCOMPRADO")
                                         .WhereEqualsTo("PROVEEDOR", nombreProveedor)
                                        .GetAsync();
            var productosCompradosFirebase = documentos.ToObjects<ProductoComprado>().ToList();

            List<Tbl_ProductoComprado> productosCompradosLocal = await _repoTblProductoComprado.ObtenerProductosCompradosAsync(nombreProveedor);
            List<Tbl_ProductoComprado> productosComprados = new List<Tbl_ProductoComprado>(productosCompradosLocal);

            var productosCompradosToAdd = productosCompradosFirebase.Select(p => new Tbl_ProductoComprado
            {
                NUMEROCOMPRA = p.NUMEROCOMPRA,
                PRODUCTO = p.PRODUCTO,
                MEDIDA = p.MEDIDA,
                CANTIDAD = p.CANTIDAD,
                PRECIO = p.PRECIO,
                TOTAL = p.TOTAL,
                FECHAGUARDADO = p.FECHAGUARDADO,
                DIAFECHAGUARDADO = p.DIAFECHAGUARDADO,
                PROVEEDOR = p.PROVEEDOR
            })
            .Where(p => !productosCompradosLocal.Any(pl => pl.PRODUCTO == p.PRODUCTO));

            productosComprados.AddRange(productosCompradosToAdd);
            return productosComprados;
        }
        #endregion

    }
}