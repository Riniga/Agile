using Newtonsoft.Json;
using System;
namespace Agile.Library.Teams.Model
{
    internal class CacheItem
    {
        public string id => key;
        [JsonProperty(PropertyName = "key")]
        public string key { get; set; }
        public object cachedObject { get; set; }
        public DateTime Expirers { get; set; }
    }
}
