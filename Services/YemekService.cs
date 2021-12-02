using YemekTBackend.Models;
using Google.Cloud.Firestore;

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

        public static async Task<Dictionary<string, object>> test()
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

    }
}
