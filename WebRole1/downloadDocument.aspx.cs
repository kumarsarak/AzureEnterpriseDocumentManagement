using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace WebRole1
{
    public partial class downloadDocument : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String recNum = "default";
            String containerNameConStr = "ContainerConnectionString";
            String storageConStr = "StorageConnectionString";

            if (Request.QueryString["recordNumber"] != null && Request.QueryString["docType"] != null)
            {
                recNum = (Request.QueryString["recordNumber"]).Trim();
                containerNameConStr = (Request.QueryString["docType"]).Trim() + containerNameConStr;
                storageConStr = (Request.QueryString["docType"]).Trim() + storageConStr;

                String containerName = ConfigurationManager.ConnectionStrings[containerNameConStr].ConnectionString;

                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                            ConfigurationManager.ConnectionStrings[storageConStr].ConnectionString);

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);

                // Retrieve reference to a blob named blobName
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(recNum);

                System.IO.MemoryStream mem = new System.IO.MemoryStream();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + recNum + ".tif");
                blockBlob.DownloadToStream(mem);
                Response.BinaryWrite(mem.ToArray());
            }
            else
            {
                Response.Write("Error!!  Please add more error handling logic here .....");
            }
        }
    }
}