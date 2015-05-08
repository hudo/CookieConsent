using System;
using System.IO;
using CookieConsent.OWIN.LibOwin;
using CookieConsent.Service;
using Owin;

namespace CookieConsent.OWIN
{
    public static class AppBuilderExtension
    {
        public static void UseCookieConsent(this IAppBuilder appBuilder, 
            Action<Wireup.SettingsWireup> configuration, 
            Func<IOwinRequest, string> cultureCallback)
        {
            var settingsWireup = Wireup.Init();
            configuration(settingsWireup);
            var settings = settingsWireup.GetSettings();

            appBuilder.Use(typeof (ConsentMiddleware), new FileShim(String.Empty), settings, cultureCallback);
        }

        public static void UseCookieConsent(this IAppBuilder appBuilder)
        {
            UseCookieConsent(appBuilder, _ => {}, _ => "default");
        }

    }
}