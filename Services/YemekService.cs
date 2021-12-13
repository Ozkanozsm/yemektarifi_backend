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
            Yemekler.Add(new Yemek() { yemekID = "1", yemekIsim = "ÇORBAAA" });
            Yemekler.Add(new Yemek() { yemekID = "2", yemekIsim = "YEMEQQ" });
        }

        public static List<Yemek> GetAll() => Yemekler;


        public static async Task<ActionResult<Dictionary<string, object>>> getYemek()
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

        public static async Task<ActionResult<List<Yemek>>> getallYemek()
        {
            List<Yemek> veri = new List<Yemek>();
            CollectionReference colref = database.Collection("yemekler");
            QuerySnapshot allYemeks = await colref.GetSnapshotAsync();
            foreach (DocumentSnapshot document in allYemeks.Documents)
            {
                // Do anything you'd normally do with a DocumentSnapshot
                Yemek _yemekim = document.ConvertTo<Yemek>();
                veri.Add(_yemekim);
            }
            return veri;

        }

        public static async Task<ActionResult<Yemek>> getYemekwithID(string idstr)
        {
            List<Yemek> veri = new List<Yemek>();
            Yemek bizimYemek;
            CollectionReference colref = database.Collection("yemekler");
            QuerySnapshot allYemeks = await colref.GetSnapshotAsync();
            Query istenenYemek = colref.WhereEqualTo("yemekID", idstr);
            //TODO
            foreach (DocumentSnapshot document in allYemeks.Documents)
            {
                // Do anything you'd normally do with a DocumentSnapshot
                Yemek _yemekim = document.ConvertTo<Yemek>();
                veri.Add(_yemekim);
                _yemekim.yemekID = idstr;
                // ise

            }
            bizimYemek = veri.FirstOrDefault(yemek => yemek.yemekID == idstr);
            // LINQ
            return bizimYemek;

        }

        public static async Task<ActionResult<Yemek>> putNewYemek(Yemek _yemek)
        {
            // TODO gecici obje oluturup onu Yemek'e convert et
            CollectionReference colref = database.Collection("yemekler");
            
            _yemek.adminOnayi = 0;
            _yemek.begenenler = new List<string>();

            _yemek.olusturmaTarihi = DateTime.Now.ToString();
            //TODO FIREBASE

            var yeniguid = System.Guid.NewGuid().ToString();
            _yemek.yemekID = yeniguid;
            // kullanmaya gerek yok gibi, firebase kendisi unique bir deger atiyor

            await colref.AddAsync(_yemek);
            return _yemek;
        }

        public static async Task<ActionResult<Yemek>> yemekDuzenle(string idstr, int komut)
        {
            List<Yemek> veri = new List<Yemek>();
            Yemek bizimYemek;
            List<DocumentSnapshot> docreflist = new List<DocumentSnapshot>();
            DocumentReference docref;
            CollectionReference colref = database.Collection("yemekler");

            QuerySnapshot allYemeks = await colref.GetSnapshotAsync();
            foreach (DocumentSnapshot document in allYemeks.Documents)
            {
                docreflist.Add(document);
               
                // Do anything you'd normally do with a DocumentSnapshot
                Yemek _yemekim = document.ConvertTo<Yemek>();
                if (_yemekim.yemekID == idstr)
                {
                    
                };
                _yemekim.adminOnayi += 1;

                if (_yemekim.yemekID==idstr)
                {
                    docref = document.Reference;
                    
                }
                veri.Add(_yemekim);
            }

            bizimYemek = veri.FirstOrDefault(yemek => yemek.yemekID == idstr);
            bizimYemek.adminOnayi = komut;
            //await docref.SetAsync(bizimYemek);
            return bizimYemek;

        }



        /*
        public static async Task<ActionResult<Yemek>> putNewBlob(Google.Cloud.Firestore.Blob veri)
        {
            // TODO gecici obje oluturup onu Yemek'e convert et
            CollectionReference colref = database.Collection("yemekler");
            _yemek.adminOnayi = 0;
            _yemek.begenenler = new List<string>();
            _yemek.olusturanID = 0;
            _yemek.olusturmaTarihi = DateTime.Now.ToString();

            // var yeniguid = System.Guid.NewGuid().ToString();
            // kullanmaya gerek yok gibi, firebase kendisi unique bir deger atiyor

            await colref.AddAsync(_yemek);
            return _yemek;
        }
        */



    }
}
