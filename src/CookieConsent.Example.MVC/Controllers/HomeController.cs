using System.Web.Mvc;
using CookieConsent.Example.MVC.Models;

namespace CookieConsent.Example.MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string lang)
        {
            var model = new HomeViewModel {Culture = lang};
            return View(model);
        }
    }
}