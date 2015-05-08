using System.IO;
using Nancy;

namespace CookieConsent.Example.OWIN
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = x => File.ReadAllText("index.html");
        }
    }
}