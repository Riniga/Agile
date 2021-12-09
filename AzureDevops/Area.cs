using System;
using System.Collections.Generic;

namespace AzureDevops
{
    public class Area
    {
        public string AreaPath;
        public Dictionary<BucketType,Bucket> buckets;
        public Area(string areaPath)
        {
            AreaPath = areaPath;
            buckets = new Dictionary<BucketType, Bucket>
            {
                [BucketType.Backlog] = new Bucket(),
                [BucketType.Commited] = new Bucket(),
                [BucketType.Done] = new Bucket()
            };
        }

        public void AddItemToBucket(RteWorkItem workItem, DateTime date, BucketType bucket)
        {
            buckets[bucket].Add(workItem, date);
        }
        public void RemoveItemFromBucket(RteWorkItem workItem, DateTime date, BucketType bucket)
        {
            buckets[bucket].Remove(workItem, date);
        }

        public List<RteWorkItem> GetBucketAt(DateTime date, BucketType bucket)
        {
            return buckets[bucket].GetBucketAt(date);
        }
    }
}
