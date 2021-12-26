using YemekTBackend.Models;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;

namespace YemekTBackend.Services
{
    public class KullaniciService
    {
        static FirestoreDb database;
        static KullaniciService()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"Config/yemektarifi-8bc5d-firebase-adminsdk.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            database = FirestoreDb.Create("yemektarifi-8bc5d");
        }

        

        
        public static async Task<ActionResult<Kullanici>> Test()
        {
            var data = new Kullanici();
            CollectionReference colref = database.Collection("kullanicilar");
            QuerySnapshot Kullanicilar = await colref.GetSnapshotAsync();

            foreach(DocumentSnapshot document in Kullanicilar.Documents)
            {
                data = document.ConvertTo<Kullanici>();
                return data;
                
            }
            return data;
        }
        
        
        public static async Task<ActionResult<Kullanici>> getKullaniciWithID(string idstr)
        {
            

            CollectionReference colref = database.Collection("Kullanicilar");
            QuerySnapshot users = await colref.GetSnapshotAsync();

            foreach(DocumentSnapshot document in users.Documents)
            {
                if(document.Id == idstr)
                {
                    return document.ConvertTo<Kullanici>();
                }
            }

            return null;

        }
        

        public static async Task<ActionResult<Kullanici>> CreateUser(Kullanici user)
        {
            CollectionReference colref = database.Collection("kullanicilar");
            DocumentReference docref;
            user.isAdmin = false;
            user.addedRecipes = new List<string>();
            user.likedRecipes = new List<string>();
            
            //TODO match firebase id and user id
            docref = await colref.AddAsync(user);

            FirebaseService.MatchIDs(docref);

            return user;

        }

        public static async Task<ActionResult<Kullanici>> LikeRecipe(string userID, string recipeID)
        {
            //TRY CATCH EKLE
            DocumentReference user = database.Collection("kullanicilar").Document(userID);
            DocumentReference recipe = database.Collection("yemekler").Document(recipeID);

            //tarifin beğenilerine kullanıcıyı ekle
            //kullanıcının beğenilerine tarifi ekle
            return new Kullanici();
        }
    }
}
