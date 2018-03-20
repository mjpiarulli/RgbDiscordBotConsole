using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace RgbDiscordBotConsole
{
    public class DiscordApiWrapper
    {
        private readonly string _urlBase;

        public DiscordApiWrapper()
        {
            _urlBase = "https://discordapp.com/api/v6/";
        }

        public DiscordApiWrapper(string urlBase)
        {
            _urlBase = urlBase;
        }

        private HttpClient CreateClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(_urlBase)
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bot", "NDA5NDM4ODQyOTQ5NzMwMzA0.DVenQQ.LxJFN0PCvrW5f_b_enCrU1XgT6s");

            return client;
        }

        private TResult CallWebApiFunc<TResult>(string uri, IEnumerable<object> args = null)
        {
            var value = default(TResult);

            using (var client = CreateClient())
            {
                var response = client.GetAsync(uri).Result;
                if (response.IsSuccessStatusCode)
                {
                    var x = response.Content.ReadAsStringAsync().Result;
                }
            }

            return value;
        }

        public string GetUser(ulong id)
        {
            var uri = $"users/{id}";
            var user = CallWebApiFunc<string>(uri);
            return user;
        }
    }
}