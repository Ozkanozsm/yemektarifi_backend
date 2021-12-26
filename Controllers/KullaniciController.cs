using Microsoft.AspNetCore.Mvc;
using YemekTBackend.Models;
using YemekTBackend.Services;
using Google.Cloud.Firestore;

namespace YemekTBackend.Controllers
{
    [Route("api/kullanici")]
    [ApiController]
    public class KullaniciController : ControllerBase
    {
        [HttpGet("test")]
        public Task<ActionResult<Kullanici>> test()
        {
            return KullaniciService.Test();
        }

        [HttpGet("kullaniciwithid/{userID}")]
        public Task<ActionResult<Kullanici>> getKullaniciWithID(string userID)
        {
            return KullaniciService.getKullaniciWithID(userID);
        }

        [HttpPost("ekle")]
        public Task<ActionResult<Kullanici>> CreateUser(Kullanici user)
        {
            return KullaniciService.CreateUser(user);
        }

        [HttpPost("begen/{userID}/{recipeID}")]
        public Task<ActionResult<Kullanici>> LikeRecipe(string UserID, string recipeID)
        {
            return KullaniciService.LikeRecipe(UserID, recipeID);   
        }
    }
    
    
}
