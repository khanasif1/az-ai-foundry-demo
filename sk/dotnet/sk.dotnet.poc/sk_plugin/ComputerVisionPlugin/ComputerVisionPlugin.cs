using System.ComponentModel;
using System.Text.Json.Serialization;
using Microsoft.SemanticKernel;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

using dotnet_plugin;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;


public class ComputerVisionPlugin
{
    private IConfiguration? __configuration;

    [KernelFunction("read_pdf")]
    [Description("Read the PDF file which can include text as well text in the form of image, add the response to kernelarguments to be used by AIReportPlugin")]
    [return: Description("The response will be of type PDFPage, if the PDF reading fails an empty PDFPage object will be returned")]
    public async Task<PDFPage?> ReadPDFDataAsync(Kernel kernel)
    {
        PDFPage _pdfPage = new PDFPage();

        if (kernel.Data.TryGetValue("configuration", out object? configObj) && configObj is IConfiguration config)
        {
            __configuration = config;
        }

        if (kernel.Data.TryGetValue("filePath", out var filePathObj) && filePathObj is string pdfPath)
        {
            AzureComputerVision acv = new AzureComputerVision(__configuration);
            IList<ReadResult> lines = acv.ExecuteRead(pdfPath).GetAwaiter().GetResult();
            //_pdfPage.pagedata = lines; // JsonConvert.SerializeObject(lines);
            _pdfPage.pagedata = lines[1].Lines.Select(x => x.Text).ToList();

            kernel.Data.Remove("page");
            //add page in kernel data only if it does not exist else remove and add
            if (!kernel.Data.ContainsKey("page"))
                kernel.Data.Add("page", JsonConvert.SerializeObject(_pdfPage.pagedata));
            else
            {
                kernel.Data.Remove("page");
                kernel.Data.Add("page", JsonConvert.SerializeObject(_pdfPage.pagedata));
            }
        }

        return _pdfPage;
    }
}

public class PDFPage
{
    [JsonPropertyName("pagedata")]
    public List<string> pagedata { get; set; }

}