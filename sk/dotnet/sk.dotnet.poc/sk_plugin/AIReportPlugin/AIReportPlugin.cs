using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Text.Json.Serialization;
using Microsoft.SemanticKernel;
using Microsoft.Extensions.Configuration;

public class AIReportPlugin
{
    private IConfiguration? __configuration;
 
    private const string ENDPOINT = "https://ai-hub-demo-basemodel.openai.azure.com/openai/deployments/gpt-4o/chat/completions?api-version=2024-02-15-preview";

    [KernelFunction("report_generation")]
    [Description("used the data in  Kernelargument recieved from ComputerVisionPlugin next convert it into html, Markup or text report to present to the users")]
    [return: Description("The response will be of type report as html, Markup or text in form of string")]

    public async Task<ReportModel?> GenerateReportAsync(Kernel kernel)
    {


        if (kernel.Data.TryGetValue("configuration", out object? configObj) && configObj is IConfiguration config)
        {
            __configuration = config;
        }
        if (__configuration == null)
        {
            throw new InvalidOperationException("Configuration is not set in AIReport");
        }
        string API_KEY = __configuration["AppSettings:gpt-apikey"];

        if (string.IsNullOrEmpty(API_KEY))
        {
            throw new InvalidOperationException("API key is not set in the configuration");
        }


        Console.WriteLine("Azure openAI Gpt4o for report generation");
        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Add("api-key", API_KEY);
            var payload = new
            {
                messages = new object[]
                {
                  new {
                      role = "system",
                      content = new object[] {
                          new {
                              type = "text",
                              text = "You are an AI assistant that helps people find information."
                          }
                      }
                  },
                  new {
                      role = "user",
                      content = new object[] {
                          new {
                              type = "text",
                              text = kernel.Data["page"]                          }
                      }
                  }
                },
                temperature = 0.7,
                top_p = 0.95,
                max_tokens = 800,
                stream = false
            };
            Console.WriteLine("request to generate report");
            var response = await httpClient.PostAsync(ENDPOINT, new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response from model :{JsonConvert.DeserializeObject < dynamic >(responseData)}");
                return new ReportModel { reporttext = responseData };
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}, {response.ReasonPhrase}");
                return new ReportModel { reporttext = "Error in generating report" };

            }
        }
        
    }
}

public class ReportModel
{
    [JsonPropertyName("reporttext")]
    public string reporttext { get; set; }

}