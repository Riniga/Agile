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
            if (!Contains(areaPath)) areas.Add(new Area(areaPath));
            return areas.Where(area => area.AreaPath==areaPath).FirstOrDefault();
        }

        public void Print()
        {
            foreach (var area in areas)
            {
                foreach (var item in area.Backlog)
                {
                    Console.WriteLine("{0}|{1}|{2}|{3}", area.GetMainAreaPath() , item.Key.IterationPath.Replace("Skanska Sverige IT\\", ""),"Backlog", item.Value.Count );
                }
                foreach (var item in area.Commited)
                {
                    Console.WriteLine("{0}|{1}|{2}|{3}" , area.GetMainAreaPath(), item.Key.IterationPath.Replace("Skanska Sverige IT\\", ""), "Commited", item.Value.Count);
                }
                foreach (var item in area.Done)
                {
                    Console.WriteLine("{0}|{1}|{2}|{3}" , area.GetMainAreaPath(), item.Key.IterationPath.Replace("Skanska Sverige IT\\", ""), "Done", item.Value.Count);
                }
            }
        }

        
    }
}
