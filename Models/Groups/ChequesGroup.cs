using TesisAppSINMVVM.LocalDatabase.Tables;

namespace TesisAppSINMVVM.Models
{
    public class ChequesGroup : List<Tbl_Cheque>
    {
        public string DiaFecha { get; private set; }
        public ChequesGroup(string diaFecha, List<Tbl_Cheque> cheques)  : base (cheques)
        {
            DiaFecha = diaFecha;
        }
    }
}
