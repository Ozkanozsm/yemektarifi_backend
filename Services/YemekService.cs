using YemekTBackend.Models;
using Google.Cloud.Firestore;

namespace YemekTBackend.Services
{
    public class YemekService
    {
        public static List<Yemek> Yemekler { get; set; }
        static YemekService()
        {
            Yemekler = new List<Yemek>();
            Yemekler.Add(new Yemek() { yemekID = 1, yemekIsim = "ÇORBAAA" });
            Yemekler.Add(new Yemek() { yemekID = 2, yemekIsim = "YEMEQQ" });
        }

        public static List<Yemek> GetAll() => Yemekler;


    }
}
