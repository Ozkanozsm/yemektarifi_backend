using YemekTBackend.Models;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;

namespace YemekTBackend.Services
{
    public class YemekService
    {
        static FirestoreDb database;
        public static List<Yemek> Yemekler { get; set; }
        static YemekService()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"Configs/yemektarifi-8bc5d-firebase-adminsdk.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            database = FirestoreDb.Create("yemektarifi-8bc5d");
            Yemekler = new List<Yemek>();
            Yemekler.Add(new Yemek() { yemekID = 1, yemekIsim = "ÇORBAAA" });
            Yemekler.Add(new Yemek() { yemekID = 2, yemekIsim = "YEMEQQ" });
        }

        public static List<Yemek> GetAll() => Yemekler;

        /*
        // HATA: https://youtu.be/SrRrxYBR3s0?t=385
        public static async Task<ActionResult<Yemek>> getYemek()
        {
            Yemek _yemek;
            var veri = new Dictionary<string, object>();
            DocumentReference docref = database.Collection("yemekler").Document("09a24398-d8b6-4f02-ad3b-209fd4aaa8b1");
            DocumentSnapshot snap = await docref.GetSnapshotAsync();
            if (snap.Exists)
            {
                _yemek = snap.ConvertTo<Yemek>();
                Dictionary<string, object> dctveri = snap.ToDictionary();
                foreach (var item in dctveri)
                {
                    veri.Add(item.Key, item.Value);
                }
                return _yemek;
            }
            return new Yemek() { yemekID = 1, yemekIsim = "TEMP YEMEK" };
        }
        */


        public static async Task<ActionResult<Dictionary<string, object>>> getYemek()
        {
            var veri = new Dictionary<string, object>();
            DocumentReference docref = database.Collection("yemekler").Document("09a24398-d8b6-4f02-ad3b-209fd4aaa8b1");
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

        /*
        public static async Task<ActionResult<List<Dictionary<string, object>>>> getAllYemek()
        {
            List<Yemek> yemeks = new List<Yemek>();
            Query Qref = database.Collection("yemekler");
            QuerySnapshot snap = await Qref.GetSnapshotAsync();
            foreach (DocumentSnapshot docsnap in snap)
            {
                Yemek yemek = docsnap.ConvertTo<Yemek>();
                yemeks.Add(yemek);
            }

            //return new Yemek() { };
        }
        */




    }
}
