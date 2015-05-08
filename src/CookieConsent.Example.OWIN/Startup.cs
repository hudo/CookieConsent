using System;
using System.Security.Cryptography.X509Certificates;
using CookieConsent.OWIN;
using Owin;

namespace CookieConsent.Example.OWIN
{
    public class Startup
    {
        public void Configuration(IAppBuilder builder)
        {
            builder.Use(async (ctx, next) =>
            {
                Console.WriteLine(ctx.Request.Uri);
                await next();
            });

            builder.UseCookieConsent();

            builder.UseNancy();
        }
    }
}