
using TesisAppSINMVVM.Database.Tables;

namespace TesisAppSINMVVM.Models
{
    public class ChequesGroup : List<Cheque>
    {
        public string DiaFecha { get; private set; }
        public ChequesGroup(string daFecha, List<Cheque> cheques)  : base (cheques)
        {
            DiaFecha = daFecha;
        }
    }
}
