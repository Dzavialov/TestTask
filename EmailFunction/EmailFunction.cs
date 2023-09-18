using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Azure.KeyVault.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;

namespace EmailFunction
{
    public class EmailFunction
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName = "files";
        public EmailFunction()
        {
            var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            _blobServiceClient = new BlobServiceClient(connectionString);
        }
        [FunctionName("SendEmailFunction")]
        [return: SendGrid(ApiKey = "SendGridApiKey")]
        public SendGridMessage Run([BlobTrigger("files/{name}", Connection = "")]Stream myBlob, string name, ILogger log, Uri uri,
            IDictionary<string, string> metaData)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var lastUploadedBlob = containerClient.GetBlobs().OrderByDescending(m => m.Properties.LastModified).FirstOrDefault();
            var userEmail = metaData["UserEmail"];

            BlobClient client = containerClient.GetBlobClient(lastUploadedBlob.Name);
            Uri sasUri = client.GenerateSasUri(BlobSasPermissions.Write | BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddHours(1));

            var senderEmail = Environment.GetEnvironmentVariable("SenderEmail");

            var msg = new SendGridMessage()
            {
                From = new EmailAddress(senderEmail, "Dmytro"),
                Subject = "The file is successfully uploaded",
                PlainTextContent = "Link to your file: " + sasUri.AbsoluteUri
            };
            msg.AddTo(new EmailAddress(userEmail, "User"));

            return msg;
        }
    }
}
