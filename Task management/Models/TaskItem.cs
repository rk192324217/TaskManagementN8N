namespace MyNotesApi.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AssignedTo { get; set; } = string.Empty; // e.g., "User2"
        public bool IsCompleted { get; set; } = false;
    }
}
