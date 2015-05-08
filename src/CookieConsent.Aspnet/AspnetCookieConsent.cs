using System.Web;
using CookieConsent.Service;

namespace CookieConsent.Aspnet
{


    public sealed class AspnetCookieConsent
    {
        private static readonly object Sync = new object();
        private static AspnetCookieConsent _instance;
        
        private readonly ConsentService _consentService;
        private readonly IFileShim _fileShim;

        private string _cachedHtmlFileContent;

        public static ConsentSettings Settings { get; set; }

        private AspnetCookieConsent(IFileShim fileShim, ConsentService consentService)
        {
            _fileShim = fileShim;
            _consentService = consentService;
        }

        static AspnetCookieConsent()
        {
            Settings = new ConsentSettings();
        }

        public static AspnetCookieConsent Default
        {
            get
            {
                if (_instance == null)
                {
                    lock (Sync)
                    {
                        var wwwroot = HttpContext.Current.Server.MapPath("\\");
                        _instance = new AspnetCookieConsent(
                            new FileShim(wwwroot),
                            new ConsentService(new HttpContextCookieStorage()));
                            
                        _instance.PreloadAssets();
                    }
                }
                return _instance;
            }
        }

        private void PreloadAssets()
        {
            _cachedHtmlFileContent = _fileShim.ReadAllText(Settings.HtmlFileLocation);
        }
        
        /// <summary>
        /// Renders HTML of a cookie consent with specific culture translation. 
        /// If no culture info is provided, default culture is used (English)
        /// </summary>
        /// <param name="culture">Culture code</param>
        /// <returns>HTML content of a cookie consent</returns>
        public string RenderConsent(string culture = "default")
        {
            if (HttpContext.Current.IsDebuggingEnabled)
                _cachedHtmlFileContent = _fileShim.ReadAllText(Settings.HtmlFileLocation);

            return _consentService.RenderNotificationHtml(Settings, _cachedHtmlFileContent, culture, HttpContext.Current.IsDebuggingEnabled);
        }
        
    }
}