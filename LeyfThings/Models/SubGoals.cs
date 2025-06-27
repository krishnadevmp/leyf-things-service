namespace LeyfThings.Models
{
    public class SubGoals
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool IsComplete { get; set; }
        public DateTime? DueDate { get; set; }

        public Guid GoalId { get; set; }
        public Goal Goal { get; set; }
    }
}
