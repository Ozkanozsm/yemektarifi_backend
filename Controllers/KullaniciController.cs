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
        [HttpGet("kullaniciwithid/{userID}")]
        public async Task<ActionResult<Kullanici>> GetKullaniciWithID(string userID)
        {
            // id'si verilen kullanıcıyı alma
            if (KullaniciService.CheckUserIDIsExist(userID).Result)
            {
                // kullanıcı zaten var ise id'yi döndürme
                return await KullaniciService.GetKullaniciWithID(userID);
            }
            return StatusCode(404); // not found
        }

        [HttpPost("ekle")]
        public async Task<ActionResult<Kullanici>> CreateUser(Kullanici user)
        {
            // yeni kullanıcı ekleme
            if (!KullaniciService.CheckIsAlreadyIn(user).Result && KullaniciService.IsValidEmail(user.eMail))
            {
                // eğer kullanıcı zaten yok ise ve eposta valid ise
                return await KullaniciService.CreateUser(user);
            }
            return StatusCode(409); // conflict
        }


        [HttpPost("begen/{userID}/{recipeID}/{komut}")]
        public async Task<ActionResult<int>> LikeRecipe(string userID, string recipeID, bool komut)
        {
            if (KullaniciService.CheckUserIDIsExist(userID).Result && YemekService.CheckRecipeIDIsExist(recipeID).Result)
            {
                // kullanıcı yemeği beğendiyse ve komut sil komutu ise
                // kullanıcı yemeği beğenmediyse ve komut ekle komutu ise
                if (KullaniciService.IsUserLikedRecipe(userID, recipeID).Result != komut)
                {
                    await KullaniciService.LikeRecipe(userID, recipeID, komut);
                    return YemekService.GetRecipesLikeCount(recipeID).Result;
                }
                return StatusCode(409); // conflict
            }
            return NotFound(); // not found
        }

        [HttpGet("begen/{userID}")]
        public async Task<ActionResult<List<Yemek>>> GetLikedRecipes(string userID)
        {
            // kullanıcının beğendiği yemekleri döndürme
            if (KullaniciService.CheckUserIDIsExist(userID).Result)
            {
                return await KullaniciService.GetLikedRecipes(userID);
            }
            return StatusCode(404); // notfound
        }

        [HttpGet("added/{userID}")]
        public async Task<ActionResult<List<Yemek>>> GetAddedRecipes(string userID)
        {
            // kullanıcının eklediği yemekleri döndürme
            if (KullaniciService.CheckUserIDIsExist(userID).Result)
            {
                return await KullaniciService.GetAddedRecipes(userID);
            }
            return StatusCode(404); // notfound
        }

        [HttpPost("update/{userID}")]
        public async Task<ActionResult<Kullanici>> UpdateUser(KullaniciUpdate user, string userID)
        {
            // kullanıcı güncelleme
            return await KullaniciService.UpdateUser(user, userID);
        }
    }
}
