namespace TesisAppSINMVVM.Models.Groups
{
    public class HistorialComprasGroup : List<HistorialCompras>
    {
        public string NombreGrupo { get; private set; }
        public HistorialComprasGroup(string nombreGrupo, List<HistorialCompras> listaHistorialCompras) : base(listaHistorialCompras)
        {
            NombreGrupo = nombreGrupo;
        }
    }
}
