using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole1.Models
{
    public class APInvoice
    {
        public int APInvoiceID { get; set; }
        public string Record_Number { get; set; }
        public DateTime Invoice_Date { get; set; }
        public string Invoice_Number { get; set; }
        public string Vendor_Number { get; set; }
        public string Vendor_Name { get; set; }
        public string PO_Number { get; set; }
        public string Invoice_Type_cd { get; set; }
    }
}