using System.ComponentModel.DataAnnotations;

namespace MoTime.ViewModel
{
    public class LeaveViewModel
    {
        public int? EmployeeId { get; set; }

        public int? LeaveTypeId { get; set; }

        [DisplayFormat(DataFormatString = "0:yyyy-MM-dd")]
        public DateTime? StartDate { get; set; }

        [DisplayFormat(DataFormatString = "0:yyyy-MM-dd")]
        public DateTime? EndDate { get; set; }
    }
}
