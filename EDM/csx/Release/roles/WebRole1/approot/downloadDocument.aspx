<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="downloadDocument.aspx.cs" Inherits="WebRole1.downloadDocument" %>
<%@ Import Namespace="Microsoft.WindowsAzure.Storage" %>
<%@ Import Namespace="Microsoft.WindowsAzure.Storage.Blob" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>

<script runat=server>
void Page_Load(object sender, EventArgs e)
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
</script>
