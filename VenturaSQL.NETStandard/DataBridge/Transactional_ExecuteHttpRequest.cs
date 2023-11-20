using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace VenturaSQL
{

    public static partial class Transactional
    {
        private static async Task<byte[]> ExecuteHttpRequestAsync(HttpConnector connector, MemoryStream memorystream)
        {
            // Send the request

            // Sharing one HttpClient for all requests:
            // https://www.aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/

            HttpClient client = VenturaSqlConfig.GetHttpClientFromFactory(connector);

            // Had no effect.
            // client.DefaultRequestHeaders.Accept.Clear();
            // client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/venturasql"));

            byte[] buffer = memorystream.GetBuffer();

            var content = new ByteArrayContent(buffer, 0, (int)memorystream.Length);

            // Needed. See FrameStreamInputFormatter.cs in project VenturaSQL.AspNetCore.Server.
            content.Headers.ContentType = new MediaTypeHeaderValue("application/venturasql");

            using (HttpResponseMessage response = await client.PostAsync(connector.Url, content))
            {

                // work in progress
                //if (!response.IsSuccessStatusCode)
                //{
                //    object xx = response.Content;
                //}

                response.EnsureSuccessStatusCode();

                byte[] response_array = await response.Content.ReadAsByteArrayAsync();

                return response_array;
            }
        }

    } // end of class
} // end of namespace

