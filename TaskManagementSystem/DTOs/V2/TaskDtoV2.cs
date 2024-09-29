namespace TaskManagementSystem.DTOs.V2
{
    public class TaskDtoV2
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
    }
}
