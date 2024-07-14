using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesisAppSINMVVM.Database.Respositories;
using TesisAppSINMVVM.LocalDatabase.Tables;
using TesisAppSINMVVM.Models;

namespace TesisAppSINMVVM.FirebaseDataBase.Repositories
{
    public class Usuario_Repository
    {
        private Tbl_Usuario_Repository _repoTblUsuario = new Tbl_Usuario_Repository(); 

        public Usuario_Repository() { }

        #region ESCRITURA
        public async Task GuardarNuevoUsuarioAsync(Usuario usuario)
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                await CrossCloudFirestore.Current
                         .Instance
                         .Collection("USUARIO")
                         .AddAsync(usuario);
                await _repoTblUsuario.GuardarNuevoUsuarioAsync(usuario.USUARIO, usuario.CONTRASENA,usuario.CORREO);
            }
            //else
            //{
            //    await _repoTblUsuario.GuardarNuevoUsuarioAsync(usuario.USUARIO, usuario.CONTRASENA, usuario.CORREO);
            //}
        }
        #endregion

        #region LECTURA
        public async Task<List<Tbl_Usuario>> ObtenerUsuariosAsync()
        {
            var documentos = await CrossCloudFirestore.Current
                                         .Instance
                                         .Collection("USUARIO")
                                         .GetAsync();
            var usuariosFirebase = documentos.ToObjects<Usuario>().ToList();

            List<Tbl_Usuario> usuariosLocal = await _repoTblUsuario.ObtenerUsuariosAsync();
            List<Tbl_Usuario> usuarios = new List<Tbl_Usuario>(usuariosLocal);

            var usuariosToAdd = usuariosFirebase
                .Select(u => new Tbl_Usuario
                {
                    USUARIO = u.USUARIO,
                    CONTRASENA = u.CONTRASENA,
                    CORREO = u.CORREO
                })
                .Where(p => !usuariosLocal.Any(ul => 
                    ul.USUARIO == p.USUARIO &&
                    ul.CONTRASENA == p.CONTRASENA &&
                    ul.CORREO == p.CORREO
                ));

            usuarios.AddRange(usuariosToAdd);

            return usuarios;
        }
        #endregion

    }
}
