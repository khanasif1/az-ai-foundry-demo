using System.ComponentModel;
using System.Text.Json.Serialization;
using Microsoft.SemanticKernel;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

using dotnet_plugin;
using Newtonsoft.Json;


public class ComputerVisionPlugin
{

    

    [KernelFunction("read_pdf")]
    [Description("Read the PDF file which can include text as well text in the form of image, add the response to kernelarguments to be used by AIReportPlugin")]
    [return: Description("The response will be of type PDFPage, if the PDF reading fails an empty PDFPage object will be returned")]
    
    public async Task<PDFPage?> ReadPDFDataAsync(Kernel kernel)
    {
      


        AzureComputerVision acv = new AzureComputerVision();
        IList<ReadResult> lines = acv.ExecuteRead().GetAwaiter().GetResult();
        PDFPage _pdfPage = new PDFPage();
        //_pdfPage.pagedata = lines; // JsonConvert.SerializeObject(lines);
        _pdfPage.pagedata = lines[1].Lines.Select(x => x.Text).ToList();
        
        //GlobalArgs._args = _pdfPage;
        //argument.Add("page", _pdfPage);
        kernel.Data.Remove("page");
        //add page in kernel data only if it does not exist else remove and add
        if (!kernel.Data.ContainsKey("page"))
            kernel.Data.Add("page", JsonConvert.SerializeObject(_pdfPage.pagedata));
        else
        {
            kernel.Data.Remove("page");
            kernel.Data.Add("page", JsonConvert.SerializeObject(_pdfPage.pagedata));
        }


        return _pdfPage;
        //return new PDFPage { pagedata = "This is a sample response" };
    }
}

public class PDFPage
{
    [JsonPropertyName("pagedata")]
    public List<string> pagedata { get; set; }

}