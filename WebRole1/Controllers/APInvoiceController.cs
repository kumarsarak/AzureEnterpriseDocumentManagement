using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebRole1.Models;
using System.Diagnostics;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using PagedList.Mvc;
using PagedList;

namespace WebRole1.Controllers
{
    [EdmAuthorize("APAdmin", "APUser")]
    public class APInvoiceController : Controller
    {
        private APAppDBContext db = new APAppDBContext();

        public ViewResult Index(string recordnumber, string invoicedate, string toinvoicedate, string invoicenumber, string vendornumber, string vendorname, string ponumber, string invoicetypecd, string sortOrder, int? page)
        {

            if (NoParams(recordnumber, invoicedate, toinvoicedate, invoicenumber, vendornumber, vendorname, ponumber, invoicetypecd))
            {
                return NoParamSearch(sortOrder, page);
            }
            else
            {
                return SearchLink(recordnumber, invoicedate, toinvoicedate, invoicenumber, vendornumber, vendorname, ponumber, invoicetypecd, sortOrder, page);
                
            }

        }

        private bool NoParams(string recordnumber, string invoicedate, string toinvoicedate, string invoicenumber, string vendornumber, string vendorname, string ponumber, string invoicetypecd)
        {
            if (String.IsNullOrEmpty(recordnumber) && String.IsNullOrEmpty(invoicedate) && String.IsNullOrEmpty(toinvoicedate) && String.IsNullOrEmpty(invoicenumber) && String.IsNullOrEmpty(vendornumber) && String.IsNullOrEmpty(vendorname) && String.IsNullOrEmpty(ponumber) && String.IsNullOrEmpty(invoicetypecd))
                return true;
            else
                return false;
        }

