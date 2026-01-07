using System.Web.Mvc;

namespace MovieStudioWebApplication.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

    }
}
