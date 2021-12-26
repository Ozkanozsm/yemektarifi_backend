using Google.Cloud.Firestore;
using System.Collections;

namespace YemekTBackend.Models
{
    [FirestoreData]
    public class Kullanici
    {
        [FirestoreProperty("userID")]
        public string userID { get; set; }

        [FirestoreProperty("userName")]
        public string userName { get; set; }

        [FirestoreProperty("eMail")]
        public string eMail { get; set; }

        [FirestoreProperty("addedRecipes")]
        public List<string> addedRecipes { get; set; }

        [FirestoreProperty("likedRecipes")]
        public List<string> likedRecipes { get; set; }

        [FirestoreProperty("creationDate")]
        public string creationDate { get; set; }

        [FirestoreProperty("isAdmin")]
        public bool isAdmin { get; set; }

        [FirestoreProperty("password")]
        public string password { get; set; }

        [FirestoreProperty("imageURL")]
        public string imageURL { get; set; }







    }     
}
