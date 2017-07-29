using System.Web.Mvc;

namespace UrlsAndRoutes.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.Controller = "Home";
            ViewBag.Action = "Index";

            return View("ActionName");
        }

        public ActionResult CustomVariable(string id = "DefaultId")
        {
            ViewBag.Controller = "Home";
            ViewBag.Action = "Index";
            ViewBag.CustomVariable = id;

            return View();
        }

        public RedirectToRouteResult MyActionMethod()
        {
            string myActionUrl = Url.Action("Index", new { id = "MyId" });

            string myRouteUrl = Url.RouteUrl(new { controller = "Home", action = "Index" });

            // do something with URLs...

            return RedirectToRoute(new { controller = "Home", action = "Index", id = "MyId" });
        }
    }
}