using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using AzureFunction.Models;

namespace AzureFunction.Business
{
    class CosmosHelper
    {
        private static string EndpointUrl = "https://nissesgagnercosmosdb.documents.azure.com:443/";
        private static string PrimaryKey = "Av8F3gUR3WAL4SXyOxjqdxo9IDHQ25pkcVmUA4ZHFS0ZsqtNhJFiYfzXZkNmwHdVvNI73KFtIawlP7PmjS5E8g==";
        private static string databaseId = "NissesGagnerDatabase";
        private static string containerId = "DevopsHealthRadar";
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
            result.Id = Guid.NewGuid();
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
            _cosmosClient = new CosmosClient(EndpointUrl, PrimaryKey);
            Database database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            Container container = await database.CreateContainerIfNotExistsAsync(containerId, partitionKeyPath);
            return container;
        }
    }
}
