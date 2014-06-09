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
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;

namespace WebRole1.Controllers
{
    public class ARCustRemitController : Controller
    {
        private ARAppDBContext db = new ARAppDBContext();

        // GET: /ARCustRemit/
        public async Task<ActionResult> Index()
        {
            return View(await db.ARCustRemits.ToListAsync());
        }
/*
        // GET: /ARCustRemit/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARCustRemit arcustremit = await db.ARCustRemits.FindAsync(id);
            if (arcustremit == null)
            {
                return HttpNotFound();
            }
            return View(arcustremit);
        }

        // GET: /ARCustRemit/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /ARCustRemit/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="ARCustRemitID,Record_Number,Chk_Deposit_Dt,Chk_Serial_Num,Chk_Transit_Num,Chk_Account_Num,Lockbox,Has_Action_Item,Lawson_Customer")] ARCustRemit arcustremit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.ARCustRemits.Add(arcustremit);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            } catch (DataException dex)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator. dex="+dex);
            }
            return View(arcustremit);
        }

        // GET: /ARCustRemit/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARCustRemit arcustremit = await db.ARCustRemits.FindAsync(id);
            if (arcustremit == null)
            {
                return HttpNotFound();
            }
            return View(arcustremit);
        }

        // POST: /ARCustRemit/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="ARCustRemitID,Record_Number,Chk_Deposit_Dt,Chk_Serial_Num,Chk_Transit_Num,Chk_Account_Num,Lockbox,Has_Action_Item,Lawson_Customer")] ARCustRemit arcustremit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(arcustremit).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(arcustremit);
        }
*/
        // GET: /ARCustRemit/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARCustRemit arcustremit = await db.ARCustRemits.FindAsync(id);
            if (arcustremit == null)
            {
                return HttpNotFound();
            }
            return View(arcustremit);
        }

        // POST: /ARCustRemit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ARCustRemit arcustremit = await db.ARCustRemits.FindAsync(id);

            // Delete blob
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    ConfigurationManager.ConnectionStrings["ARStorageConnectionString"].ConnectionString);

            String containerName = ConfigurationManager.ConnectionStrings["ARContainerConnectionString"].ConnectionString;

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            // Retrieve reference to a blob named blobName
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(arcustremit.Record_Number.Trim());

            blockBlob.DeleteIfExists();

            // Then delete meta data record
            db.ARCustRemits.Remove(arcustremit);
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
