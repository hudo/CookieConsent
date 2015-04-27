using System.Collections.Generic;
using System.Threading.Tasks;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace CookieConsent.OWIN.Microsoft
{
    
    public class ConsentMiddleware
    {
        private readonly AppFunc _next;

        public ConsentMiddleware(AppFunc next)
        {
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> env)
        {
            await _next(env);
        }
    }
}