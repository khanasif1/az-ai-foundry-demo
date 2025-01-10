//using System.ComponentModel;
//using System.Text.Json.Serialization;
//using Microsoft.SemanticKernel;
//using System;
//using System.Collections.Generic;
//using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
//using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
//using System.Threading.Tasks;
//using System.IO;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using System.Threading;
//using System.Linq;
//using dotnet_plugin;


//public class ContentSummaryPlugin
//{
 

 
//    [KernelFunction("read_pdf")]
//    [Description("Read the PDF file which can include text as well text in the form of image")]
//    [return: Description("The response will be of type PDFPage, if the PDF reading fails an empty PDFPage object will be returned")]
    
//    public async Task<PDFPage?> ReadPDFDataAsync()
//    {
//        AzureComputerVision acv = new AzureComputerVision();
//        List<string> lines = acv.ExecuteRead().GetAwaiter().GetResult();
//        PDFPage _pdfPage= new PDFPage();
//        _pdfPage.pagedata = JsonConvert.SerializeObject(lines);
//        return _pdfPage;
//        //return new PDFPage { pagedata = "This is a sample response" };
//    }
//}

//public class PDFPage
//{
//    [JsonPropertyName("pagedata")]
//    public string pagedata { get; set; }

//}