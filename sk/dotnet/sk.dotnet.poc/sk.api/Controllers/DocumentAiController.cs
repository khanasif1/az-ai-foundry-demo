using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace sk.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentAiController : ControllerBase
    {
        AiPlugin aiPlugin = new AiPlugin();
        
        // GET: api/<DocumentAiController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            aiPlugin.CallAiOrchestrator("read pdf and generate a html report with modern html UI using bootstrap. The report should have a high quality user experience. It should have header banner and footer information. All the cost breakdown should be create with quick to understand experience and clean html user interface.");
            return new string[] { "value1", "value2" };
        }

        //// GET api/<DocumentAiController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<DocumentAiController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<DocumentAiController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<DocumentAiController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
