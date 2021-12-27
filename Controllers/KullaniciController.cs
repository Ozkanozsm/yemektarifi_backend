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
            if (KullaniciService.CheckUserIDIsExist(userID).Result)
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


        [HttpPost("begen/{userID}/{recipeID}/{komut}")]
        public async Task<ActionResult<int>> LikeRecipe(string userID, string recipeID, bool komut)
        {
            if(KullaniciService.CheckUserIDIsExist(userID).Result && YemekService.checkRecipeIDIsExist(recipeID).Result)
            {
                //kullanıcı yemeği beğendiyse ve komut sil komutu ise
                //kullanıcı yemeği beğenmediyse ve komut ekle komutu ise
                if (KullaniciService.IsUserLikedRecipe(userID, recipeID).Result != komut)
                {
                    await KullaniciService.LikeRecipe(userID, recipeID, komut);
                    return YemekService.GetRecipesLikeCount(recipeID).Result;
                }

                return StatusCode(409);
               
            }
            return NotFound();

        }

        [HttpGet("begen/{userID}")]
        public async Task<ActionResult<List<Yemek>>> getLikedRecipes(string userID)
        {
            
            if (KullaniciService.CheckUserIDIsExist(userID).Result)
            {
                return await KullaniciService.getLikedRecipes(userID);
            }
            return StatusCode(404);//notfound
            
            //return await KullaniciService.getLikedRecipes(userID);
        }

        [HttpGet("added/{userID}")]
        public async Task<ActionResult<List<Yemek>>> getAddedRecipes(string userID)
        {
            if (KullaniciService.CheckUserIDIsExist(userID).Result)
            {
                return await KullaniciService.getAddedRecipes(userID);
            }
            return StatusCode(404);//notfound
        }

        

    }


}
