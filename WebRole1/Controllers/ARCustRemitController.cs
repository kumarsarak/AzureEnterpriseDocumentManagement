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
using System.Configuration;
using PagedList.Mvc;
using PagedList;

namespace WebRole1.Controllers
{
     [EdmAuthorize("ARAdmin", "ARUser")]
    public class ARCustRemitController : Controller
    {
        private ARAppDBContext db = new ARAppDBContext();

        public ViewResult Index(string recordnumber, string chkdepdate, string tochkdepdate, string checknumber, string routingnumber, string chkaccnumber, string lockbox, string sortOrder, int? page)
        {

            if (NoParams(recordnumber, chkdepdate, tochkdepdate, checknumber, routingnumber, chkaccnumber, lockbox))
            {
                return NoParamSearch(sortOrder, page);
            }
            else
            {
                return SearchLink(recordnumber, chkdepdate, tochkdepdate, checknumber, routingnumber, chkaccnumber, lockbox, sortOrder, page);

            }

        }

        private bool NoParams(string recordnumber, string chkdepdate, string tochkdepdate, string checknumber, string routingnumber, string chkaccnumber, string lockbox)
        {
            if (String.IsNullOrEmpty(recordnumber) && String.IsNullOrEmpty(chkdepdate) && String.IsNullOrEmpty(tochkdepdate) && String.IsNullOrEmpty(checknumber) && String.IsNullOrEmpty(routingnumber) && String.IsNullOrEmpty(chkaccnumber) && String.IsNullOrEmpty(lockbox))
                return true;
            else
                return false;
        }

