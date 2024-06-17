
using TesisAppSINMVVM.Database.Tables;

namespace TesisAppSINMVVM.Models
{
    public class ChequeGroup : List<Tbl_Cheque>
    {
        public string DiaFecha { get; private set; }
        public ChequeGroup(string daFecha, List<Tbl_Cheque> cheques)  : base (cheques)
        {
            DiaFecha = daFecha;
        }
    }
}
