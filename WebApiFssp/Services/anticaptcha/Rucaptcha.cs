using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using Newtonsoft.Json;

namespace WebApiFssp.Services.anticaptcha
{
    public class Rucaptcha : IAnticaptcha
    {
        private readonly string _apiKey;
        private string _urlStart = "https://rucaptcha.com/in.php";
        public Rucaptcha()
        {
            _apiKey = Startup.Configuration["API_KEY"];
        }

        public string DateFromImage(string path)
        {
            return SendRequest(path);
        }

        private string SendRequest(string img)
        {
            var postParams = new Dictionary<string, string> { { "key", _apiKey }, { "method", "base64" }, { "lang", "ru" }, {"json", "1"}, {"regsense", "1"}, {"body", img} };
            var resp = MakeRequest<FirstAnswer>("POST", _urlStart, postParams);
            if (resp.status != 1)
            {
                throw new Exception("status service anticaptcha is not 1");
            }
            return TryGetResult(resp.request);
        }

        private string TryGetResult(string req)
        {
            var url = $"https://rucaptcha.com/res.php?key={_apiKey}&action=get&id={req}&json=1";
            var tryCount = 20;
            while (true)
            {
                if (tryCount < 0)
                {
                    throw new Exception("cannot get response anticaptcha after 20 attempts");
                }

                try
                {
                    var resp = MakeRequest<FirstAnswer>("GET", url);
                    if (resp.status != 1)
                    {
                        throw new Exception("status service anticaptcha is not 1");
                    }
                    return resp.request;
                }
                catch (Exception e)
                {
                    Thread.Sleep(4000);
                }

                tryCount--;
            }
        }
        
        private static T MakeRequest<T>(string httpMethod, string route, Dictionary<string, string> postParams = null)
        {
            using (var client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(new HttpMethod(httpMethod), route);

                if (postParams != null)
                    requestMessage.Content = new FormUrlEncodedContent(postParams);


                HttpResponseMessage response = client.SendAsync(requestMessage).Result;

                string apiResponse = response.Content.ReadAsStringAsync().Result;
                try
                {
                    if (apiResponse != "")
                        return JsonConvert.DeserializeObject<T>(apiResponse);;
                    throw new Exception();
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error ocurred while calling the API. It responded with the following message: {response.StatusCode} {response.ReasonPhrase}");
                }
            }
        }
    }

     class FirstAnswer
     {
         public int status;
         public string request;
     }
}