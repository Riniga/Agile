using Agile.Library.Teams.Model;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Agile.Library
{
    public class CosmosCache<T> where T : class
    {
        public static async Task<T> Get(string key)
        {
            var container = await GetContainer();
            try
            {
                var readResponse = await container.ReadItemAsync<CacheItem>(id: key, partitionKey: new PartitionKey(key));
                var item = readResponse.Resource;
                if (item.Expirers < DateTime.Now) return null;
                else
                {
                    return JsonConvert.DeserializeObject<T>(item.cachedObject.ToString());
                }
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.Message);
                return null; 
            }
        }

        public static async Task Set(string key, T requirements, int cacheTime)
        {
            CacheItem teams = new CacheItem
            {
                key = key,
                cachedObject = requirements,
                Expirers = DateTime.Now.AddMinutes(cacheTime)
            };
            var container = await GetContainer();
            if (await Exists(key)) await container.ReplaceItemAsync<CacheItem>(teams, teams.key, new PartitionKey(key)); 
            else await container.CreateItemAsync<CacheItem>(teams, new PartitionKey(teams.key));
        }

        private static async Task<Container> GetContainer()
        {
            var endpointUrl = Environment.GetEnvironmentVariable("EndpointUrl");
            var primaryKey = Environment.GetEnvironmentVariable("PrimaryKey");
            var databaseId = Environment.GetEnvironmentVariable("DatabaseId");
            var cacheContainerId = "Cache";
            var partitionKeyPath = "/key";

            var cosmosClient = new CosmosClient(endpointUrl, primaryKey);
            Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            Container container = await database.CreateContainerIfNotExistsAsync(cacheContainerId, partitionKeyPath);
            return container;
        }

        private static async Task<bool> Exists(string key)
        {
            var container = await GetContainer();
            try
            {
                ItemResponse<CacheItem> readResponse = await container.ReadItemAsync<CacheItem>(id: key, partitionKey: new PartitionKey(key));
                var item = readResponse.Resource;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

    }
}
