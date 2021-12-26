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
        public async Task<ActionResult<Yemek>> test()
        {

            //Hamburger id: d9ecdd9f-f26d-4f1d-8c8e-91241096afff
            var _yemek = YemekService.getYemekwithID("d9ecdd9f-a-91241096afff").Result;
            if (_yemek.Value == null)
                return NotFound();

            return _yemek;
        }


        [HttpGet("all")]
        public Task<ActionResult<List<Yemek>>> getallyemek()
        {
            // firebase'e erisiyor
            return YemekService.getallYemek();
        }

        [HttpGet("yemekwithid/{yemekid}")]
        public async Task<ActionResult<Yemek>> getYemekwithid(string yemekid)
        {
            var _yemek = YemekService.getYemekwithID(yemekid).Result;
            if (_yemek.Value == null)
            {
                return NotFound();
            }
                        
            return _yemek;
        }



        [HttpPost("ekle")]
        public Task<ActionResult<Yemek>> YemekEkle(Yemek _yemek)
        {
            // CONTROLLERI BURADA YAP

            return YemekService.putNewYemek(_yemek);

        }

        [HttpPost("sil/{yemekid}")]
        public Task<ActionResult<Yemek>> YemekSil(string yemekid)
        {
            return YemekService.yemekSil(yemekid);
        }


        [HttpPost("duzenle/{yemekid}/{komut}")]
        public Task<ActionResult<Yemek>> YemekDuzenle(string yemekid, int komut)
        {
            // CONTROLLERI BURADA YAP

            return YemekService.yemekDuzenle(yemekid, komut);

        }





    }
}
