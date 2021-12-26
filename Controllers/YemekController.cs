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
        public async Task<ActionResult<Yemek>> Test()
        {
            // TEST FUNCTION
            return NotFound();
        }

        [HttpGet("all")]
        public Task<ActionResult<List<Yemek>>> GetAllYemek()
        {
            // firebase'e erisiyor
            return YemekService.GetAllYemek();
        }

        [HttpGet("yemekwithid/{yemekid}")]
        public async Task<ActionResult<Yemek>> GetYemekwithid(string yemekid)
        {
            var _yemek = YemekService.GetYemekwithID(yemekid).Result;
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

            return YemekService.PutNewYemek(_yemek);
        }

        [HttpPost("sil/{yemekid}")]
        public Task<ActionResult<Yemek>> YemekSil(string yemekid)
        {
            return YemekService.DeleteYemek(yemekid);
        }

        [HttpPost("duzenle/{yemekid}/{komut}")]
        public Task<ActionResult<Yemek>> YemekDuzenle(string yemekid, int komut)
        {
            // CONTROLLERI BURADA YAP

            return YemekService.YemekEdit(yemekid, komut);
        }

        [HttpGet("begenenler/{yemekid}")]
        public Task<ActionResult<List<Kullanici>>> GetLikes(string yemekid)
        {
            return YemekService.GetLikes(yemekid);
        }

    }
}
