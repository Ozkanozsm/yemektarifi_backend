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
        public async Task<ActionResult<Yemek>> YemekEkle(Yemek _yemek)
        {
            // CONTROLLERI BURADA YAP
            if (KullaniciService.CheckUserIDIsExist(_yemek.olusturanID).Result)
            {
                return await YemekService.PutNewYemek(_yemek);
            }

            return NotFound();  
        }

        [HttpPost("sil/{yemekid}")]
        public async Task<ActionResult<Yemek>> YemekSil(string yemekid)
        {
            if (YemekService.checkRecipeIDIsExist(yemekid).Result)
            {
                return await YemekService.DeleteYemek(yemekid);
            }

            return NotFound();
        }

        [HttpPost("duzenle/{yemekid}/{komut}")]
        public async Task<ActionResult<Yemek>> YemekDuzenle(string yemekid, int komut)
        {
            if (YemekService.checkRecipeIDIsExist(yemekid).Result)
            {
                await YemekService.YemekEdit(yemekid, komut);

                if(YemekService.GetRecipesAdminOnayi(yemekid).Result == komut)
                {
                    return StatusCode(200);
                }

                return StatusCode(503);

            }
            return NotFound();
            

            

            
        }

        [HttpGet("begenenler/{yemekid}")]
        public Task<ActionResult<List<Kullanici>>> GetLikes(string yemekid)
        {
            return YemekService.GetLikes(yemekid);
        }

    }
}
