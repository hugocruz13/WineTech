using System;
using System.ServiceModel;

namespace DAL.Helpers
{
    public static class SoapEndpointResolver
    {
        private const string DefaultSoapBaseUrl = "http://soap:8080";

        public static string BuildServiceUrl(string serviceName)
        {
            var baseUrl = Environment.GetEnvironmentVariable("SOAP_BASE_URL");
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                baseUrl = DefaultSoapBaseUrl;
            }

            return $"{baseUrl.TrimEnd('/')}/Services/{serviceName}.asmx";
        }

        public static BasicHttpBinding CreateBinding(string serviceUrl)
        {
            var uri = new Uri(serviceUrl);
            var securityMode = uri.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase)
                ? BasicHttpSecurityMode.Transport
                : BasicHttpSecurityMode.None;

            return new BasicHttpBinding(securityMode)
            {
                MaxReceivedMessageSize = 10 * 1024 * 1024
            };
        }

        public static EndpointAddress CreateEndpointAddress(string serviceUrl)
        {
            return new EndpointAddress(serviceUrl);
        }
    }
}