using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole1.Models
{
    public class ARCustRemit
    {
        public int ARCustRemitID { get; set; }
        public string Record_Number { get; set; }
        public DateTime Chk_Deposit_Dt { get; set; }
        public string Chk_Serial_Num { get; set; }
        public string Chk_Transit_Num { get; set; }
        public string Chk_Account_Num { get; set; }
        public string Lockbox { get; set; }
        public string Has_Action_Item { get; set; }
        public string Lawson_Customer { get; set; }
    }
}