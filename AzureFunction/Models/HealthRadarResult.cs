using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AzureFunction.Models
{
    public class HealthRadarResult
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        [JsonProperty(PropertyName = "role")] 
        public string Role {get; set;}
        [JsonProperty(PropertyName = "area")] 
        public string Area {get; set;}
        [JsonProperty(PropertyName = "answers")]
        public List<int> Answers { get; set;}
        [JsonProperty(PropertyName = "posted")]
        public DateTime Posted { get; set; }
    }
}
