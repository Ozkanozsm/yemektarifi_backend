using YemekTBackend.Models;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;

namespace YemekTBackend.Services
{
    public class YemekService
    {
        static FirestoreDb database;
        static YemekService()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"Config/yemektarifi-8bc5d-firebase-adminsdk.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            database = FirestoreDb.Create("yemektarifi-8bc5d");
        }

        public static async Task<ActionResult<Dictionary<string, object>>> GetYemek()
        {
            var veri = new Dictionary<string, object>();
            DocumentReference docref = database.Collection("yemekler").Document("0080ec05-3118-4c3b-83a8-a7e1db5959da");
            DocumentSnapshot snap = await docref.GetSnapshotAsync();
            if (snap.Exists)
            {
                Dictionary<string, object> dctveri = snap.ToDictionary();
                foreach (var item in dctveri)
                {
                    veri.Add(item.Key, item.Value);
                }
            }
            return veri;
        }

        public static async Task<ActionResult<List<Yemek>>> GetAllYemek()
        {
            List<Yemek> veri = new();
            CollectionReference colref = database.Collection("yemekler");
            QuerySnapshot allYemeks = await colref.GetSnapshotAsync();
            foreach (DocumentSnapshot document in allYemeks.Documents)
            {
                Yemek _yemekim = document.ConvertTo<Yemek>();
                veri.Add(_yemekim);
            }
            return veri;
        }

        public static async Task<ActionResult<Yemek>> GetYemekwithID(string idstr)
        {
            List<Yemek> veri = new();
            Yemek bizimYemek;
            CollectionReference colref = database.Collection("yemekler");
            QuerySnapshot allYemeks = await colref.GetSnapshotAsync();
            Query istenenYemek = colref.WhereEqualTo("yemekID", idstr);
            foreach (DocumentSnapshot document in allYemeks.Documents)
            {
                Yemek _yemekim = document.ConvertTo<Yemek>();
                veri.Add(_yemekim);
            }
            bizimYemek = veri.FirstOrDefault(yemek => yemek.yemekID == idstr);
            return bizimYemek;
        }

        public static async Task<ActionResult<Yemek>> PutNewYemek(YemekData _yemek)
        {
            Yemek yeniYemek = new Yemek();

            yeniYemek.hazirlanmaSuresi = _yemek.hazirlanmaSuresi;
            yeniYemek.kategori = _yemek.kategori;
            yeniYemek.malzemeler = _yemek.malzemeler;
            yeniYemek.olusturanID = _yemek.olusturanID;
            yeniYemek.imageURL = _yemek.imageURL;
            yeniYemek.olusturanUserName = _yemek.olusturanUserName;
            yeniYemek.yemekIsim = _yemek.yemekIsim;
            yeniYemek.yemekTarif = _yemek.yemekTarif;

            yeniYemek.yemekID = Guid.NewGuid().ToString();
            yeniYemek.adminOnayi = 0;
            yeniYemek.begenenler = new List<string>();
            yeniYemek.olusturmaTarihi = DateTime.Now.ToUniversalTime();

            DocumentReference docref = database.Collection("yemekler").Document(yeniYemek.yemekID);
            await docref.SetAsync(yeniYemek);

            //kullanıcının eklediği yemeklere yemeği ekleme
            KullaniciService.AddUserAddedList(yeniYemek.olusturanID, yeniYemek.yemekID);


            return yeniYemek;

        }

        public static async Task<int> YemekAdminDuzenle(YemekAdmin yemek)
        {
            var gelenid = yemek.yemekID;

            DocumentSnapshot docsnap = await database.Collection("yemekler").Document(gelenid).GetSnapshotAsync();
            DocumentReference docref = docsnap.Reference;
            Dictionary<FieldPath, object> updates = new()
            {
                { new FieldPath("adminOnayi"), 1 },
                { new FieldPath("hazirlanmaSuresi"), yemek.hazirlanmaSuresi },
                { new FieldPath("kategori"), yemek.kategori },
                { new FieldPath("malzemeler"), yemek.malzemeler },
                { new FieldPath("yemekIsim"), yemek.yemekIsim },
                { new FieldPath("yemekTarif"), yemek.yemekTarif }
            };
            await docref.UpdateAsync(updates);
            return 1;
        }

