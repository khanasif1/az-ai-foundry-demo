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

public class AIReportPlugin
{


    private const string API_KEY = "EwAfYEWgZWfzJwZjWgAcRnirKcEqZgJ6XjEUGeROrklnVMum3HiBJQQJ99ALACYeBjFXJ3w3AAAAACOGXzsT"; // Set your key here

    private const string QUESTION = "Generate a small report for cost of staying in a hotel in sydney. Use html format to make it look good"; // Set your question here

    private const string ENDPOINT = "https://ai-hub-demo-basemodel.openai.azure.com/openai/deployments/gpt-4o/chat/completions?api-version=2024-02-15-preview";

    [KernelFunction("report_generation")]
    [Description("Json or text will be received as input, convert it into html, Markup or text report to present to the users")]
    [return: Description("The response will be of type report as html, Markup or text in form of string")]

    public async Task<ReportModel?> GenerateReportAsync()
    {
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
                              text = QUESTION
                          }
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