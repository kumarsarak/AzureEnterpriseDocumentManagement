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

namespace WebRole1.Controllers
{
    public class APInvoiceController : Controller
    {
        private APAppDBContext db = new APAppDBContext();

        public async Task<ActionResult> Index()
        {
            var apinvoices = from m in db.APInvoices select m;
            return View(await apinvoices.ToListAsync());
        }

        [HttpPost]
        public async Task<PartialViewResult> Search(string recordnumber, string invoicedate, string invoicenumber, string vendornumber, string vendorname, string ponumber, string invoicetypecd)
        {
            var apinvoices = from m in db.APInvoices select m;
            if (!String.IsNullOrEmpty(recordnumber))
            {
                apinvoices = apinvoices.Where(a => a.Record_Number.Contains(recordnumber));
            }

            if (!string.IsNullOrEmpty(invoicedate))
            {
                apinvoices = apinvoices.Where(b => b.Invoice_Date.ToString() == invoicedate);
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

            return PartialView("_AP", await apinvoices.ToListAsync());

            
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
