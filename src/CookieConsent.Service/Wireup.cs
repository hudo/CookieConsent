using System;

namespace CookieConsent.Service
{
    public class Wireup
    {
        public static SettingsWireup Init()
        {
            return new SettingsWireup();
        }

        public class SettingsWireup
        {
            internal static ConsentSettings Settings;

            static SettingsWireup()
            {
                Settings = new ConsentSettings();
            }

            public ConsentSettings GetSettings()
            {
                return Settings;
            }

            public SettingsWireup WithLocalizedContent(string culture, string title, string description, string learnMoreTitle, string closeTitle, string learnMoreUrl = null)
            {
                Settings.LocalizedContentSettings.Add(
                    culture.ToLower(),
                    new ConsentSettings.LocalizedContent()
                    {
                        Title = title,
                        Description = description,
                        LearnMoreLink = learnMoreUrl,
                        LearnMoreLinkText = learnMoreTitle,
                        CloseButtonTitle = closeTitle
                    });
                
                return this;
            }

            public SettingsWireup WithDefaultContent(
                string title = "Cookie Consent", 
                string description = "This website uses cookies", 
                string learnMoreTitle = "Learn more", 
                string closeTitle = "Accept",
                string learnMoreUrl = "http://en.wikipedia.org/wiki/Directive_on_Privacy_and_Electronic_Communications")
            {
                if (Settings.LocalizedContentSettings.ContainsKey("default"))
                    Settings.LocalizedContentSettings.Remove("default");

                Settings.FallbackCulture = "default";
                return WithLocalizedContent("default", title, description, learnMoreTitle, closeTitle, learnMoreUrl);
            }

            public SettingsWireup SetDefaultFallbackCulture(string culture)
            {
                Settings.FallbackCulture = culture;
                return this;
            }

            public SettingsWireup WithConsentHtml(string htmlLocation)
            {
                Settings.HtmlFileLocation = htmlLocation;
                return this;
            }

            public SettingsWireup WithJavascriptLocation(string jsLocation)
            {
                Settings.JavascriptFileLocation = jsLocation;
                return this;
            }
        }
    }
}