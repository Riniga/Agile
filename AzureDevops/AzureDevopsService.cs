using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Client;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AzureDevops
{
    internal class AzureDevopsService : IAzureDevopsService
    {
        private WorkItemTrackingHttpClient witClient;
        private readonly ILogger<AzureDevopsService> logger;
        private readonly IConfiguration configuration;

        public AzureDevopsService(ILogger<AzureDevopsService> log, IConfiguration config)
        {
            logger = log;
            configuration = config;
            Settings.CacheEnabled = configuration.GetValue<string>("CacheEnabled").ToLower() == "true";
        }

    public void Run()
        {
            ConnectToAzure(new Uri(configuration.GetValue<string>("CollectionUri")));
            var iterations = FeatchIterations();
            var requirements = RteWorkItems(configuration.GetValue<string>("Query"));
            RteWorkItemManager.CreateStatistics(iterations, requirements);
            RteWorkItemManager.Print(iterations);
        }

        private void ConnectToAzure(Uri collectionUri)
        {
            
            var credentials = new VssBasicCredential(string.Empty, configuration.GetValue<string>("PersonalAccessToken"));
            VssConnection connection = new VssConnection(collectionUri, credentials);
            witClient = connection.GetClient<WorkItemTrackingHttpClient>();
            logger.LogInformation("Connected to Azure Devops");
        }

        private List<Iteration> FeatchIterations()
        {
            var result = new List<Iteration>();
            var interationsSection = configuration.GetSection("Iterations");
            var iterations = interationsSection.GetChildren();
            foreach (IConfigurationSection iteration in iterations)
            {
                result.Add(new Iteration(
                    iteration.GetSection("IterationPath").Get<string>(),
                    DateTime.Parse(iteration.GetSection("Start").Get<string>()),
                    DateTime.Parse(iteration.GetSection("End").Get<string>())));
            }
            return result;
        }

        private List<RteWorkItem> RteWorkItems(string query)
        {
            var workItems = Cache<List<RteWorkItem>>.Get(key: query);
            if (workItems != null) return workItems;
            workItems = GetRequirements(configuration.GetValue<string>("Project") , query);
            Cache<List<RteWorkItem>>.Add(query, workItems);
            return workItems;
        }


        private List<RteWorkItem> GetRequirements(string project, string query)
        {
            List<RteWorkItem> result = new List<RteWorkItem>();
            var currentQuery = FindQuery(project, query);
            WorkItemQueryResult queryResult = witClient.QueryByIdAsync(currentQuery.Id).Result;

            if (queryResult.WorkItems.Any())
            {
                int skip = 0;
                const int batchSize = 100;
                IEnumerable<WorkItemReference> workItemRefs;
                do
                {
                    workItemRefs = queryResult.WorkItems.Skip(skip).Take(batchSize);
                    if (workItemRefs.Any())
                    {
                        var batch = witClient.GetWorkItemsAsync(workItemRefs.Select(wir => wir.Id)).Result;
                        foreach (var workItem in batch)
                        {
                            var states = GetWorkItemStatuses((int)workItem.Id);
                            result.Add(new RteWorkItem(workItem, states));
                        }
                    }
                    skip += batchSize;
                }
                while (workItemRefs.Count() == batchSize);
            }
            return result;
        }
        private QueryHierarchyItem FindQuery(string project, string query)
        {
            var queryHierarchyItems = witClient.GetQueriesAsync(project, depth: 2).Result;
            var myQueriesFolder = queryHierarchyItems.FirstOrDefault(qhi => qhi.Name.Equals("My Queries"));
            return myQueriesFolder.Children.FirstOrDefault(qhi => qhi.Name.Equals(query));
        }

        private List<KeyValuePair<DateTime, string>> GetWorkItemStatuses(int workItemId)
        {
            var result = new List<KeyValuePair<DateTime, string>>();
            var updates = witClient.GetUpdatesAsync(workItemId).Result;
            foreach (var workItemUpdate in updates)
            {

                if (workItemUpdate.Fields == null || !workItemUpdate.Fields.ContainsKey("System.State")) continue;
                var stateField = workItemUpdate.Fields["System.State"];
                var changedField = workItemUpdate.Fields["System.ChangedDate"];

                if (stateField.OldValue != stateField.NewValue)
                    result.Add(new KeyValuePair<DateTime, string>((DateTime)changedField.NewValue, (string)stateField.NewValue));
            }
            return result;
        }
    }
}
