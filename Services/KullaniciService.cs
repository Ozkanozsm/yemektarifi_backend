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
            try
            {
                DocumentSnapshot user = await database.Collection("kullanicilar").Document(idstr).GetSnapshotAsync();
                return user.ConvertTo<Kullanici>();
            }   
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public static async Task<ActionResult<Kullanici>> CreateUser(Kullanici user)
        {
            
            DocumentReference docref = database.Collection("kullanicilar").Document(user.userID);

            user.isAdmin = false;
            user.addedRecipes = new List<string>();
            user.likedRecipes = new List<string>();
            user.creationDate = DateTime.Now.ToString();

            await docref.SetAsync(user);

            return user;

        }

        public static async Task<ActionResult<Kullanici>> LikeRecipe(string userID, string recipeID, bool komut)
        {

            DocumentSnapshot user = await database.Collection("kullanicilar").Document(userID).GetSnapshotAsync();
            DocumentSnapshot recipe = await database.Collection("yemekler").Document(recipeID).GetSnapshotAsync();

            //tarifin beğenilerine kullanıcıyı ekle
            //kullanıcının beğenilerine tarifi ekle

            List<string> newLikedList = user.GetValue<List<string>>("likedRecipes");
            List<string> newBegenenlerList = recipe.GetValue<List<string>>("begenenler");


            if(komut)
            {
                newLikedList.Add(recipeID);
                newBegenenlerList.Add(userID);

            }
            else
            {
                newLikedList.Remove(recipeID);
                newBegenenlerList.Remove(userID);

            }
            

            await user.Reference.UpdateAsync(new Dictionary<FieldPath, object>
            {
                {new FieldPath("likedRecipes"), newLikedList}
            });

            await recipe.Reference.UpdateAsync(new Dictionary<FieldPath, object>
            {
                {new FieldPath("begenenler"), newBegenenlerList}
            });

            return user.ConvertTo<Kullanici>();

        }


        public static async Task<ActionResult<List<Yemek>>> getLikedRecipes(string userID)
        {
            
            DocumentSnapshot user = await database.Collection("kullanicilar").Document(userID).GetSnapshotAsync();
            
            List<Yemek> likedRecipes = new List<Yemek>();
            foreach (string recipeID in user.GetValue<List<string>>("likedRecipes"))
            {
                try
                {
                    DocumentSnapshot recipeSnap = await database.Collection("yemekler").Document(recipeID).GetSnapshotAsync();
                    Yemek recipe = recipeSnap.ConvertTo<Yemek>();
                    likedRecipes.Add(recipe);

                }
                catch (InvalidOperationException)
                {

                }
                
            }

            return likedRecipes;
        }
        public static async Task<ActionResult<List<Yemek>>> getAddedRecipes(string userID)
        {
            //try catch
            DocumentSnapshot user = await database.Collection("kullanicilar").Document(userID).GetSnapshotAsync();

            List<Yemek> addedRecipes = new List<Yemek>();
            foreach (string recipeID in user.GetValue<List<string>>("addedRecipes"))
            {
                try
                {
                    DocumentSnapshot recipeSnap = await database.Collection("yemekler").Document(recipeID).GetSnapshotAsync();
                    Yemek recipe = recipeSnap.ConvertTo<Yemek>();
                    addedRecipes.Add(recipe);

                }
                catch (InvalidOperationException)
                {

                }

            }

            return addedRecipes;
        }

        public static async Task<ActionResult<Kullanici>> UpdateUser(KullaniciUpdate user, string userID)
        {
            DocumentSnapshot docSnap = await database.Collection("kullanicilar").Document(userID).GetSnapshotAsync();
            DocumentReference docref = docSnap.Reference;

            Dictionary<FieldPath, object> updates = new()
            {
                { new FieldPath("userName"), user.userName },
                { new FieldPath("imageURL"), user.imageURL }
            };

            await docref.UpdateAsync(updates);
            return docSnap.ConvertTo<Kullanici>();
        }

        public static async Task<bool> checkIsAlreadyIn(Kullanici user)
        {
            QuerySnapshot colSnap = await database.Collection("kullanicilar").GetSnapshotAsync();

            string eMail = user.eMail;

            foreach(DocumentSnapshot docsnap in colSnap)
            {
                if (docsnap.GetValue<string>("eMail").Equals(eMail))
                {
                    return true;
                }
            }

            return false;

        }

        public static async Task<bool> CheckUserIDIsExist(string userID)
        {
            DocumentSnapshot docSnap = await database.Collection("kullanicilar").Document(userID).GetSnapshotAsync();
            if (docSnap.Exists)
            {
                return true;
            }
            return false;
        }

        public static async void AddUserAddedList(string userID, string recipeID)
        {
            DocumentSnapshot user = await database.Collection("kullanicilar").Document(userID).GetSnapshotAsync();

            List<string> newAddedList = user.GetValue<List<string>>("addedRecipes");
            newAddedList.Add(recipeID);

            await user.Reference.UpdateAsync(new Dictionary<FieldPath, object>
            {
                {new FieldPath("addedRecipes"), newAddedList}
            });

            return;
        }

        public static async Task<bool> IsUserLikedRecipe(string userID, string recipeID)
        {
            DocumentSnapshot user = await database.Collection("kullanicilar").Document(userID).GetSnapshotAsync();
            List<string> likedList = user.GetValue<List<string>>("likedRecipes");

            if (likedList.Contains(recipeID))
            {
                return true;
            }
            return false;
        }


        public static bool IsValidEmail(string eMail)
        {
            try
            {
                var _eMail = new System.Net.Mail.MailAddress(eMail);
                return true;
            }
            catch
            {
                return false;
            }
        }

        
    }
}

