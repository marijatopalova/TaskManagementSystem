namespace TaskManagementSystem.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Foreign keys
        public int UserId { get; set; }
        public int ProjectId { get; set; }

        // Navigation properties
        public virtual User? User { get; set; }
        public virtual Project? Project { get; set; }
    }
}
