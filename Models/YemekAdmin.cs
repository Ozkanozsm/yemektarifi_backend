using Google.Cloud.Firestore;
using System.Collections;


namespace YemekTBackend.Models
{
    public class YemekAdmin
    {
        public int hazirlanmaSuresi { get; set; }
        public string kategori { get; set; }
        public List<string> malzemeler { get; set; }
        public string yemekIsim { get; set; }
        public string yemekTarif { get; set; }
        public string yemekID { get; set; }
    }
}