        private ViewResult NoParamSearch(string sortOrder, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date" : "";
            ViewBag.VendorNameSortParm = sortOrder == "vendor" ? "vendor_desc" : "vendor";
            ViewBag.InvoiceTypeSortParm = sortOrder == "invoicetype" ? "invoicetype_desc" : "invoicetype";
            var apinvoices = db.APInvoices.OrderByDescending(a => a.Invoice_Date).Take(100);


            switch (sortOrder)
            {
                case "vendor_desc":
                    apinvoices = apinvoices.OrderByDescending(s => s.Vendor_Name);
                    break;
                case "vendor":
                    apinvoices = apinvoices.OrderBy(s => s.Vendor_Name);
                    break;
                case "invoicetype_desc":
                    apinvoices = apinvoices.OrderByDescending(s => s.Invoice_Type_cd);
                    break;
                case "invoicetype":
                    apinvoices = apinvoices.OrderBy(s => s.Invoice_Type_cd);
                    break;
                case "date":
                    apinvoices = apinvoices.OrderBy(s => s.Invoice_Date);
                    break;
                default:
                    apinvoices = apinvoices.OrderByDescending(a => a.Invoice_Date);
                    break;
            }

            int pageSize = 30;
            int pageNumber = (page ?? 1);


            return View(apinvoices.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public async Task<PartialViewResult> Search(string recordnumber, string invoicedate, string toinvoicedate, string invoicenumber, string vendornumber, string vendorname, string ponumber, string invoicetypecd, string sortOrder, int? page)
        {
            ViewBag.Currentrecordnumber = recordnumber;
            ViewBag.Currentinvoicedate = invoicedate;
            ViewBag.Currenttoinvoicedate = toinvoicedate;
            ViewBag.Currentinvoicenumber = invoicenumber;
            ViewBag.Currentvendornumber = vendornumber;
            ViewBag.Currentvendorname = vendorname;
            ViewBag.Currentponumber = ponumber;
            ViewBag.Currentinvoicetypecd = invoicetypecd;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date" : "";
            ViewBag.VendorNameSortParm = sortOrder == "vendor" ? "vendor_desc" : "vendor";
            ViewBag.InvoiceTypeSortParm = sortOrder == "invoicetype" ? "invoicetype_desc" : "invoicetype";
            DateTime toinvoicedateparse; DateTime invoicedateparse;
           // if (!String.IsNullOrEmpty(toinvoicedate)) {toinvoicedateparse = Convert.ToDateTime(toinvoicedate);}
           // if (!String.IsNullOrEmpty(toinvoicedate)) { invoicedateparse = Convert.ToDateTime(invoicedate); }


            page = 1;
            var apinvoices = from m in db.APInvoices select m;
           
            
            
            
            if (!String.IsNullOrEmpty(recordnumber))
            {
                apinvoices = apinvoices.Where(a => a.Record_Number.Contains(recordnumber));
            }

            

            if (!String.IsNullOrEmpty(invoicenumber))
            {
                apinvoices = apinvoices.Where(c => c.Invoice_Number.Contains(invoicenumber));
            }

            if (!String.IsNullOrEmpty(vendornumber))
            {
                apinvoices = apinvoices.Where(d => d.Vendor_Number.Contains(vendornumber));
            }

            if (!String.IsNullOrEmpty(vendorname))
            {
                apinvoices = apinvoices.Where(e => e.Vendor_Name.Contains(vendorname));
            }

            if (!String.IsNullOrEmpty(ponumber))
            {
                apinvoices = apinvoices.Where(f => f.PO_Number.Contains(ponumber));
            }

            if (!String.IsNullOrEmpty(invoicetypecd))
            {
                apinvoices = apinvoices.Where(g => g.Invoice_Type_cd.Contains(invoicetypecd));
            }

            if ((!string.IsNullOrEmpty(invoicedate)) && (string.IsNullOrEmpty(toinvoicedate)))
            {
                invoicedateparse = Convert.ToDateTime(invoicedate);
                apinvoices = apinvoices.Where(b => b.Invoice_Date.Year >= invoicedateparse.Year && b.Invoice_Date.Month >= invoicedateparse.Month && b.Invoice_Date.Day >= invoicedateparse.Day);
            }

            if ((string.IsNullOrEmpty(invoicedate)) && (!string.IsNullOrEmpty(toinvoicedate)))
            {
                toinvoicedateparse = Convert.ToDateTime(toinvoicedate);
                apinvoices = apinvoices.Where(b => b.Invoice_Date.Year <= toinvoicedateparse.Year && b.Invoice_Date.Month <= toinvoicedateparse.Month && b.Invoice_Date.Day <= toinvoicedateparse.Day);
            }

            if ((!string.IsNullOrEmpty(invoicedate)) && (!string.IsNullOrEmpty(toinvoicedate)))
            {
                toinvoicedateparse = Convert.ToDateTime(toinvoicedate);
                invoicedateparse = Convert.ToDateTime(invoicedate);
                apinvoices = apinvoices.Where(b => (b.Invoice_Date >= invoicedateparse.Date));
                apinvoices = apinvoices.Where(b => (b.Invoice_Date <= toinvoicedateparse.Date));
            }

            if (apinvoices.Count() > 100)
            {
                ViewBag.TotalAPRecords = "Search Criteria returned " + apinvoices.Count() + " records. Only the first 100 records are displayed. Please narrow your search criteria.";
                apinvoices = apinvoices.OrderByDescending(a => a.Invoice_Date).Take(100);
            }
            else
            {
                ViewBag.TotalAPRecords = "Your search criteria returned " + apinvoices.Count() + " record(s).";
            }
            switch (sortOrder)
            {
                case "vendor_desc":
                    apinvoices = apinvoices.OrderByDescending(s => s.Vendor_Name);
                    break;
                case "vendor":
                    apinvoices = apinvoices.OrderBy(s => s.Vendor_Name);
                    break;
                case "invoicetype_desc":
                    apinvoices = apinvoices.OrderByDescending(s => s.Invoice_Type_cd);
                    break;
                case "invoicetype":
                    apinvoices = apinvoices.OrderBy(s => s.Invoice_Type_cd);
                    break;
                case "date":
                    apinvoices = apinvoices.OrderBy(s => s.Invoice_Date);
                    break;
                default:
                    apinvoices = apinvoices.OrderByDescending(a => a.Invoice_Date);
                    break;
            }

            int pageSize = 30;
            int pageNumber = (page ?? 1);


            //return PartialView("_AP", await apinvoices.ToListAsync());
            return PartialView("_AP", apinvoices.ToPagedList(pageNumber, pageSize));

        }

        public ViewResult SearchLink(string recordnumber, string invoicedate, string toinvoicedate, string invoicenumber, string vendornumber, string vendorname, string ponumber, string invoicetypecd, string sortOrder, int? page)
        {
            ViewBag.Currentrecordnumber = recordnumber;
            ViewBag.Currentinvoicedate = invoicedate;
            ViewBag.Currenttoinvoicedate = toinvoicedate;
            ViewBag.Currentinvoicenumber = invoicenumber;
            ViewBag.Currentvendornumber = vendornumber;
            ViewBag.Currentvendorname = vendorname;
            ViewBag.Currentponumber = ponumber;
            ViewBag.Currentinvoicetypecd = invoicetypecd;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date" : "";
            ViewBag.VendorNameSortParm = sortOrder == "vendor" ? "vendor_desc" : "vendor";
            ViewBag.InvoiceTypeSortParm = sortOrder == "invoicetype" ? "invoicetype_desc" : "invoicetype";
            DateTime toinvoicedateparse; DateTime invoicedateparse;

            var apinvoices = from m in db.APInvoices select m;
            

            if (!String.IsNullOrEmpty(recordnumber))
            {
                apinvoices = apinvoices.Where(a => a.Record_Number.Contains(recordnumber));
            }

            if (!String.IsNullOrEmpty(invoicenumber))
            {
                apinvoices = apinvoices.Where(c => c.Invoice_Number.Contains(invoicenumber));
            }

            if (!String.IsNullOrEmpty(vendornumber))
            {
                apinvoices = apinvoices.Where(d => d.Vendor_Number.Contains(vendornumber));
            }

            if (!String.IsNullOrEmpty(vendorname))
            {
                apinvoices = apinvoices.Where(e => e.Vendor_Name.Contains(vendorname));
            }

            if (!String.IsNullOrEmpty(ponumber))
            {
                apinvoices = apinvoices.Where(f => f.PO_Number.Contains(ponumber));
            }

            if (!String.IsNullOrEmpty(invoicetypecd))
            {
                apinvoices = apinvoices.Where(g => g.Invoice_Type_cd.Contains(invoicetypecd));
            }

            if ((!string.IsNullOrEmpty(invoicedate)) && (string.IsNullOrEmpty(toinvoicedate)))
            {
                invoicedateparse = Convert.ToDateTime(invoicedate);
                apinvoices = apinvoices.Where(b => b.Invoice_Date.Year >= invoicedateparse.Year && b.Invoice_Date.Month >= invoicedateparse.Month && b.Invoice_Date.Day >= invoicedateparse.Day);
            }

            if ((string.IsNullOrEmpty(invoicedate)) && (!string.IsNullOrEmpty(toinvoicedate)))
            {
                toinvoicedateparse = Convert.ToDateTime(toinvoicedate);
                apinvoices = apinvoices.Where(b => b.Invoice_Date.Year <= toinvoicedateparse.Year && b.Invoice_Date.Month <= toinvoicedateparse.Month && b.Invoice_Date.Day <= toinvoicedateparse.Day);
            }

            if ((!string.IsNullOrEmpty(invoicedate)) && (!string.IsNullOrEmpty(toinvoicedate)))
            {
                toinvoicedateparse = Convert.ToDateTime(toinvoicedate);
                invoicedateparse = Convert.ToDateTime(invoicedate);
                apinvoices = apinvoices.Where(b => (b.Invoice_Date >= invoicedateparse.Date));
                apinvoices = apinvoices.Where(b => (b.Invoice_Date <= toinvoicedateparse.Date));
            }

             if (apinvoices.Count() > 100)
            {
                ViewBag.TotalAPRecords = "Search Criteria returned " + apinvoices.Count() + " records. Only the first 100 records are displayed. Please narrow your search criteria.";
                apinvoices = apinvoices.OrderByDescending(a => a.Invoice_Date).Take(100);
            }
            else
            {
                ViewBag.TotalAPRecords = "Your search criteria returned " + apinvoices.Count() + " record(s).";
            }

            switch (sortOrder)
            {
                case "vendor_desc":
                    apinvoices = apinvoices.OrderByDescending(s => s.Vendor_Name);
                    break;
                case "vendor":
                    apinvoices = apinvoices.OrderBy(s => s.Vendor_Name);
                    break;
                case "invoicetype_desc":
                    apinvoices = apinvoices.OrderByDescending(s => s.Invoice_Type_cd);
                    break;
                case "invoicetype":
                    apinvoices = apinvoices.OrderBy(s => s.Invoice_Type_cd);
                    break;
                case "date":
                    apinvoices = apinvoices.OrderBy(s => s.Invoice_Date);
                    break;
                default:
                    apinvoices = apinvoices.OrderByDescending(a => a.Invoice_Date);
                    break;
            }

            int pageSize = 30;
            int pageNumber = (page ?? 1);
            return View(apinvoices.ToPagedList(pageNumber, pageSize));

        }

        
        public JsonResult GetAutoCompleteData(string term)
        {
            var apinvoices = db.APInvoices.Where(m => m.Record_Number.StartsWith(term)).Select (y => y.Record_Number).ToList();
            
               /* var recordnumbers = from r in apinvoices select r.Record_Number;
                if (!String.IsNullOrEmpty(term))
                {
                    apinvoices = apinvoices.Where(a => a.Record_Number.StartsWith(term));
                    return recordnumbers.ToList();
                }
            */
            
            return Json(apinvoices, JsonRequestBehavior.AllowGet);
        }

       /* [HttpPost]
        public async Task<PartialViewResult> Index(string recordnumber)
        {
            var apinvoices = from m in db.APInvoices select m;
            if (!String.IsNullOrEmpty(recordnumber))
            {
                apinvoices = apinvoices.Where(a => a.Record_Number.Contains(recordnumber));
            }

            return PartialView("_AP", await apinvoices.ToListAsync());
        }*/

/*
        // GET: /APInvoice/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            APInvoice apinvoice = await db.APInvoices.FindAsync(id);
            if (apinvoice == null)
            {
                return HttpNotFound();
            }
            return View(apinvoice);
        }

        // GET: /APInvoice/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /APInvoice/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="APInvoiceID,Record_Number,Invoice_Date,Invoice_Number,Vendor_Number,Vendor_Name,PO_Number,Invoice_Type_cd")] APInvoice apinvoice)
        {
            if (ModelState.IsValid)
            {
                db.APInvoices.Add(apinvoice);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(apinvoice);
        }

        // GET: /APInvoice/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            APInvoice apinvoice = await db.APInvoices.FindAsync(id);
            if (apinvoice == null)
            {
                return HttpNotFound();
            }
            return View(apinvoice);
        }

        // POST: /APInvoice/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="APInvoiceID,Record_Number,Invoice_Date,Invoice_Number,Vendor_Number,Vendor_Name,PO_Number,Invoice_Type_cd")] APInvoice apinvoice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(apinvoice).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(apinvoice);
        }
*/
        // GET: /APInvoice/Delete/5
        [EdmAuthorize("APAdmin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            APInvoice apinvoice = await db.APInvoices.FindAsync(id);
            if (apinvoice == null)
            {
                return HttpNotFound();
            }
            return View(apinvoice);
        }

        // POST: /APInvoice/Delete/5
        [EdmAuthorize("APAdmin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            APInvoice apinvoice = await db.APInvoices.FindAsync(id);
            // Delete blob
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    ConfigurationManager.ConnectionStrings["APStorageConnectionString"].ConnectionString);

            String containerName = ConfigurationManager.ConnectionStrings["APContainerConnectionString"].ConnectionString;

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            // Retrieve reference to a blob named blobName
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(apinvoice.Record_Number.Trim());

            blockBlob.DeleteIfExists();

            db.APInvoices.Remove(apinvoice);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        
    }
}
