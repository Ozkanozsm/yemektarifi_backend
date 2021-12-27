using Google.Cloud.Firestore;
using System.Collections;


namespace YemekTBackend.Models
{
    public class YemekData
    {
        public int hazirlanmaSuresi { get; set; }
        public string kategori { get; set; }
        public List<string> malzemeler { get; set; }
        public string olusturanID { get; set; }
        public string imageURL { get; set; }
        public string olusturanUserName { get; set; }
        public string yemekIsim { get; set; }
        public string yemekTarif { get; set; }
        public string yemekTempTarih { get; set; }
    }
}
