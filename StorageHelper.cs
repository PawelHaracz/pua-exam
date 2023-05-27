using System.Text;
using Azure.Storage;
using Azure.Storage.Blobs;

namespace WebStorageSample
{
    public class StorageHelper
    {
        public static async Task UploadBlob(string accountName, string accountKey, string containerName, string blobName, string blobContents)
        {
            var sharedKeyCredential =
                new StorageSharedKeyCredential(accountName, accountKey);
            var containerEndpoint = "https://" + accountName + ".blob.core.windows.net";
            var blobContainerUri = new Uri(new Uri(containerEndpoint), containerName);
            BlobContainerClient containerClient = new BlobContainerClient(blobContainerUri, sharedKeyCredential);

            try
            {
                // Create the container if it does not exist.
                await containerClient.CreateIfNotExistsAsync();

                BlobClient blobClient = containerClient.GetBlobClient(blobName);

                // Upload text to a new block blob.
                byte[] byteArray = Encoding.ASCII.GetBytes(blobContents);

                using (MemoryStream stream = new MemoryStream(byteArray))
                {
                    await blobClient.UploadAsync(stream, overwrite: true);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static async Task<string> GetBlob(string accountName, string accountKey, string containerName, string blobName)
        {
            var sharedKeyCredential =
                new StorageSharedKeyCredential(accountName, accountKey);
            var containerEndpoint = "https://" + accountName + ".blob.core.windows.net";
            var blobContainerUri = new Uri(new Uri(containerEndpoint), containerName);
            BlobContainerClient containerClient = new BlobContainerClient(blobContainerUri, sharedKeyCredential);

            try
            {
                // Create the container if it does not exist.
                await containerClient.CreateIfNotExistsAsync();

                BlobClient blobClient = containerClient.GetBlobClient(blobName);
                if (await blobClient.ExistsAsync())
                {
                    var response = await blobClient.DownloadAsync();
                    using (var streamReader = new StreamReader(response.Value.Content))
                    {
                        while (!streamReader.EndOfStream)
                        {
                            var line = await streamReader.ReadLineAsync();
                            Console.WriteLine(line);
                            return line;
                        }
                    }
                }
                return "";
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
