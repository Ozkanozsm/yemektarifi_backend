using Microsoft.AspNetCore.Mvc;
using YemekTBackend.Models;
using YemekTBackend.Services;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace YemekTBackend.Controllers
{
    [Route("api/yemek")]
    [ApiController]
    public class YemekController : ControllerBase
    {
        // GET: api/<YemekController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<YemekController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet("test")]
        public Task<Dictionary<string, object>> test()
        {
            return YemekService.test();
        }


        [HttpGet("all")]
        public ActionResult<List<Yemek>> GetAll()
        {
            return YemekService.GetAll();
        }

        // POST api/<YemekController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<YemekController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<YemekController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
