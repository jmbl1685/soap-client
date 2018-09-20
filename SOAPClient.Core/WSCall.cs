using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SOAPClient.Core
{
    public static class WSCall
    {
        public static XDocument Client(XDocument xml, string uri)
        {
            XDocument soapResponse;

            try
            {
                using (var client = new HttpClient(new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip }))
                {
                    var request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri($"{uri}?wsdl"),
                        Method = HttpMethod.Post
                    };

                    request.Content = new StringContent(xml.ToString(), Encoding.UTF8, "text/xml");

                    request.Headers.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
                    request.Headers.Add("SOAPAction", uri);

                    HttpResponseMessage response = client.SendAsync(request).Result;

                    if (!response.IsSuccessStatusCode)
                        throw new ArgumentException($"{response.ReasonPhrase}");

                    Task<Stream> streamTask = response.Content.ReadAsStreamAsync();
                    Stream stream = streamTask.Result;
                    var sr = new StreamReader(stream);
                    soapResponse = XDocument.Load(sr);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return soapResponse;
        }
    }
}
