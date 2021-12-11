using Google.Cloud.Firestore;
using System.Collections;


namespace YemekTBackend.Models
{
    [FirestoreData]
    public class Yemek
    {

        [FirestoreProperty("adminOnayi")]
        public int adminOnayi { get; set; }

        [FirestoreProperty("begenenler")]
        public List<string> begenenler { get; set; }

        [FirestoreProperty("hazirlanmaSuresi")]
        public int hazirlanmaSuresi { get; set; }

        [FirestoreProperty("kategori")]
        public string kategori { get; set; }

        [FirestoreProperty("malzemeler")]
        public List<string> malzemeler { get; set; }

        [FirestoreProperty("olusturanID")]
        public int olusturanID { get; set; }

        [FirestoreProperty("olusturmaTarihi")]
        public string olusturmaTarihi { get; set; }

        [FirestoreProperty("yemekID")]
        public string yemekID { get; set; }

        [FirestoreProperty("yemekIsim")]
        public string yemekIsim { get; set; }
        //fixed int->string

        [FirestoreProperty("yemekTarif")]
        public string yemekTarif { get; set; }

    }
}
