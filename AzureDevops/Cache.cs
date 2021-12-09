using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AzureDevops
{
    public class Cache<T> where T : class
    {
        internal static T Get(string key)
        {
            if (!Settings.CacheEnabled) return null;
            var path = @"cache/" + key + ".json";

            if (!File.Exists(path)) return null;
            var json = File.ReadAllText(path);

            var deserialized = (JArray)JsonConvert.DeserializeObject(json);
            return deserialized.ToObject<T>(); ;
        }

        internal static void Add(string key,T requirements)
        {
            var path = @"cache/" + key + ".json";
            var json = JsonConvert.SerializeObject(requirements, Formatting.Indented);
            (new FileInfo(path)).Directory.Create();
            File.WriteAllText(path, json);
        }
    }
}