using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Agile.Api.Models;

namespace Agile.Api.Business
{
    class CosmosHelper
    {
        private static string partitionKeyPath = "/id";
        private static CosmosClient _cosmosClient;

        internal static async Task<List<HealthRadarResult>> GetAllResultsFromCosmosDb()
        {
            var queryDefinition = new QueryDefinition("SELECT * FROM results");
            var container = await GetContainer();
            var queryResultSetIterator = container.GetItemQueryIterator<HealthRadarResult>(queryDefinition);
            var results = new List<HealthRadarResult>();
            while (queryResultSetIterator.HasMoreResults)
            {
                var currentResultSet = await queryResultSetIterator.ReadNextAsync();
                results.AddRange(currentResultSet);
            }
            return results;
        }

        internal static async Task CreateResultInCosmosDb(HealthRadarResult result)
        {
            if (result.Id==Guid.Empty) result.Id = Guid.NewGuid();
            var container = await GetContainer();
            await container.CreateItemAsync<HealthRadarResult>(result, new PartitionKey(result.Id.ToString()));
        }
        
        internal static async Task UpdateResultInCosmosDb(HealthRadarResult result)
        {
            var container = await GetContainer();
            await container.ReplaceItemAsync<HealthRadarResult>(result, result.Id.ToString(), new PartitionKey(result.Id.ToString()));
        }
        
        internal static async Task DeleteResultFromCosmosDb(string id, string partition)
        {
            var container = await GetContainer();
            await container.DeleteItemAsync<HealthRadarResult>(id, new PartitionKey(partition));
        }
        private static async Task<Container> GetContainer()
        {
            string endpointUrl = Environment.GetEnvironmentVariable("EndpointUrl");  //ConfigurationManager.AppSettings["EndpointUrl"];
            string primaryKey = Environment.GetEnvironmentVariable("PrimaryKey");  //ConfigurationManager.AppSettings["PrimaryKey"];
            string databaseId = Environment.GetEnvironmentVariable("DatabaseId");  //ConfigurationManager.AppSettings["DatabaseId"];
            string containerId = Environment.GetEnvironmentVariable("ContainerId");  //ConfigurationManager.AppSettings["ContainerId"];
            _cosmosClient = new CosmosClient(endpointUrl, primaryKey);
            Database database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            Container container = await database.CreateContainerIfNotExistsAsync(containerId, partitionKeyPath);
            return container;
        }
    }
}
