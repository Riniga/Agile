using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Agile.Library.Teams.Model
{
    public class DeliveryPlanning
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime TimeStamp{ get; set; }
        
    }
}
