using System;
using System.Collections.Generic;

namespace AzureDevops
{
    public class Iteration
    {
        public string IterationPath;
        public DateTime Start;
        public DateTime End;

        public Iteration() { }
        public Iteration(string iterationPath, DateTime start, DateTime end)
        {
            IterationPath = iterationPath;
            Start = start;
            End=end;
        }

        public bool Contains(string path)
        {
            var pathParts = path.Split('\\');
            if (pathParts.Length == 1) return false;
            if (path.Split('\\')[1]== IterationPath.Split('\\')[1]) return true;
            return false;
        }
    }
}
