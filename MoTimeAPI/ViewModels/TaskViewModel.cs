namespace MoTimeAPI.ViewModels
{
    public class TaskViewModel
    {
        public int? ProjectId { get; set; }

        public int? EmployeeId { get; set; }

        public int? TaskStatusId { get; set; }

        public int? PriorityId { get; set; }

        public int? TaskTypeId { get; set; }

        public string TaskName { get; set; }

        public string TaskDescription { get; set; }

        public DateTime? DueDate { get; set; }

        public bool? IsComplete { get; set; }
    }
}
