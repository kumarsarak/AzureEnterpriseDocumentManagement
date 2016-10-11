using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
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
    [EdmAuthorize("APCAAdmin", "APCAUser")]
    public class APCAInvoiceController : Controller
    {

        public System.Threading.CancellationToken cancellationToken;

        public APCAInvoiceController()
        {
            System.Threading.CancellationTokenSource tokenSource = new System.Threading.CancellationTokenSource();
            cancellationToken = tokenSource.Token;
        }
        
        private APCAAppDBContext db = new APCAAppDBContext();

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
            var apcainvoices = db.APCAInvoices.OrderByDescending(a => a.Invoice_Date).Take(100);


            switch (sortOrder)
            {
                case "vendor_desc":
                    apcainvoices = apcainvoices.OrderByDescending(s => s.Vendor_Name);
                    break;
                case "vendor":
                    apcainvoices = apcainvoices.OrderBy(s => s.Vendor_Name);
                    break;
                case "invoicetype_desc":
                    apcainvoices = apcainvoices.OrderByDescending(s => s.Invoice_Type_cd);
                    break;
                case "invoicetype":
                    apcainvoices = apcainvoices.OrderBy(s => s.Invoice_Type_cd);
                    break;
                case "date":
                    apcainvoices = apcainvoices.OrderBy(s => s.Invoice_Date);
                    break;
                default:
                    apcainvoices = apcainvoices.OrderByDescending(a => a.Invoice_Date);
                    break;
            }

            int pageSize = 30;
            int pageNumber = (page ?? 1);


            return View(apcainvoices.ToPagedList(pageNumber, pageSize));
        }

        private IQueryable<APCAInvoice> GetApInvoices()
        {
            var apcainvoices = from m in db.APCAInvoices select m;
            return apcainvoices;
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
            DateTime? toinvoicedateparse = null; DateTime? invoicedateparse = null;
            if (!String.IsNullOrEmpty(toinvoicedate)) { toinvoicedateparse = Convert.ToDateTime(toinvoicedate); }
            if (!String.IsNullOrEmpty(invoicedate)) { invoicedateparse = Convert.ToDateTime(invoicedate); }


            page = 1;
            var apcainvoices = new List<APCAInvoice>();

            if (NoParams(recordnumber, invoicedate, toinvoicedate, invoicenumber, vendornumber, vendorname, ponumber, invoicetypecd))
            {
                apcainvoices = db.APCAInvoices.OrderByDescending(a => a.Invoice_Date).Take(100).ToList();
            }
            else
            {

                SqlConnection oSQLConn = new SqlConnection();
                oSQLConn.ConnectionString = ConfigurationManager.ConnectionStrings["APCAAppDBContext"].ConnectionString;

                try
                {
                    await oSQLConn.OpenAsync();
                    SqlCommand cmd = new SqlCommand("GetAPCAInvoices", oSQLConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter fromInvoiceDateparam = new SqlParameter("@FromInvoiceDate", System.Data.SqlDbType.DateTime);
                    fromInvoiceDateparam.Value = invoicedateparse;
                    SqlParameter toInvoiceDateparam = new SqlParameter("@ToInvoiceDate", System.Data.SqlDbType.DateTime);
                    toInvoiceDateparam.Value = toinvoicedateparse;

                    cmd.Parameters.AddWithValue("@RecordNumber", recordnumber);
                    cmd.Parameters.Add(fromInvoiceDateparam);
                    cmd.Parameters.Add(toInvoiceDateparam);
                    cmd.Parameters.AddWithValue("@InvoiceNumber", invoicenumber);
                    cmd.Parameters.AddWithValue("@VendorNumber", vendornumber);
                    cmd.Parameters.AddWithValue("@VendorName", vendorname);
                    cmd.Parameters.AddWithValue("@PONumber", ponumber);

                    await Task.Run(async () =>
                    {
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken))
                        {
                            while (await reader.ReadAsync(cancellationToken))
                            {
                                apcainvoices.Add(new APCAInvoice
                                {

                                    //APCAInvoiceID = reader["APCAInvoiceID"].ToString(),
                                    Record_Number = reader["Record_Number"].ToString(),
                                    Invoice_Date = Convert.ToDateTime(reader["Invoice_Date"].ToString()),
                                    Invoice_Number = reader["Invoice_Number"].ToString(),
                                    Vendor_Number = reader["Vendor_Number"].ToString(),
                                    Vendor_Name = reader["Vendor_Name"].ToString(),
                                    PO_Number = reader["PO_Number"].ToString(),
                                    Invoice_Type_cd = (reader["Invoice_Type_cd"] != null) ? reader["Invoice_Type_cd"].ToString() : null


                                    //etc
                                });
                            }
                        }
                    }, cancellationToken);


                }
                catch (Exception ex)
                {
                }


                if (apcainvoices.Count() > 100)
                {
                    ViewBag.TotalAPRecords = "Search Criteria returned " + apcainvoices.Count() + " records. Only the first 100 records are displayed. Please narrow your search criteria.";
                    if (Response.IsClientConnected)
                    {
                        apcainvoices = await Task.Run(() => apcainvoices.OrderByDescending(a => a.Invoice_Date).Take(100).ToList());
                    }
                    else
                    {
                        apcainvoices = await Task.Run(() => apcainvoices.OrderByDescending(a => a.Invoice_Date).Take(100).ToList(), cancellationToken);
                    }
                }
                else
                {
                    ViewBag.TotalAPRecords = "Your search criteria returned " + apcainvoices.Count() + " record(s).";
                }

            }

            switch (sortOrder)
            {
                case "vendor_desc":
                    apcainvoices = apcainvoices.OrderByDescending(s => s.Vendor_Name).ToList();
                    break;
                case "vendor":
                    apcainvoices = apcainvoices.OrderBy(s => s.Vendor_Name).ToList();
                    break;
                case "invoicetype_desc":
                    apcainvoices = apcainvoices.OrderByDescending(s => s.Invoice_Type_cd).ToList();
                    break;
                case "invoicetype":
                    apcainvoices = apcainvoices.OrderBy(s => s.Invoice_Type_cd).ToList();
                    break;
                case "date":
                    apcainvoices = apcainvoices.OrderBy(s => s.Invoice_Date).ToList();
                    break;
                default:
                    apcainvoices = apcainvoices.OrderByDescending(a => a.Invoice_Date).ToList();
                    break;
            }

            int pageSize = 30;
            int pageNumber = (page ?? 1);


            //return PartialView("_AP", await apcainvoices.ToListAsync());
            return PartialView("_APCA", apcainvoices.ToPagedList(pageNumber, pageSize));

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
            DateTime? toinvoicedateparse = null; DateTime? invoicedateparse = null;
            if (!String.IsNullOrEmpty(toinvoicedate)) { toinvoicedateparse = Convert.ToDateTime(toinvoicedate); }
            if (!String.IsNullOrEmpty(invoicedate)) { invoicedateparse = Convert.ToDateTime(invoicedate); }


           // page = 1;
            var apcainvoices = new List<APCAInvoice>();

            if (NoParams(recordnumber, invoicedate, toinvoicedate, invoicenumber, vendornumber, vendorname, ponumber, invoicetypecd))
            {
                apcainvoices = db.APCAInvoices.OrderByDescending(a => a.Invoice_Date).Take(100).ToList();
            }
            else
            {

                SqlConnection oSQLConn = new SqlConnection();
                oSQLConn.ConnectionString = ConfigurationManager.ConnectionStrings["APCAAppDBContext"].ConnectionString;

                try
                {
                   oSQLConn.Open();
                    SqlCommand cmd = new SqlCommand("GetAPCAInvoices", oSQLConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter fromInvoiceDateparam = new SqlParameter("@FromInvoiceDate", System.Data.SqlDbType.DateTime);
                    fromInvoiceDateparam.Value = invoicedateparse;
                    SqlParameter toInvoiceDateparam = new SqlParameter("@ToInvoiceDate", System.Data.SqlDbType.DateTime);
                    toInvoiceDateparam.Value = toinvoicedateparse;

                    cmd.Parameters.AddWithValue("@RecordNumber", recordnumber);
                    cmd.Parameters.Add(fromInvoiceDateparam);
                    cmd.Parameters.Add(toInvoiceDateparam);
                    cmd.Parameters.AddWithValue("@InvoiceNumber", invoicenumber);
                    cmd.Parameters.AddWithValue("@VendorNumber", vendornumber);
                    cmd.Parameters.AddWithValue("@VendorName", vendorname);
                    cmd.Parameters.AddWithValue("@PONumber", ponumber);

                   
                        using (SqlDataReader reader =  cmd.ExecuteReader())
                        {
                            while ( reader.Read())
                            {
                                apcainvoices.Add(new APCAInvoice
                                {

                                    //APCAInvoiceID = reader["APCAInvoiceID"].ToString(),
                                    Record_Number = reader["Record_Number"].ToString(),
                                    Invoice_Date = Convert.ToDateTime(reader["Invoice_Date"].ToString()),
                                    Invoice_Number = reader["Invoice_Number"].ToString(),
                                    Vendor_Number = reader["Vendor_Number"].ToString(),
                                    Vendor_Name = reader["Vendor_Name"].ToString(),
                                    PO_Number = reader["PO_Number"].ToString(),
                                    Invoice_Type_cd = (reader["Invoice_Type_cd"] != null) ? reader["Invoice_Type_cd"].ToString() : null


                                    //etc
                                });
                            }
                        }
                    


                }
                catch (Exception ex)
                {
                }


                if (apcainvoices.Count() > 100)
                {
                    ViewBag.TotalAPRecords = "Search Criteria returned " + apcainvoices.Count() + " records. Only the first 100 records are displayed. Please narrow your search criteria.";
                    if (Response.IsClientConnected)
                    {
                        apcainvoices =  apcainvoices.OrderByDescending(a => a.Invoice_Date).Take(100).ToList();
                    }
                    else
                    {
                        apcainvoices = apcainvoices.OrderByDescending(a => a.Invoice_Date).Take(100).ToList();
                    }
                }
                else
                {
                    ViewBag.TotalAPRecords = "Your search criteria returned " + apcainvoices.Count() + " record(s).";
                }

            }

            switch (sortOrder)
            {
                case "vendor_desc":
                    apcainvoices = apcainvoices.OrderByDescending(s => s.Vendor_Name).ToList();
                    break;
                case "vendor":
                    apcainvoices = apcainvoices.OrderBy(s => s.Vendor_Name).ToList();
                    break;
                case "invoicetype_desc":
                    apcainvoices = apcainvoices.OrderByDescending(s => s.Invoice_Type_cd).ToList();
                    break;
                case "invoicetype":
                    apcainvoices = apcainvoices.OrderBy(s => s.Invoice_Type_cd).ToList();
                    break;
                case "date":
                    apcainvoices = apcainvoices.OrderBy(s => s.Invoice_Date).ToList();
                    break;
                default:
                    apcainvoices = apcainvoices.OrderByDescending(a => a.Invoice_Date).ToList();
                    break;
            }

            int pageSize = 30;
            int pageNumber = (page ?? 1);


            //return PartialView("_AP", await apcainvoices.ToListAsync());
            return View(apcainvoices.ToPagedList(pageNumber, pageSize));

        }

        
        public JsonResult GetAutoCompleteData(string term)
        {
            var apcainvoices = db.APCAInvoices.Where(m => m.Record_Number.StartsWith(term)).Select (y => y.Record_Number).ToList();
            
               /* var recordnumbers = from r in apcainvoices select r.Record_Number;
                if (!String.IsNullOrEmpty(term))
                {
                    apcainvoices = apcainvoices.Where(a => a.Record_Number.StartsWith(term));
                    return recordnumbers.ToList();
                }
            */
            
            return Json(apcainvoices, JsonRequestBehavior.AllowGet);
        }

       /* [HttpPost]
        public async Task<PartialViewResult> Index(string recordnumber)
        {
            var apcainvoices = from m in db.APCAInvoices select m;
            if (!String.IsNullOrEmpty(recordnumber))
            {
                apcainvoices = apcainvoices.Where(a => a.Record_Number.Contains(recordnumber));
            }

            return PartialView("_AP", await apcainvoices.ToListAsync());
        }*/