        private ViewResult NoParamSearch(string sortOrder, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date" : "";
            ViewBag.LockBoxSortParm = sortOrder == "lockbox" ? "lockbox_desc" : "lockbox";
            ViewBag.CheckNumberSortParm = sortOrder == "checknumber" ? "checknumber_desc" : "checknumber";
            var arcustremits = db.ARCustRemits.OrderByDescending(a => a.Chk_Deposit_Dt).Take(100);


            switch (sortOrder)
            {
                
                case "lockbox_desc":
                    arcustremits = arcustremits.OrderByDescending(s => s.Lockbox);
                    break;
                case "lockbox":
                    arcustremits = arcustremits.OrderBy(s => s.Lockbox);
                    break;
                case "checknumber_desc":
                    arcustremits = arcustremits.OrderByDescending(s => s.Chk_Serial_Num);
                    break;
                case "checknumber":
                    arcustremits = arcustremits.OrderBy(s => s.Chk_Serial_Num);
                    break;
                case "date":
                    arcustremits = arcustremits.OrderBy(s => s.Chk_Deposit_Dt);
                    break;
                default:
                    arcustremits = arcustremits.OrderByDescending(a => a.Chk_Deposit_Dt);
                    break;
            }

            int pageSize = 30;
            int pageNumber = (page ?? 1);


            return View(arcustremits.ToPagedList(pageNumber, pageSize));
            //return PartialView("_AR", arcustremits.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public async Task<PartialViewResult> Search(string recordnumber, string chkdepdate, string tochkdepdate, string checknumber, string routingnumber, string chkaccnumber, string lockbox, string sortOrder, int? page)
        {
            ViewBag.Currentrecordnumber = recordnumber;
            ViewBag.Currentchkdepdate = chkdepdate;
            ViewBag.Currenttochkdepdate = tochkdepdate;
            ViewBag.Currentchecknumber = checknumber;
            ViewBag.Currentroutingnumber = routingnumber;
            ViewBag.Currentchkaccnumber = chkaccnumber;
            ViewBag.Currentlockbox = lockbox;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date" : "";
            ViewBag.LockBoxSortParm = sortOrder == "lockbox" ? "lockbox_desc" : "lockbox";
            ViewBag.CheckNumberSortParm = sortOrder == "checknumber" ? "checknumber_desc" : "checknumber";
            DateTime tochkdepdateparse ;
            DateTime chkdepdateparse ;



            page = 1;
            var arcustremits = from m in db.ARCustRemits select m;

            if (!String.IsNullOrEmpty(recordnumber))
            {
                arcustremits = arcustremits.Where(a => a.Record_Number.Contains(recordnumber));
            }


            if (!String.IsNullOrEmpty(checknumber))
            {
                var chksernumberfilter = checknumber;

                if ((chksernumberfilter.StartsWith("*")) && (chksernumberfilter.EndsWith("*")))
                {
                    chksernumberfilter = chksernumberfilter.Replace("*", "");
                    arcustremits = arcustremits.Where(e => e.Chk_Serial_Num.Contains(chksernumberfilter));
                }
                else if (chksernumberfilter.StartsWith("*"))
                {
                    chksernumberfilter = chksernumberfilter.Replace("*", "");
                    arcustremits = arcustremits.Where(e => e.Chk_Serial_Num.Trim().EndsWith(chksernumberfilter));
                }
                else if (chksernumberfilter.EndsWith("*"))
                {
                    chksernumberfilter = chksernumberfilter.Replace("*", "");
                    arcustremits = arcustremits.Where(e => e.Chk_Serial_Num.Trim().StartsWith(chksernumberfilter));
                }
                else
                {
                    arcustremits = arcustremits.Where(e => e.Chk_Serial_Num.Equals(checknumber));
                }
            }

            if (!String.IsNullOrEmpty(routingnumber))
            {
                arcustremits = arcustremits.Where(d => d.Chk_Transit_Num.Contains(routingnumber));
            }

            if (!String.IsNullOrEmpty(chkaccnumber))
            {
                arcustremits = arcustremits.Where(e => e.Chk_Account_Num.Contains(chkaccnumber));
            }

            
            if (!String.IsNullOrEmpty(lockbox))
            {
                arcustremits = arcustremits.Where(f => f.Lockbox.Contains(lockbox));
            }

            if ((!string.IsNullOrEmpty(chkdepdate)) && (string.IsNullOrEmpty(tochkdepdate)))
            {
                chkdepdateparse = Convert.ToDateTime(chkdepdate);
                arcustremits = arcustremits.Where(b => b.Chk_Deposit_Dt.Year >= chkdepdateparse.Year && b.Chk_Deposit_Dt.Month >= chkdepdateparse.Month && b.Chk_Deposit_Dt.Day >= chkdepdateparse.Day);
            }

            if ((string.IsNullOrEmpty(chkdepdate)) && (!string.IsNullOrEmpty(tochkdepdate)))
            {
                tochkdepdateparse = Convert.ToDateTime(tochkdepdate);
                arcustremits = arcustremits.Where(b => b.Chk_Deposit_Dt.Year <= tochkdepdateparse.Year && b.Chk_Deposit_Dt.Month <= tochkdepdateparse.Month && b.Chk_Deposit_Dt.Day <= tochkdepdateparse.Day);
            }

            if ((!string.IsNullOrEmpty(chkdepdate)) && (!string.IsNullOrEmpty(tochkdepdate)))
            {
                chkdepdateparse = Convert.ToDateTime(chkdepdate);
                tochkdepdateparse = Convert.ToDateTime(tochkdepdate);

                arcustremits = arcustremits.Where(b => (b.Chk_Deposit_Dt >= chkdepdateparse.Date));
                arcustremits = arcustremits.Where(b => (b.Chk_Deposit_Dt <= tochkdepdateparse.Date));
            }

            if (arcustremits.Count() > 100)
            {
                ViewBag.TotalARRecords = "Search Criteria returned " + arcustremits.Count() + " records. Only the first 100 records are displayed. Please narrow your search criteria.";
                arcustremits = arcustremits.OrderByDescending(a => a.Chk_Deposit_Dt).Take(100);
            }
            else
            {
                ViewBag.TotalARRecords = "Your search criteria returned " + arcustremits.Count() + " record(s).";
            }

           

            switch (sortOrder)
            {
                case "lockbox_desc":
                    arcustremits = arcustremits.OrderByDescending(s => s.Lockbox);
                    break;
                case "lockbox":
                    arcustremits = arcustremits.OrderBy(s => s.Lockbox);
                    break;
                case "checknumber_desc":
                    arcustremits = arcustremits.OrderByDescending(s => s.Chk_Serial_Num);
                    break;
                case "checknumber":
                    arcustremits = arcustremits.OrderBy(s => s.Chk_Serial_Num);
                    break;
                case "date":
                    arcustremits = arcustremits.OrderBy(s => s.Chk_Deposit_Dt);
                    break;
                default:
                    arcustremits = arcustremits.OrderByDescending(a => a.Chk_Deposit_Dt);
                    break;
            }

            int pageSize = 30;
            int pageNumber = (page ?? 1);


            //return PartialView("_AP", await apinvoices.ToListAsync());
            return PartialView("_AR", arcustremits.ToPagedList(pageNumber, pageSize));

        }

        public ViewResult SearchLink(string recordnumber, string chkdepdate, string tochkdepdate, string checknumber, string routingnumber, string chkaccnumber, string lockbox, string sortOrder, int? page)
        {

            ViewBag.Currentrecordnumber = recordnumber;
            ViewBag.Currentchkdepdate = chkdepdate;
            ViewBag.Currenttochkdepdate = tochkdepdate;
            ViewBag.Currentchecknumber = checknumber;
            ViewBag.Currentroutingnumber = routingnumber;
            ViewBag.Currentchkaccnumber = chkaccnumber;
            ViewBag.Currentlockbox = lockbox;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date" : "";
            ViewBag.LockBoxSortParm = sortOrder == "lockbox" ? "lockbox_desc" : "lockbox";
            ViewBag.CheckNumberSortParm = sortOrder == "checknumber" ? "checknumber_desc" : "checknumber";
            DateTime tochkdepdateparse;
            DateTime chkdepdateparse;


            var arcustremits = from m in db.ARCustRemits select m;
            

            if (!String.IsNullOrEmpty(recordnumber))
            {
                arcustremits = arcustremits.Where(a => a.Record_Number.Contains(recordnumber));
            }

            if (!String.IsNullOrEmpty(checknumber))
            {
                var chksernumberfilter = checknumber;

                if ((chksernumberfilter.StartsWith("*")) && (chksernumberfilter.EndsWith("*")))
                {
                    chksernumberfilter = chksernumberfilter.Replace("*", "");
                    arcustremits = arcustremits.Where(e => e.Chk_Serial_Num.Contains(chksernumberfilter));
                }
                else if (chksernumberfilter.StartsWith("*"))
                {
                    chksernumberfilter = chksernumberfilter.Replace("*", "");
                    arcustremits = arcustremits.Where(e => e.Chk_Serial_Num.Trim().EndsWith(chksernumberfilter));
                }
                else if (chksernumberfilter.EndsWith("*"))
                {
                    chksernumberfilter = chksernumberfilter.Replace("*", "");
                    arcustremits = arcustremits.Where(e => e.Chk_Serial_Num.Trim().StartsWith(chksernumberfilter));
                }
                else
                {
                    arcustremits = arcustremits.Where(e => e.Chk_Serial_Num.Equals(checknumber));
                }
            }

            if (!String.IsNullOrEmpty(routingnumber))
            {
                arcustremits = arcustremits.Where(d => d.Chk_Transit_Num.Contains(routingnumber));
            }

            if (!String.IsNullOrEmpty(chkaccnumber))
            {
                arcustremits = arcustremits.Where(e => e.Chk_Account_Num.Contains(chkaccnumber));
            }

            if (!String.IsNullOrEmpty(lockbox))
            {
                arcustremits = arcustremits.Where(f => f.Lockbox.Contains(lockbox));
            }

            if ((!string.IsNullOrEmpty(chkdepdate)) && (string.IsNullOrEmpty(tochkdepdate)))
            {
                chkdepdateparse = Convert.ToDateTime(chkdepdate);
                arcustremits = arcustremits.Where(b => b.Chk_Deposit_Dt.Year >= chkdepdateparse.Year && b.Chk_Deposit_Dt.Month >= chkdepdateparse.Month && b.Chk_Deposit_Dt.Day >= chkdepdateparse.Day);
            }

            if ((string.IsNullOrEmpty(chkdepdate)) && (!string.IsNullOrEmpty(tochkdepdate)))
            {
                tochkdepdateparse = Convert.ToDateTime(tochkdepdate);
                arcustremits = arcustremits.Where(b => b.Chk_Deposit_Dt.Year <= tochkdepdateparse.Year && b.Chk_Deposit_Dt.Month <= tochkdepdateparse.Month && b.Chk_Deposit_Dt.Day <= tochkdepdateparse.Day);
            }

            if ((!string.IsNullOrEmpty(chkdepdate)) && (!string.IsNullOrEmpty(tochkdepdate)))
            {
                chkdepdateparse = Convert.ToDateTime(chkdepdate);
                tochkdepdateparse = Convert.ToDateTime(tochkdepdate);

                arcustremits = arcustremits.Where(b => (b.Chk_Deposit_Dt >= chkdepdateparse.Date));
                arcustremits = arcustremits.Where(b => (b.Chk_Deposit_Dt <= tochkdepdateparse.Date));
            }

            if (arcustremits.Count() > 100)
            {
                ViewBag.TotalARRecords = "Search Criteria returned " + arcustremits.Count() + " records. Only the first 100 records are displayed. Please narrow your search criteria.";
                arcustremits = arcustremits.OrderByDescending(a => a.Chk_Deposit_Dt).Take(100);
            }
            else
            {
                ViewBag.TotalARRecords = "Your search criteria returned " + arcustremits.Count() + " record(s).";
            }


            switch (sortOrder)
            {
                case "lockbox_desc":
                    arcustremits = arcustremits.OrderByDescending(s => s.Lockbox);
                    break;
                case "lockbox":
                    arcustremits = arcustremits.OrderBy(s => s.Lockbox);
                    break;
                case "checknumber_desc":
                    arcustremits = arcustremits.OrderByDescending(s => s.Chk_Serial_Num);
                    break;
                case "checknumber":
                    arcustremits = arcustremits.OrderBy(s => s.Chk_Serial_Num);
                    break;
                case "date":
                    arcustremits = arcustremits.OrderBy(s => s.Chk_Deposit_Dt);
                    break;
                default:
                    arcustremits = arcustremits.OrderByDescending(a => a.Chk_Deposit_Dt);
                    break;
            }

            int pageSize = 30;
            int pageNumber = (page ?? 1);


            return View(arcustremits.ToPagedList(pageNumber, pageSize));
            //return PartialView("_AR", arcustremits.ToPagedList(pageNumber, pageSize));

        }

        

        public JsonResult GetAutoCompleteData(string term)
        {
            var arcustremits = db.ARCustRemits.Where(m => m.Record_Number.StartsWith(term)).Select(y => y.Record_Number).ToList();

            /* var recordnumbers = from r in apinvoices select r.Record_Number;
             if (!String.IsNullOrEmpty(term))
             {
                 apinvoices = apinvoices.Where(a => a.Record_Number.StartsWith(term));
                 return recordnumbers.ToList();
             }
         */

            return Json(arcustremits, JsonRequestBehavior.AllowGet);
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
        [EdmAuthorize("ARAdmin")]
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
        [EdmAuthorize("ARAdmin")]
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
