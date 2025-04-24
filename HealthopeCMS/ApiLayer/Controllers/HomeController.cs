using System.Web.Mvc;

namespace ApiLayer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //return View();
            return File("~/wwwroot/index.html", "text/html");
        }
    }
}
