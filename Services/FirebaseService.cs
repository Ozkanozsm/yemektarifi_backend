using YemekTBackend.Models;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;

namespace YemekTBackend.Services
{
    public class FirebaseService
    {
        public static async void MatchIDs(DocumentReference docref)
        {
            Dictionary<FieldPath, object> update = new Dictionary<FieldPath, object>
            {
                {new FieldPath("userID"), docref.Id }
            };

            await docref.UpdateAsync(update);
        }
    }
}
