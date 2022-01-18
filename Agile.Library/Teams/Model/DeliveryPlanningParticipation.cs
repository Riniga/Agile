using Agile.Library.Teams.Enum;
using System.Text.Json.Serialization;

namespace Agile.Library.Teams.Model
{
    public class DeliveryPlanningParticipation
    {
        public int Id { get; set; }
        public DeliveryPlanning Event { get; set; }
        public Employee Employee { get; set; }
        [JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public DeliveryPlanningParticipationScale Scale { get; set; }
    }
}
