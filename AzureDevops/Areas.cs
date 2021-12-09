using System;
using System.Collections.Generic;
using System.Linq;

namespace AzureDevops
{
    public class Areas
    {
        private static List<Area> areas;
        
        public Areas()
        {
            areas = new List<Area>();
        }

        private bool Contains(string areaToFind)
        {
            foreach (var area in areas)
            {
                if (area.AreaPath == areaToFind) return true;
            }
            return false;
        }

        internal Area Get(string areaPath)
        {
            var mainAreaPath = GetMainAreaPath(areaPath);
            if (!Contains(mainAreaPath)) areas.Add(new Area(mainAreaPath));
            return areas.Where(area => area.AreaPath== mainAreaPath).FirstOrDefault();
        }

        public void Print(Iteration iteration)
        {
            foreach (var area in areas)
            {
                var backlogItems = area.GetBucketAt(iteration.Start, BucketType.Backlog);
                var commitedItems = area.GetBucketAt(iteration.End, BucketType.Commited);
                var doneWorkItems = area.GetBucketAt(iteration.End, BucketType.Done).Except(area.GetBucketAt(iteration.Start, BucketType.Done)).ToList();
                var extraWorkItems = new List<RteWorkItem>();


                int extra = 0;
                foreach (var item in commitedItems)
                {
                    string type = (string)item.WorkItem.Fields["Microsoft.VSTS.CMMI.RequirementType"];
                    if (type == "Planerat (extra)") extraWorkItems.Add(item);
                }
                foreach (var item in doneWorkItems)
                {
                    string type = (string)item.WorkItem.Fields["Microsoft.VSTS.CMMI.RequirementType"];
                    if (type == "Planerat (extra)") extraWorkItems.Add(item);
                }

                Console.WriteLine("{0}|{1}|{2}|{3}|{4}|{5}", area.AreaPath, iteration.IterationPath.Replace("Skanska Sverige IT\\", ""), backlogItems.Count, commitedItems.Count,extraWorkItems.Count, doneWorkItems.Count);
            }
        }

        public static string GetMainAreaPath(string areaPath)
        {
            var pathParts = areaPath.Split('\\');
            if (pathParts.Length == 1) return pathParts[0];
            return pathParts[1];
        }
    }
}
