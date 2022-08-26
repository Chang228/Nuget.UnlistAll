using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows.Documents;

namespace Nuget.UnlistAll.Api
{ 
    public class NugetResponse
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("resources")]
        public List<NugetResource> Resources { get; set; }
    }
    public class NugetResource
    {
        [JsonProperty("@id")]
        public string Id { get; set; }
        [JsonProperty("@type")]
        public string Type { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class NugetData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("versions")]
        public List<NugetVersion> Versions { get; set; }

        [JsonProperty("authors")]
        public List<string> Authors { get; set; }

        [JsonProperty("iconUrl")]
        public string IconUrl { get; set; }

        [JsonProperty("licenseUrl")]
        public string LicenseUrl { get; set; }

        [JsonProperty("projectUrl")]
        public string ProjectUrl { get; set; }

        [JsonProperty("registration")]
        public string Registration { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("totalDownloads")]
        public int TotalDownloads { get; set; }

        [JsonProperty("verified")]
        public bool Verified { get; set; }
    }

    public class NugetIndexResponse
    {
        [JsonProperty("totalHits")]
        public int TotalHits { get; set; }

        [JsonProperty("data")]
        public List<NugetData> Data { get; set; }
    }

    public class NugetVersion
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("downloads")]
        public int Downloads { get; set; }

        [JsonProperty("@id")]
        public string Id { get; set; }
    }



}
