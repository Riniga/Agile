using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AzureDevops
{
    public class RteWorkItemManager //TODO: Make singeleton
    {
        private static DataTable table;
        private static Areas areas;


        public static void CreateStatistics(List<Iteration> iterations, List<RteWorkItem> requirements)
        {
            areas = new Areas();
            foreach (var iteration in iterations)
            {
                {
                    foreach (var requirement in requirements)
                    {
                        //RteWorkItemManager.AddWorkItemToTable(requirement, iteration);
                        AddWorkItemToAreaBucket(requirement, iteration);
                    }
                }
            }
            areas.Print();
        }

        internal static void AddWorkItemToAreaBucket(RteWorkItem workItem, Iteration iteration)
        {
            var area = areas.Get(workItem.AreaPath);

            var startState = GetState(workItem, iteration.Start);
            var endState = GetState(workItem, iteration.End);

            if (startState == "Proposed" || startState == "Ready") area.AddItemToBucket(workItem, iteration, Bucket.Backlog);
            if (startState == "Active" ) area.AddItemToBucket(workItem, iteration, Bucket.Commited); ;



            if (iteration.Contains(workItem.IterationPath))
            { 
                if (startState == "Resolved" || startState == "Closed")
                { 
                    area.AddItemToBucket(workItem, iteration, Bucket.Done);
                }

                if (endState == "Resolved" || endState == "Closed")
                {
                    area.AddItemToBucket(workItem, iteration, Bucket.Done);
                }
            }

        }

        
        internal static void AddWorkItemToTable(RteWorkItem workItem, Iteration iteration)
        {
            if (table == null) table = CreateTable();
            var state1 = GetState(workItem, iteration.Start);
            var state2 = GetState(workItem, iteration.End);
            
            var iterationPath = workItem.IterationPath;

            if (iterationPath == iteration.IterationPath) //TODO Använd iterationPath från history (start?)
            {
                AddOrInsertRequirementIntoTable(workItem, state1, state2, iteration);
            }
        }

        private static void AddOrInsertRequirementIntoTable(RteWorkItem workItem, string state1, string state2, Iteration iteration)
        {
            var area = GetMaintenanceArea(workItem);
            if (state1 == string.Empty) return; //TODO: Tänk.... kan state1 inte vara tom men state2?
                                                // Note: State2 (Closed) räknas bara om kravet fanns med i from.... lägger man till behov så räknas dom inte in!

            object[] keys = new object[] { area, iteration.IterationPath, iteration.Start, iteration.End };
            var row = table.Rows.Find(keys);

            if (row == null)
            {
                row = table.NewRow();
                row["area"] = area;
                row["iteration"] = iteration.IterationPath;
                row["from"] = iteration.Start;
                row["to"] = iteration.End;
                row["backlog"] = (state1 == "Proposed" || state1 == "Ready") ? 1 : 0;
                row["åtagna"] = (state1 == "Active" || state1 == "Resolved") ? 1 : 0;
                row["slutförda"] = (state2 == "Closed") ? 1 : 0;

                table.Rows.Add(row);
            }
            else
            {
                if (state1 == "Proposed" || state1 == "Ready") row["backlog"] = (int)row["backlog"] + 1;
                else if (state1 == "Active" || state1 == "Resolved") row["åtagna"] = (int)row["åtagna"] + 1;
                if (state2 == "Closed") row["slutförda"] = (int)row["slutförda"] + 1;
            }
        }


        public static void PrintRteWorkItemTable()
        {
            Console.WriteLine("area|iteration|from|to|backlog|åtagna|slutförda");
            foreach (DataRow row in table.Rows)
            {
                Console.WriteLine("{0}|{1}|{2}|{3}|{4}|{5}|{6}", row["area"], ((string)row["iteration"]).Replace("Skanska Sverige IT\\","") , ((DateTime)row["from"]).ToShortDateString(), ((DateTime)row["to"]).ToShortDateString(), row["backlog"], row["åtagna"], row["slutförda"]);
            }
        }


        private static string GetMaintenanceArea(RteWorkItem requirement)
        {
            var parts = requirement.AreaPath.Split("\\");
            return (parts.Length == 2) ? parts[1] : parts[0];
        }
        private static DataTable CreateTable()
        {
            DataTable table = new DataTable();
            DataColumn column = new DataColumn
            {
                DataType = System.Type.GetType("System.String"),
                ColumnName = "area",
            };
            table.Columns.Add(column);

            column = new DataColumn
            {
                DataType = System.Type.GetType("System.String"),
                ColumnName = "iteration",
            };
            table.Columns.Add(column);

            column = new DataColumn
            {
                DataType = System.Type.GetType("System.DateTime"),
                ColumnName = "from",
            };
            table.Columns.Add(column);
            column = new DataColumn
            {
                DataType = System.Type.GetType("System.DateTime"),
                ColumnName = "to",
            };
            table.Columns.Add(column);

            column = new DataColumn
            {
                DataType = System.Type.GetType("System.Int32"),
                ColumnName = "backlog",
            };
            table.Columns.Add(column);
            column = new DataColumn
            {
                DataType = System.Type.GetType("System.Int32"),
                ColumnName = "åtagna",
            };
            table.Columns.Add(column);
            column = new DataColumn
            {
                DataType = System.Type.GetType("System.Int32"),
                ColumnName = "slutförda",
            };
            table.Columns.Add(column);

            DataColumn[] primaryKeyColumns = new DataColumn[4];
            primaryKeyColumns[0] = table.Columns["area"];
            primaryKeyColumns[1] = table.Columns["iteration"];
            primaryKeyColumns[2] = table.Columns["from"];
            primaryKeyColumns[3] = table.Columns["to"];
            table.PrimaryKey = primaryKeyColumns;

            return table;
        }
        private static string GetState(RteWorkItem requirement, DateTime date)
        {
            if (date < requirement.States[0].Key) return string.Empty;
            for (int i = 1; i < requirement.States.Count; i++)
            {
                if (date < requirement.States[i].Key) return requirement.States[i - 1].Value;
            }
            return requirement.States.LastOrDefault().Value;
        }
    }
}
