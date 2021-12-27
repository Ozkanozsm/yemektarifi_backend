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
        public async Task<ActionResult<Kullanici>> getKullaniciWithID(string userID)
        {
            if (KullaniciService.checkUserIDIsExist(userID).Result)
            {
                return await KullaniciService.getKullaniciWithID(userID);
            }

            return StatusCode(404);//not found
            
        }

        [HttpPost("ekle")]
        public async Task<ActionResult<Kullanici>> CreateUser(Kullanici user)
        {

            if (!KullaniciService.checkIsAlreadyIn(user).Result)
            {
                return await KullaniciService.CreateUser(user);
            }
            
            return StatusCode(409);//conflict
        }


        [HttpPost("begen/{userID}/{recipeID}")]
        public Task<ActionResult<Kullanici>> LikeRecipe(string userID, string recipeID)
        {
            
            return KullaniciService.LikeRecipe(userID, recipeID);
        }

        [HttpGet("begen/{userID}")]
        public async Task<ActionResult<List<Yemek>>> getLikedRecipes(string userID)
        {
            if (KullaniciService.checkUserIDIsExist(userID).Result)
            {
                return await KullaniciService.getLikedRecipes(userID);
            }
            return StatusCode(404);//notfound
        }

        [HttpGet("added/{userID}")]
        public async Task<ActionResult<List<Yemek>>> getAddedRecipes(string userID)
        {
            if (KullaniciService.checkUserIDIsExist(userID).Result)
            {
                return await KullaniciService.getAddedRecipes(userID);
            }
            return StatusCode(404);//notfound
        }

    }


}
