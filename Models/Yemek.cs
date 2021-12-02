using Google.Cloud.Firestore;
using System.Collections;


namespace YemekTBackend.Models
{
    [FirestoreData]
    public class Yemek
    {

        // TODO BOZUK string to Int32
        [FirestoreProperty]
        public int adminOnayi { get; set; }

        [FirestoreProperty]
        public List<string> begenenler { get; set; }

        [FirestoreProperty]
        public int hazirlanmaSuresi { get; set; }

        [FirestoreProperty]
        public string kategori { get; set; }

        [FirestoreProperty]
        public List<string> malzemeler { get; set; }

        [FirestoreProperty]
        public int olusturanID { get; set; }

        [FirestoreProperty]
        public string olusturmaTarihi { get; set; }

        [FirestoreProperty]
        public string yemekIsim { get; set; }

        [FirestoreProperty]
        public int yemekID { get; set; }


    }
}
