using Microsoft.AspNetCore.Mvc;

namespace YemekTBackend.Controllers
{
    public class HomeController : Controller
    {
        [Route(""), HttpGet]
        public RedirectResult RedirectToSwaggerUi()
        {
            // Root sayfasını api sayfasına yönlendirme
            return Redirect("/swagger/");
        }
    }
}
