using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace UpdaterCore
{
    public class WeebClient : WebClient
    {
        #region Properties
        public Uri URL { get; private set; }
        public CookieContainer CookieJar { get; private set; }
        #endregion

        public WeebClient(CookieContainer cookieJar)
        {
            base.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            this.CookieJar = cookieJar;
        }

        public WeebClient()
        {
            base.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            this.CookieJar = new CookieContainer();
        }

        #region Static Functions
        internal static bool ValidateCertificate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors errors) => (errors == SslPolicyErrors.None);
        #endregion

        #region Overrides
        protected override WebResponse GetWebResponse(WebRequest request)
        {
            WebResponse response = base.GetWebResponse(request);

            URL = response.ResponseUri;

            return response;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateCertificate);

            WebRequest request = base.GetWebRequest(address);

            if (request is HttpWebRequest)
                ((HttpWebRequest)request).CookieContainer = CookieJar;
            ((HttpWebRequest)request).AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            return request;
        }
        #endregion
    }
}
