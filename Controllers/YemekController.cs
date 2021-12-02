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
        [HttpGet("test")]
        public Task<ActionResult<Dictionary<string, object>>> test()
        {
            return YemekService.getYemek();
        }


        [HttpGet("all")]
        public ActionResult<List<Yemek>> GetAll()
        {
            return YemekService.GetAll();
        }
    }
}