        public static async Task<ActionResult<Yemek>> YemekEdit(string idstr, int komut)
        {
            Yemek bizimYemek = new();
            DocumentReference docref;
            CollectionReference colref = database.Collection("yemekler");
            QuerySnapshot Yemeks = await colref.GetSnapshotAsync();
            foreach (DocumentSnapshot document in Yemeks.Documents)
            {
                docref = document.Reference;
                if (docref.Id == idstr)
                {
                    Dictionary<FieldPath, object> updates = new()
                    {
                        { new FieldPath("adminOnayi"), komut }
                    };
                    await docref.UpdateAsync(updates);
                }
            }
            return bizimYemek;
        }

        public static async Task<int> DeleteYemek(string recipeID)
        {
            /*
            Yemek bizimYemek = new();
            DocumentReference docref;
            CollectionReference colref = database.Collection("yemekler");
            QuerySnapshot Yemeks = await colref.GetSnapshotAsync();
            foreach (DocumentSnapshot document in Yemeks.Documents)
            {
                docref = document.Reference;

                if (docref.Id == idstr)
                {
                    await docref.DeleteAsync();
                }
            }
            return bizimYemek;
            */

            DocumentReference docref = database.Collection("yemekler").Document(recipeID);
            DeleteRecipeFromAllUsers(recipeID);
            await docref.DeleteAsync();
            return 0;
        }

        public static async Task<ActionResult<List<Kullanici>>> GetLikes(string recipeID)
        {
            //TRY CATCH EKLE
            DocumentSnapshot recipe = await database.Collection("yemekler").Document(recipeID).GetSnapshotAsync();
            List<Kullanici> likes = new List<Kullanici>();

            foreach (string userID in recipe.GetValue<List<string>>("begenenler"))
            {

                try
                {
                    DocumentSnapshot userSnap = await database.Collection("kullanicilar").Document(userID).GetSnapshotAsync();
                    Kullanici user = userSnap.ConvertTo<Kullanici>();
                    likes.Add(user);

                }
                catch (InvalidOperationException)
                {

                }

            }

            return likes;
        }

        public static async Task<bool> checkRecipeIDIsExist(string recipeID)
        {
            DocumentSnapshot docSnap = await database.Collection("yemekler").Document(recipeID).GetSnapshotAsync();
            if (docSnap.Exists)
            {
                return true;
            }
            return false;
        }

        public static async Task<int> GetRecipesAdminOnayi(string recipeID)
        {
            DocumentSnapshot docSnap = await database.Collection("yemekler").Document(recipeID).GetSnapshotAsync();
            return docSnap.GetValue<int>("adminOnayi");

        }

        public static async Task<int> GetRecipesLikeCount(string recipeID)
        {
            DocumentSnapshot docSnap = await database.Collection("yemekler").Document(recipeID).GetSnapshotAsync();
            return docSnap.GetValue<List<string>>("begenenler").Count;
        }

        public static async void DeleteRecipeFromAllUsers(string recipeID)
        {
            QuerySnapshot querySnap = await database.Collection("kullanicilar").GetSnapshotAsync();
            
            foreach(DocumentSnapshot docSnap in querySnap)
            {
                if (docSnap.GetValue<List<string>>("likedRecipes").Contains(recipeID)){
                    List<string> newLikedList = docSnap.GetValue<List<string>>("likedRecipes");
                    newLikedList.Remove(recipeID);

                    await docSnap.Reference.UpdateAsync(new Dictionary<FieldPath, object>
                    {
                        {new FieldPath("likedRecipes"), newLikedList }
                    });
                }

                if (docSnap.GetValue<List<string>>("addedRecipes").Contains(recipeID))
                {
                    List<string> newAddedList = docSnap.GetValue<List<string>>("addedRecipes");
                    newAddedList.Remove(recipeID);

                    await docSnap.Reference.UpdateAsync(new Dictionary<FieldPath, object>
                    {
                        {new FieldPath("addedRecipes"), newAddedList }

                    });
                }
            }
        }  



    }
}
