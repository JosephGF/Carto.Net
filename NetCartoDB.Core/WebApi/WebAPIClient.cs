using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NetCarto.Core.Extensions;

namespace NetCarto.Core.WebApi
{
    public class WebAPIClient
    {
        public static Task<string> GetAsync(string uri)
        {
            //using (var _client = new HttpClient())
            //{
            var _client = new HttpClient();
                return _client.GetStringAsync(uri);
            //}
        }

        public static async Task<T> GetJsonAsync<T>(string uri) where T : new()
        {
            var response = await GetAsync(uri);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(response);
        }

        public static T GetJson<T>(string uri) where T : new()
        {
            var task = GetAsync(uri);
            task.Wait();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(task.Result);
        }

        public static Task<HttpResponseMessage> PostAsync(string uri)
        {
            return PostAsync(uri, string.Empty);
        }

        public static Task<HttpResponseMessage> PostAsync(string uri, object data)
        {
            using (var _client = new HttpClient())
            {
                HttpContent content = new StringContent(data.ToJson());
                return _client.PostAsync(uri, content);
            }
        }
    }
}
