using Microsoft.AspNetCore.Mvc;
using YemekTBackend.Models;
using YemekTBackend.Services;
using Google.Cloud.Firestore;

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

        [HttpGet("yemekwithid/{yemekid}")]
        public Task<ActionResult<Yemek>> getYemekwithid(string yemekid)
        {
            return YemekService.getYemekwithID(yemekid);
        }

        [HttpPost("ekle")]
        public Task<ActionResult<Yemek>> YemekEkle(Yemek _yemek)
        {
            // CONTROLLERI BURADA YAP

            return YemekService.putNewYemek(_yemek);

        }

        /*
        [HttpPost("duzenle/{yemekid}/{komut}")]
        public Task<ActionResult<Yemek>> YemekDuzenle(string yemekid, int komut)
        {
            // CONTROLLERI BURADA YAP

            return YemekService.yemekDuzenle(yemekid, komut);

        }
        */




    }
}
