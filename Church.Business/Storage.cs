using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Business
{
    public class Storage
    {
        public string conStr { get; set; }
        public string StorageRoot { get; set; }

        public Storage(string conStr, string StorageRoot)
        {
            this.conStr = conStr;
            this.StorageRoot = StorageRoot;
        }

        public bool ValidateImagesAndPdf(string fileName)
        {

            if (Path.GetExtension(fileName).ToLower() != ".jpg"
                && Path.GetExtension(fileName).ToLower() != ".jpeg"
                && Path.GetExtension(fileName).ToLower() != ".png"
                && Path.GetExtension(fileName).ToLower() != ".pdf")
                return false;
            else
                return true;
        }

        public string createBlobFromStream(string containerName, string blobName, Stream file, string contentType)
        {
            try
            {
                // Retrieve storage account from connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(conStr);

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve reference to a previously created container.
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);

                // Retrieve reference to a blob named "myblob".
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
                blockBlob.Properties.ContentType = contentType;

                // Create or overwrite the "myblob" blob with contents from a local file.
                blockBlob.UploadFromStream(file);

                return StorageRoot + containerName + "/" + blobName;
            }
            catch (Exception gEx)
            {
                return "Ha ocurrido un Error: -" + gEx;
            }
        }


        public bool deleteBlob(string containerName, string blobName)
        {
            try
            {
                // Retrieve storage account from connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(conStr);

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve reference to a previously created container.
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);

                // Retrieve reference to a blob named "myblob.txt".
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);

                // Delete the blob.
                blockBlob.Delete();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
