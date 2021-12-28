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
        [HttpGet("all")]
        public Task<ActionResult<List<Yemek>>> GetAllYemek()
        {
            // sistemdeki tüm yemekleri döndürme
            return YemekService.GetAllYemek();
        }

        [HttpGet("yemekwithid/{yemekid}")]
        public async Task<ActionResult<Yemek>> GetYemekwithid(string yemekid)
        {
            // id'si verilen yemeği döndürme
            var _yemek = YemekService.GetYemekwithID(yemekid).Result;
            if (_yemek.Value == null)
            {
                // yemek yok ise
                return StatusCode(404); // not found
            }
            return _yemek;
        }

        [HttpPost("ekle")]
        public async Task<ActionResult<Yemek>> YemekEkle(YemekData _yemek)
        {
            // yeni yemek ekleme
            if (KullaniciService.CheckUserIDIsExist(_yemek.olusturanID).Result)
            {
                return await YemekService.PutNewYemek(_yemek);
            }
            return StatusCode(404); // not found
        }

        [HttpPost("sil/{yemekid}")]
        public async Task<ActionResult<Yemek>> YemekSil(string yemekid)
        {
            // yemek silme
            if (YemekService.CheckRecipeIDIsExist(yemekid).Result)
            {
                await YemekService.DeleteYemek(yemekid);
                return StatusCode(200);
            }
            return StatusCode(404); // not found
        }

        [HttpPost("duzenle/{yemekid}/{komut}")]
        public async Task<ActionResult<Yemek>> YemekDuzenle(string yemekid, int komut)
        {
            // yemek düzenleme
            if (YemekService.CheckRecipeIDIsExist(yemekid).Result)
            {
                await YemekService.YemekEdit(yemekid, komut);
                if (YemekService.GetRecipesAdminOnayi(yemekid).Result == komut)
                {
                    return StatusCode(200);
                }
                return StatusCode(503);
            }
            return StatusCode(404); // not found
        }

        [HttpPost("duzenleAdmin")]
        public async Task<ActionResult<Yemek>> YemekDuzenleAdmin(YemekAdmin yemek)
        {
            // admin paneli üzerinden yemek düzenleme
            await YemekService.YemekDuzenleAdmin(yemek);
            return Ok();
        }

        [HttpPost("duzenleKullanici")]
        public async Task<ActionResult<Yemek>> YemekDuzenleKullanici(YemekAdmin yemek)
        {
            // kullanıcı tarafından yemek düzenleme
            await YemekService.YemekDuzenleKullanici(yemek);
            return Ok();
        }

        [HttpGet("begenenler/{yemekid}")]
        public Task<ActionResult<List<Kullanici>>> GetLikes(string yemekid)
        {
            // id'si verilen yemeğin beğeni sayısını alma
            return YemekService.GetLikes(yemekid);
        }
    }
}
