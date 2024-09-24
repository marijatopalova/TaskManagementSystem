namespace TaskManagementSystem.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        // Navigation properties
        public virtual ICollection<TaskItem>? Tasks { get; set; }
        public virtual ICollection<Project>? Projects { get; set; }
    }
}
