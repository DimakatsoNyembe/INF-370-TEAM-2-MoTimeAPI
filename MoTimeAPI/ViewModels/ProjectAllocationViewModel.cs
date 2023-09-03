namespace MoTimeAPI.ViewModels
{
    public class ProjectAllocationViewModel
    {

        public int ProjectId { get; set; }

        public int EmployeeId { get; set; }

        public int ClaimItemId { get; set; }

        public bool IsEligibleToClaim { get; set; }

        public decimal ClaimableAmount { get; set; }

        public int TotalNumHours { get; set; }

        public int BillableOverTime { get; set; }

        public bool IsOperationalManager { get; set; }

        public bool IsProjectManager { get; set; }
    }

}
