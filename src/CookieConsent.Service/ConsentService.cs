using System;
using System.Collections.Concurrent;

namespace CookieConsent.Service
{
    public class ConsentService
    {
        private const string COOKIE_KEY = "CookieConsent";

        private readonly ICookieStorage _storage;

        private static readonly ConcurrentDictionary<string, string> CachedConsentHtml = new ConcurrentDictionary<string, string>();

        public ConsentService(ICookieStorage storage)
        {
            _storage = storage;
        }

        public string RenderNotificationHtml(ConsentSettings settings, string htmlContent, string culture, bool isDebug)
        {
            var cookieContent = _storage.Read(COOKIE_KEY);

            return string.IsNullOrEmpty(cookieContent) ? GenerateConsentHtml(settings, htmlContent, culture, isDebug) : string.Empty;
        }

        private string GenerateConsentHtml(ConsentSettings settings, string htmlContent, string culture, bool isDebug)
        {
            if (string.IsNullOrWhiteSpace(culture) || !CachedConsentHtml.ContainsKey(culture))
                culture = settings.FallbackCulture;

            if (isDebug) return ApplyTemplate(settings, htmlContent, culture);

            return CachedConsentHtml.GetOrAdd(culture, x => ApplyTemplate(settings, htmlContent, culture));
        }

        private static string ApplyTemplate(ConsentSettings settings, string htmlContent, string culture)
        {
            var template = htmlContent;
            var mappings = settings.GetMappings(culture) ?? settings.GetMappings(settings.FallbackCulture);

            if (mappings == null) throw new Exception("Can't find culture and fallback culture");

            foreach (var mapping in mappings)
                template = template.Replace(string.Format("{{{0}}}", mapping.Key), mapping.Value);

            return template;
        }
    }
}