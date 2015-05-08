using CookieConsent.OWIN.LibOwin;
using CookieConsent.Service;

namespace CookieConsent.OWIN
{
    internal class CookieStorage : ICookieStorage
    {
        private readonly IOwinContext _owinContext;

        public CookieStorage(IOwinContext owinContext)
        {
            _owinContext = owinContext;
        }

        public string Read(string key)
        {
            return _owinContext.Request.Cookies[key];
        }

        public void Store(string key, string content)
        {
            _owinContext.Response.Cookies.Append(key, content);
        }
    }
}