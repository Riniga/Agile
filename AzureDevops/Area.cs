using System;
using System.Collections.Generic;

namespace AzureDevops
{
    public class Area
    {
        public string AreaPath;
        
        public Dictionary<Iteration, List<RteWorkItem>> Backlog;
        public Dictionary<Iteration, List<RteWorkItem>> Commited;
        public Dictionary<Iteration, List<RteWorkItem>> Done;


        public Area(string areaPath)
        {
            AreaPath = areaPath;
            Backlog = new Dictionary<Iteration, List<RteWorkItem>>();
            Commited = new Dictionary<Iteration, List<RteWorkItem>>();
            Done = new Dictionary<Iteration, List<RteWorkItem>>();
        }

        public void AddItemToBucket(RteWorkItem item, Iteration iteration, Bucket bucket)
        {
            if (bucket == Bucket.Backlog)
            {
                if (!Backlog.ContainsKey(iteration)) Backlog.Add(iteration, new List<RteWorkItem>());
                Backlog[iteration].Add(item);
            }
            if (bucket == Bucket.Commited)
            {
                if (!Commited.ContainsKey(iteration)) Commited.Add(iteration, new List<RteWorkItem>());
                Commited[iteration].Add(item);
            }
            if (bucket == Bucket.Done)
            {
                if (!Done.ContainsKey(iteration)) Done.Add(iteration, new List<RteWorkItem>());
                Done[iteration].Add(item);
            }
        }

        internal string GetMainAreaPath()
        {
            var pathParts = AreaPath.Split('\\');
            if (pathParts.Length == 1) return pathParts[0];
            return pathParts[1];
        }
    }
}
