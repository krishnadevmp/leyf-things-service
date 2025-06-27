namespace LeyfThings.DTOs
{
    public class GoalDTO
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public DateTime? TargetDate { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public List<SubGoalDTO> SubGoals { get; set; } = new();
    }
}
