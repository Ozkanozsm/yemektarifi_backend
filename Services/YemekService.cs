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

        public static async Task<ActionResult<Yemek>> PutNewYemek(Yemek _yemek)
        {
            DocumentReference docref;
            CollectionReference colref = database.Collection("yemekler");
            var yeniguid = Guid.NewGuid().ToString();
            _yemek.adminOnayi = 0;
            _yemek.begenenler = new List<string>();
            _yemek.olusturmaTarihi = DateTime.Now.ToString();
            _yemek.yemekID = yeniguid;
            docref = await colref.AddAsync(_yemek);
            FirebaseService.MatchYemekIDs(docref);
            return _yemek;
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

        public static async Task<ActionResult<Yemek>> DeleteYemek(string idstr)
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
                    await docref.DeleteAsync();
                }
            }
            return bizimYemek;
        }
    }
}
