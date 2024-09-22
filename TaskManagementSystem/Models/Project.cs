namespace TaskManagementSystem.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Navigation properties
        public virtual ICollection<TaskItem>? Tasks { get; set; } 
        public virtual ICollection<User>? Users { get; set; }
    }
}
