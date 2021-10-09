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
        }
        

        public void Run()
        {
            ConnectToAzure(new Uri(configuration.GetValue<string>("CollectionUri")));
            var iterations = FeatchIterations();
            var requirements = RteWorkItems(configuration.GetValue<string>("Query"));
            
            RteWorkItemManager.CreateStatistics(iterations, requirements);
            //RteWorkItemManager.PrintRteWorkItemTable();

            //PrintRequirements(requirements);
        }

        

        private void PrintRequirements(List<RteWorkItem> requirements)
        {
            foreach (var requirement in requirements)
            {
                foreach(var state in requirement.States)
                    Console.WriteLine("{0}|{1}|{2}|{3}|{4}|{5}", requirement.WorkItem.Id, requirement.WorkItem.Fields["System.Title"], requirement.AreaPath.Replace("Skanska Sverige IT\\", ""), requirement.IterationPath.Replace("Skanska Sverige IT\\",""), state.Value, state.Key.ToShortDateString());
            }
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
            var iterations = Cache<List<Iteration>>.Get(key: "iterations");
            if (iterations != null) return iterations;

            iterations = new List<Iteration>();
            iterations.Add(new Iteration(@"Skanska Sverige IT\Leveransperiod 2021-1", DateTime.Parse("2021-01-18"), DateTime.Parse("2021-04-04")));
            iterations.Add(new Iteration(@"Skanska Sverige IT\Leveransperiod 2021-2", DateTime.Parse("2021-04-05"), DateTime.Parse("2021-04-20")));
            iterations.Add(new Iteration(@"Skanska Sverige IT\Leveransperiod 2021-3", DateTime.Parse("2021-06-21"), DateTime.Parse("2021-10-03")));
            iterations.Add(new Iteration(@"Skanska Sverige IT\Leveransperiod 2021-4", DateTime.Parse("2021-10-04"), DateTime.Parse("2022-01-23")));

            Cache<List<Iteration>>.Add("iterations", iterations);
            return iterations;
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
