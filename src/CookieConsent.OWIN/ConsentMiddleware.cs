using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CookieConsent.OWIN.LibOwin;
using CookieConsent.Service;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace CookieConsent.OWIN
{
    
    internal class ConsentMiddleware
    {
        private readonly AppFunc _next;
        private static ConsentSettings _settings;
        private static Func<IOwinRequest, string> _cultureCallback;
        private static string _cachedHtmlContent;

        public ConsentMiddleware(AppFunc next, IFileShim fileShim, ConsentSettings settings, Func<IOwinRequest, string> cultureCallback)
        {
            _next = next;
            _settings = settings;
            _cultureCallback = cultureCallback;

            _cachedHtmlContent = fileShim.ReadAllText(settings.HtmlFileLocation);
        }

        public async Task Invoke(IDictionary<string, object> env)
        {
            var context = new OwinContext(env);

            var originalBody = context.Response.Body;
            var bufferBody = new MemoryStream();
            context.Response.Body = bufferBody;

            await _next(env);

            if (context.Response.ContentType != "text/html")
            {
                await FlushOriginalContent(bufferBody, originalBody);
            }
            else
            {
                await ModifyHtmlOutput(bufferBody, originalBody, context);
            }
        }

        private static async Task ModifyHtmlOutput(MemoryStream bufferBody, Stream originalBody, IOwinContext owinContext)
        {
            var consentService = new ConsentService(new CookieStorage(owinContext));
            
            bufferBody.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(bufferBody);
            var stringBuilder = new StringBuilder(await reader.ReadToEndAsync());

            var content = "<script src=\"" 
                + _settings.JavascriptFileLocation + "\"></script>"
                + consentService.RenderNotificationHtml(_settings, _cachedHtmlContent, _cultureCallback(owinContext.Request), false)
                + "</body>";

            stringBuilder.Replace("</body>", content);

            using (var memoryStream = new MemoryStream(stringBuilder.Length))
            {
                var streamWriter = new StreamWriter(memoryStream);
                streamWriter.Write(stringBuilder.ToString());
                streamWriter.Flush();
                memoryStream.Seek(0, SeekOrigin.Begin);

                await memoryStream.CopyToAsync(originalBody);
            }
        }

        private static async Task FlushOriginalContent(MemoryStream bufferBody, Stream originalBody)
        {
            bufferBody.Seek(0, SeekOrigin.Begin);
            await bufferBody.CopyToAsync(originalBody);
        }
    }
}