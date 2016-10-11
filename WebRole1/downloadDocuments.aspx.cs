using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using ICSharpCode.SharpZipLib.Zip;

namespace WebRole1
{
    public partial class downloadDocuments : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String recNums = "default";
            String containerNameConStr = "ContainerConnectionString";
            String storageConStr = "StorageConnectionString";

            if (Request.QueryString["recordNumber"] != null && Request.QueryString["docType"] != null)
            {
                if (authorizedownload())
                {
                    try
                    {
                        recNums = (Request.QueryString["recordNumber"]).Trim();
                        List<String> downLoadFilesCollection = new List<String>();
                        downLoadFilesCollection = recNums.Split(',').ToList();
                        containerNameConStr = (Request.QueryString["docType"]).Trim() + containerNameConStr;
                        storageConStr = (Request.QueryString["docType"]).Trim() + storageConStr;

                        String containerName = ConfigurationManager.ConnectionStrings[containerNameConStr].ConnectionString;

                        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                                    ConfigurationManager.ConnectionStrings[storageConStr].ConnectionString);

                        // Create the blob client.
                        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                        CloudBlobContainer container = blobClient.GetContainerReference(containerName);

                        //ZIp File Name
                        var downloadzipFileName = string.Format((Request.QueryString["docType"]).Trim(), DateTime.Now.ToString());
                        Response.ContentType = "application/zip";
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + downloadzipFileName + ".zip");
                        byte[] buffer = new byte[4096];
                        ZipOutputStream zipoutputstream = new ZipOutputStream(Response.OutputStream);
                        zipoutputstream.SetLevel(3);

                        // Create Zip File by looping in the collection
                        
                            foreach (String downloadFile in downLoadFilesCollection)
                            {
                                // Retrieve reference to a blob named blobName
                                CloudBlockBlob blockBlob = container.GetBlockBlobReference(downloadFile.Trim());
                                System.IO.MemoryStream mem = new System.IO.MemoryStream();
                                blockBlob.DownloadToStream(mem);
                                int count = (int)mem.Length;
                                ZipEntry zipEntry = new ZipEntry(ZipEntry.CleanName(downloadFile + ".tif"));
                                zipEntry.Size = count;
                                zipoutputstream.PutNextEntry(zipEntry);
                                while (count > 0)
                                {
                                    zipoutputstream.Write(mem.ToArray(), 0, count);
                                    count = mem.Read(mem.ToArray(), 0, count);
                                }

                            }

                            zipoutputstream.Close();
                            Response.Flush();
                            
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("404"))
                        {
                            Response.Write("No Document found");
                        }
                    }
                }
            }
            else
            {
                Response.Write("Please provide the parameters");
            }
        }

        private bool authorizedownload()
        {
            if (HttpContext.Current.Session["Role"] != null &&
                ((HttpContext.Current.Session["Role"].ToString().ToLower() == "dci") ||
                 (HttpContext.Current.Session["Role"].ToString().ToLower() == "apadmin") ||
                 (HttpContext.Current.Session["Role"].ToString().ToLower() == "aradmin") ||
                 (HttpContext.Current.Session["Role"].ToString().ToLower() == "apuser") ||
                 (HttpContext.Current.Session["Role"].ToString().ToLower() == "aruser") ||
                 (HttpContext.Current.Session["Role"].ToString().ToLower() == "apcaadmin") ||
                 (HttpContext.Current.Session["Role"].ToString().ToLower() == "apcauser") 
                )
               )

                return true;
            else
                return false;

        }
    }
}