/*
        // GET: /APCAInvoice/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            APCAInvoice apinvoice = await db.APCAInvoices.FindAsync(id);
            if (apinvoice == null)
            {
                return HttpNotFound();
            }
            return View(apinvoice);
        }

        // GET: /APCAInvoice/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /APCAInvoice/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="APCAInvoiceID,Record_Number,Invoice_Date,Invoice_Number,Vendor_Number,Vendor_Name,PO_Number,Invoice_Type_cd")] APCAInvoice apinvoice)
        {
            if (ModelState.IsValid)
            {
                db.APCAInvoices.Add(apinvoice);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(apinvoice);
        }

        // GET: /APCAInvoice/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            APCAInvoice apinvoice = await db.APCAInvoices.FindAsync(id);
            if (apinvoice == null)
            {
                return HttpNotFound();
            }
            return View(apinvoice);
        }

        // POST: /APCAInvoice/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="APCAInvoiceID,Record_Number,Invoice_Date,Invoice_Number,Vendor_Number,Vendor_Name,PO_Number,Invoice_Type_cd")] APCAInvoice apinvoice)
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
        // GET: /APCAInvoice/Delete/5
        [EdmAuthorize("APCAAdmin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            APCAInvoice apinvoice = await db.APCAInvoices.FindAsync(id);
            if (apinvoice == null)
            {
                return HttpNotFound();
            }
            return View(apinvoice);
        }

        // POST: /APCAInvoice/Delete/5
        [EdmAuthorize("APCAAdmin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            APCAInvoice apinvoice = await db.APCAInvoices.FindAsync(id);
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

            db.APCAInvoices.Remove(apinvoice);
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
