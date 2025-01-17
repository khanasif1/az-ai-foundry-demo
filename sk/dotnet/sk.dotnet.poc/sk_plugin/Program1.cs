

//class Program1
//{
//    private const string API_KEY = "EwAfYEWgZWfzJwZjWgAcRnirKcEqZgJ6XjEUGeROrklnVMum3HiBJQQJ99ALACYeBjFXJ3w3AAAAACOGXzsT"; // Set your key here

//    private const string QUESTION = "Generate a small report for cost of staying in a hotel in sydney. Use html format to make it look good"; // Set your question here

//    private const string ENDPOINT = "https://ai-hub-demo-basemodel.openai.azure.com/openai/deployments/gpt-4o/chat/completions?api-version=2024-02-15-preview";
//    static async Task Main()
//    {
//       using (var httpClient = new HttpClient())
//        {
//            httpClient.DefaultRequestHeaders.Add("api-key", API_KEY);
//            var payload = new
//            {
//                messages = new object[]
//                {
//                  new {
//                      role = "system",
//                      content = new object[] {
//                          new {
//                              type = "text",
//                              text = "You are an AI assistant that helps people find information."
//                          }
//                      }
//                  },
//                  new {
//                      role = "user",
//                      content = new object[] {
//                          new {
//                              type = "text",
//                              text = QUESTION
//                          }
//                      }
//                  }
//                },
//                temperature = 0.7,
//                top_p = 0.95,
//                max_tokens = 800,
//                stream = false
//            };

//            var response = await httpClient.PostAsync(ENDPOINT, new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"));

//            if (response.IsSuccessStatusCode)
//            {
//                var responseData = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
//                Console.WriteLine(responseData);
//            }
//            else
//            {
//                Console.WriteLine($"Error: {response.StatusCode}, {response.ReasonPhrase}");
//            }
//        }
//    }
//}