using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace WebApiFssp.Helpers
{
    public class NetworkHelpers
    {
        private static readonly ILogger<NetworkHelpers> _logger;

        static NetworkHelpers()
        {
            _logger = new Logger<NetworkHelpers>(new LoggerFactory());
        }
        
        public static string DownLUserAgent(string url)
        {
            var tmp = "";
            var count = 0;
            while (true)
            {
                try
                {
                    var task = Task.Run(() => (new TimedWebClientUa()).DownloadString(url));
                    if (!task.Wait(TimeSpan.FromSeconds(100))) throw new TimeoutException();
                    tmp = task.Result;
                    break;
                }
                catch (WebException ex)
                {
                    if (ex.Response is HttpWebResponse r) _logger.LogInformation("Response code: " + r.StatusCode);
                    if (ex.Response is HttpWebResponse errorResponse &&
                        errorResponse.StatusCode == HttpStatusCode.Forbidden)
                    {
                        _logger.LogInformation("Error 403 or 434");
                        return tmp;
                    }

                    if (count >= 5)
                    {
                        _logger.LogInformation($"Не удалось скачать за {count} попыток " + url);
                        break;
                    }

                    _logger.LogInformation("Не удалось получить строку " + ex.Message + url);
                    count++;
                    Thread.Sleep(5000);
                }
                catch (Exception e)
                {
                    if (count >= 5)
                    {
                        _logger.LogInformation($"Не удалось скачать за {count} попыток " + url);
                        break;
                    }

                    switch (e)
                    {
                        case AggregateException a
                            when a.InnerException != null && a.InnerException.Message.Contains("(404) Not Found"):
                            _logger.LogInformation("404 Exception" + a.InnerException.Message + url);
                            goto Finish;
                        case AggregateException a
                            when a.InnerException != null && a.InnerException.Message.Contains("(403) Forbidden"):
                            _logger.LogInformation("403 Exception" + a.InnerException.Message + url);
                            goto Finish;
                        case AggregateException a when a.InnerException != null &&
                                                       a.InnerException.Message.Contains(
                                                           "The remote server returned an error: (434)"):
                            _logger.LogInformation("434 Exception" + a.InnerException.Message + url);
                            goto Finish;
                    }

                    _logger.LogInformation("Не удалось получить строку" + e + url);
                    count++;
                    Thread.Sleep(5000);
                }
            }

            Finish:
            return tmp;
        }
    }
    
    public class TimedWebClientUa : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            var wr = (HttpWebRequest) base.GetWebRequest(address);
            if (wr != null)
            {
                wr.Timeout = 600000;
                wr.UserAgent = "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:55.0) Gecko/20100101 Firefox/55.0";
                return wr;
            }

            return null;
        }
    }
}