using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControllersAndActions.Controllers
{
    public class DerivedController : Controller
    {
        // GET: Derived
        public ActionResult Index()
        {
            ViewBag.Message = "Hello from the DerivedController Index method";

            return View("MyView");
        }

        public void ProduceOutput()
        {
            if(Server.MachineName == "LENOVO")
            {
                Response.Redirect("Basci/Index");
            }
            else
            {
                Response.Write("Controller: Derived, Action: ProduceOutput");
            }
        }
    }
}