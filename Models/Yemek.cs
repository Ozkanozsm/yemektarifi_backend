using Google.Cloud.Firestore;

namespace YemekTBackend.Models
{
    [FirestoreData]
    public class Yemek
    {
        [FirestoreProperty]
        public string yemekIsim { get; set; }

        [FirestoreProperty]
        public int yemekID { get; set; }
    }
}
