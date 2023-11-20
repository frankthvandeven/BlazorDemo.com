using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

// https://code-maze.com/blazor-webassembly-httpclient/
// https://docs.microsoft.com/en-us/aspnet/core/blazor/call-web-api?view=aspnetcore-5.0

namespace Kenova.Client
{
    public static class KenovaHttp
    {

        /// <summary>
        /// For example: KenovaHttp.PostJsonAsync<LoginResult>("api/user/login", credentials)
        /// </summary>
        public static async Task<TOutput> PostJsonAsync<TOutput>(string requestUri, object value)
        {
            HttpClient client = KenovaClientConfig.ServiceProvider.GetService(typeof(HttpClient)) as HttpClient;

            HttpResponseMessage response = await client.PostAsJsonAsync<object>(requestUri, value);

            if (response.IsSuccessStatusCode == false)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"Server reported problem. Http status code {response.StatusCode}.");

                sb.AppendLine($"BaseAddress {client.BaseAddress}");
                sb.AppendLine($"URI {requestUri}");
                sb.AppendLine($"ReasonPhrase {response.ReasonPhrase}");

                foreach (var hd in response.Headers)
                {
                    sb.AppendLine($"Header {hd}");
                }


                string data = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(data))
                {
                    sb.AppendLine($"Data received: {data}");
                }

                throw new HttpRequestException(sb.ToString());
            }

            // PropertyNameCaseInsensitive makes the json parser throw an exception.
            //var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            //Activator.CreateInstance<TOutput>();

            TOutput output = default;

            try
            {
                output = await response.Content.ReadFromJsonAsync<TOutput>();
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"There was a problem parsing the data that was received from the server. Expected type {typeof(TOutput).FullName}");

                string data = await response.Content.ReadAsStringAsync();

                sb.AppendLine($"The length of the data is {data.Length}.");
                sb.AppendLine($"First 30 characters are: {data.Substring(0, 30)}");
                throw new Exception(sb.ToString());
            }

            return output;
        }



    }
}
