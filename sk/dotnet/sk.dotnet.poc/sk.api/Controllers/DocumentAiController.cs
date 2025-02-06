using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace sk.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentAiController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public DocumentAiController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

       

        // GET: api/<DocumentAiController>
        [HttpGet]
        public async Task<string> Get(string prompt, string filePath)
        {
            AiPlugin aiPlugin = new AiPlugin(_configuration);
            //"read pdf and generate a html report with modern html UI using bootstrap. The report should have a high quality user experience. It should have header banner and footer information. All the cost breakdown should be create with quick to understand experience and clean html user interface."
            var result = await aiPlugin.CallAiOrchestrator(prompt, filePath);
            string _result =  result.ToString().Replace("html","");

            return result.ToString().Replace("```", "");

        }
    }
}