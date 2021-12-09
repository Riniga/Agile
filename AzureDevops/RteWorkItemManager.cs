using System;
using System.Collections.Generic;
using System.Data;

namespace AzureDevops
{
    public class RteWorkItemManager
    {
        private static DataTable table;
        private static Areas areas;

        public static void CreateStatistics(List<Iteration> iterations, List<RteWorkItem> requirements)
        {
            areas = new Areas();
            
            var count = 0;
            foreach (var requirement in requirements)
            {
                count++;
                AddWorkItemToAreaBucket(requirement, requirement.States[0].Key);
                for (int i = 1; i < requirement.States.Count; i++)
                {
                    MoveWorkBetweenBuckets(requirement, i);
                }
            }
            
        }

        public static void Print(List<Iteration> iterations)
        {
            foreach (var iteration in iterations)
            {
                areas.Print(iteration);
            }
        }

        private static void AddWorkItemToAreaBucket(RteWorkItem workItem, DateTime created)
        {
            var area = areas.Get(workItem.AreaPath);
            area.AddItemToBucket(workItem, created.Date, BucketType.Backlog);
        }

        internal static void MoveWorkBetweenBuckets(RteWorkItem workItem, int id)
        {
            if (id == 0) return;

            var area = areas.Get(workItem.AreaPath);
            var fromState = workItem.States[id - 1].Value;
            var toState = workItem.States[id].Value;

            var fromBucket = BucketType.Backlog;
            if (fromState == "New" || fromState == "Proposed" || fromState == "Ready") fromBucket = BucketType.Backlog;
            else if (fromState == "Active") fromBucket = BucketType.Commited;
            else fromBucket = BucketType.Done;

            var toBucket = BucketType.Backlog;
            if (toState == "New" || toState == "Proposed" || toState == "Ready") toBucket = BucketType.Backlog;
            else if (toState == "Active") toBucket = BucketType.Commited;
            else toBucket = BucketType.Done;

            if (fromBucket != toBucket)
            {
                area.RemoveItemFromBucket(workItem, workItem.States[id].Key.Date, fromBucket);
                area.AddItemToBucket(workItem, workItem.States[id].Key.Date, toBucket);
            }   
        }
    }
}
