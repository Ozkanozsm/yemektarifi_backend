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
            // firebase'e erisiyor
            return YemekService.getYemek();
        }


        [HttpGet("all")]
        public Task<ActionResult<List<Yemek>>> getallyemek()
        {
            // firebase'e erisiyor
            return YemekService.getallYemek();
        }

        [HttpPost("ekle")]
        public Task<ActionResult<Yemek>> YemekEkle(Yemek _yemek)
        {
            // CONTROLLERI BURADA YAP

            return YemekService.putNewYemek(_yemek);

        }



        [HttpGet("oldall")]
        public ActionResult<List<Yemek>> GetAll()
        {
            // localde calisiyor
            return YemekService.GetAll();
        }
    }
}
