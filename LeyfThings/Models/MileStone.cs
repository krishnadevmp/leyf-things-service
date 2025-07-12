using System.Text.Json.Serialization;

namespace LeyfThings.Models
{
    public class MileStone
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool IsComplete { get; set; }
        public DateTime? DueDate { get; set; }

        public Guid GoalId { get; set; }
        [JsonIgnore]
        public Goal Goal { get; set; }
    }
}
