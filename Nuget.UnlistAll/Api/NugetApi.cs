using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using DevExpress.Internal.WinApi.Windows.UI.Notifications;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nuget.UnlistAll.Configuration;
using Shuhari.Framework.Utils;

namespace Nuget.UnlistAll.Api
{
    public class NugetApi
    {
        private string queryUrl = null;
        public NugetApi(AppConfig config)
        {
            Expect.IsNotNull(config, nameof(config));

            _config = config;


            var src = _config.Source;
            if (string.IsNullOrEmpty(src))
            {
                src = "https://api.nuget.org/v3/index.json";
            } 
            var request = WebRequest.CreateHttp(src);
            request.Method = "GET";
            using (var response = request.GetResponse())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    string json = new StreamReader(responseStream, Encoding.UTF8).ReadToEnd();
                    var result = JsonConvert.DeserializeObject<NugetResponse>(json);
                    queryUrl = result.Resources.First(c => c.Type == "SearchQueryService").Id;
                }
            }

        }

        private readonly AppConfig _config;

        public IEnumerable<NugetData> GetIndex()
        {
            int i = 0;
            while (i < 2000)
            {
                string url = $"{queryUrl}?q={System.Net.WebUtility.UrlEncode(_config.PackageId.ToLowerInvariant())}&skip={i}&take=100&prerelease=true";

                var request = WebRequest.CreateHttp(url);
                request.Method = "GET"; 
                WebResponse response = null;
                try
                {
                    response = request.GetResponse();
                }
                catch (System.Exception)
                {
                    yield break;
                }  
                using (var responseStream = response.GetResponseStream())
                {
                    string json = new StreamReader(responseStream, Encoding.UTF8).ReadToEnd();
                    var result = JsonConvert.DeserializeObject<NugetIndexResponse>(json);
                    if (result.Data.Count == 0)
                    {
                        yield break;
                    }
                    foreach (var item in result.Data)
                    {
                        yield return item;
                    }
                } 
                i += 100;
            }
        }
    }
}
