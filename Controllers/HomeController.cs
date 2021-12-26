using Microsoft.AspNetCore.Mvc;

namespace YemekTBackend.Controllers
{
    public class HomeController : Controller
    {
        
        [Route(""), HttpGet]
        public RedirectResult RedirectToSwaggerUi()
        {
            return Redirect("/swagger/");
        }        
    }
}
