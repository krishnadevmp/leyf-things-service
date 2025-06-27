namespace LeyfThings.Models
{
    public class Goal
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public DateTime? TargetDate { get; set; }
        public string Priority { get; set; } = "Medium";
        public string Status { get; set; } = "Not Started";
        public int Progress { get; set; } = 0;
        public List<MileStone> MileStones { get; set; } = new();
    }
}
