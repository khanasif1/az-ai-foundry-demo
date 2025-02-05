using System;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace dotnet_plugin
{
    internal class AzureComputerVision
    {
        // Add your Computer Vision key and endpoint
        static string key = string.Empty;
        static string endpoint = string.Empty;



        string storageAccountName = "skdocumentintelligence";
        string containerName = "sk-document-intelligence-demo";
                
        string sasToken = string.Empty;

        private readonly IConfiguration _configuration;
        public AzureComputerVision(IConfiguration configuration)
        {
            _configuration = configuration;
            sasToken = _configuration["AppSettings:gpt-apikey"];
            key = _configuration["AppSettings:vision-key"];
            endpoint = _configuration["AppSettings:vision-endpoint"];
            if (string.IsNullOrEmpty(sasToken) || string.IsNullOrEmpty(key) || string.IsNullOrEmpty(endpoint))
            {
                throw new InvalidOperationException("Storage sasToken, or Vision key or endpoint is not set in the configuration");
            }
        }

        public async Task<IList<ReadResult>> ExecuteRead(string pdfPath)
        {
            Console.WriteLine("Azure Cognitive Services Computer Vision");
            Console.WriteLine();

            ComputerVisionClient client = Authenticate(endpoint, key);

            // Extract text (OCR) from a URL image using the Read API
            var azureComputerVision = new AzureComputerVision(_configuration);
            return azureComputerVision.ExtractTextFromPDF(client, pdfPath).GetAwaiter().GetResult();
        }

        public static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
              new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
              { Endpoint = endpoint };
            return client;
        }

        public async Task<IList<ReadResult>> ExtractTextFromPDF(ComputerVisionClient client, string pdfPath)
        {
            //string pdfPath = "Asif DL.pdf";
            string blobName = "SK_Invoice.pdf";


            string blobUrl = pdfPath;

            string outputFilePath = Path.Combine(Directory.GetCurrentDirectory(), blobName);

            await DownloadBlobAsync($"{blobUrl}?{sasToken}", outputFilePath);

            using (var stream = new FileStream(blobName, FileMode.Open))
            {
                var textHeaders = await client.ReadInStreamAsync(stream);
                string operationLocation = textHeaders.OperationLocation;
                Thread.Sleep(2000);

                const int numberOfCharsInOperationId = 36;
                string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

                ReadOperationResult results;
                do
                {
                    results = await client.GetReadResultAsync(Guid.Parse(operationId));
                }
                while ((results.Status == OperationStatusCodes.Running ||
                        results.Status == OperationStatusCodes.NotStarted));

                var textUrlFileResults = results.AnalyzeResult.ReadResults;

                // Convert textUrlFileResults to json
                string json = JsonConvert.SerializeObject(textUrlFileResults, Formatting.Indented);


                //foreach (ReadResult page in textUrlFileResults)
                //{
                //    foreach (Line line in page.Lines)
                //    {
                //        Console.WriteLine(line.Text);
                //        lines.Add(line.Text);
                //    }
                //}
                //return lines;
                return textUrlFileResults;
            }
        }

        static async Task DownloadBlobAsync(string blobUrl, string filePath)
        {
            try
            {
                // Create BlobClient using the blob URL (SAS Token included)
                BlobClient blobClient = new BlobClient(new Uri(blobUrl));

                // Download Blob and Save to FileStream
                using (BlobDownloadInfo download = await blobClient.DownloadAsync())
                using (FileStream fileStream = File.OpenWrite(filePath))
                {
                    await download.Content.CopyToAsync(fileStream);
                }

                Console.WriteLine($"File downloaded successfully: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading blob: {ex.Message}");
            }
        }
    }
}
