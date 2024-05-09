
namespace TesisAppSINMVVM.Models
{
    public class CompraGroupModel : List<CompraModel>
    {
        public string NombreGrupo { get; private set; }
        public CompraGroupModel(string nombreGrupo, List<CompraModel> listaCompras)  : base(listaCompras)
        {
            NombreGrupo = nombreGrupo;
        }
    }
}
