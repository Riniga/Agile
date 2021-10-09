using System;
using System.Collections.Generic;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace AzureDevops
{
    public class RteWorkItem
    {
        public WorkItem WorkItem;
        public List<KeyValuePair<DateTime, string>> States;
        public string AreaPath;
        public string IterationPath;
        public RteWorkItem(WorkItem workItem, List<KeyValuePair<DateTime, string>> states)
        {
            WorkItem = workItem;
            States = states;
            AreaPath = (string) workItem.Fields["System.AreaPath"];
            IterationPath = (string)workItem.Fields["System.IterationPath"];
        }
    }
}
