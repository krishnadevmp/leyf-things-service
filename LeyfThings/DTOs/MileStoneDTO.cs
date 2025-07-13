namespace LeyfThings.DTOs
{
    public class MileStoneDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid GoalId { get; set; }
    }
}
