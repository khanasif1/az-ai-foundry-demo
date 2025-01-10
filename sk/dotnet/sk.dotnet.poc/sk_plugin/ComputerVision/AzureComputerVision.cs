using System;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Linq;

namespace dotnet_plugin
{
    internal class AzureComputerVision
    {
        // Add your Computer Vision key and endpoint
        static string key = "FDQGHx2nrS9VvYVpMFRkZBQPsiewAGNF4NLRsX9IkCxXLAozIoRuJQQJ99ALACYeBjFXJ3w3AAAFACOGQL8n";//Environment.GetEnvironmentVariable("VISION_KEY");
        static string endpoint = "https://demo-ocr-computer-vision.cognitiveservices.azure.com/";//Environment.GetEnvironmentVariable("VISION_ENDPOINT");

        private const string READ_TEXT_URL_IMAGE = "Asif DL.pdf";
        private List<string> lines = new List<string>();

        public async Task<IList<ReadResult>> ExecuteRead()
        {
            Console.WriteLine("Azure Cognitive Services Computer Vision - .NET quickstart example");
            Console.WriteLine();

            ComputerVisionClient client = Authenticate(endpoint, key);

            // Extract text (OCR) from a URL image using the Read API
            var azureComputerVision = new AzureComputerVision();
            return azureComputerVision.ExtractTextFromPDF(client).GetAwaiter().GetResult();
        }

        public static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
              new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
              { Endpoint = endpoint };
            return client;
        }

        public async Task<IList<ReadResult>> ExtractTextFromPDF(ComputerVisionClient client)
        {
            //string pdfPath = "Asif DL.pdf";
            string pdfPath = "SK_Invoice.pdf";
            using (var stream = new FileStream(pdfPath, FileMode.Open))
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
    }
}
