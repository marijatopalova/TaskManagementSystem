namespace TaskManagementSystem.DTOs.V2
{
    public class ProjectDtoV2
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<TaskDtoV2> Tasks { get; set; } = [];
        public List<UserDtoV2> Users { get; set; } = [];
    }
}
