using Microsoft.AspNetCore.Mvc;

namespace HttpRequestHandlerSample.ApiApp.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            var req = this.Request;
            return Ok(new string[] { "value1", "value2" });
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]object value)
        {
            var req = this.Request;
            return Ok(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]object value)
        {
            return Ok(value);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
