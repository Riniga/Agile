using System;
using System.Collections.Generic;
using System.Linq;

namespace AzureDevops
{
    public class Bucket
    {
        public List<Change> changes;
        
        public Bucket()
        {
            changes = new List<Change>();
        }

        internal List<RteWorkItem> GetBucketAt(DateTime date)
        {
            List<RteWorkItem> result= new List<RteWorkItem>();
            changes = changes.OrderBy(item => item.date).ToList();


            foreach (var change in changes)
            {
                if (change.date > date) break;
                if (change.type == ChangeType.Add) result.Add(change.workItem);
                else result.Remove(change.workItem);
            }
            return result;
        }

        internal void Add(RteWorkItem workItem, DateTime date)
        {
            changes.Add(new Change(workItem, date, ChangeType.Add));

        }
        internal void Remove(RteWorkItem workItem, DateTime date)
        {
            changes.Add(new Change(workItem, date, ChangeType.Remove));
        }
    }

    public class Change
    {
        public RteWorkItem workItem;
        public DateTime date;
        public ChangeType type;

        public Change(RteWorkItem workItem, DateTime date, ChangeType type)
        {
            this.workItem = workItem;
            this.date = date;
            this.type = type;
        }

        public bool SortByDate(DateTime date1, DateTime date2)
        {
            return date1<date2;
        }
    }

    public enum ChangeType { Add,Remove }
}
