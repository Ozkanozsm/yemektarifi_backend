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
            DocumentReference docref;
            QuerySnapshot Kullanicilar = await colref.GetSnapshotAsync();

            foreach(DocumentSnapshot document in Kullanicilar.Documents)
            {
                data = document.ConvertTo<Kullanici>();
                return data;
                
            }
            return data;
        }

        /*
        public static async Task<ActionResult<Kullanici>> getKullaniciWithID(string idstr)
        {

        }
        */
    }
}
