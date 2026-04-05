namespace LeyfThings.Models
{
    public class ErrorResponse
    {
        public string Type { get; set; } = string.Empty;
        public int Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public string TraceId { get; set; } = string.Empty;
        public IDictionary<string, string[]>? Errors { get; set; }
        public string? StackTrace { get; set; }
    }
}
