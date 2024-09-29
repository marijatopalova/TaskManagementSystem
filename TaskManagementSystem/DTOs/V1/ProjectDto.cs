namespace TaskManagementSystem.DTOs.V1
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<TaskDto> Tasks { get; set; } = [];
        public List<UserDto> Users { get; set; } = [];
    }
}
