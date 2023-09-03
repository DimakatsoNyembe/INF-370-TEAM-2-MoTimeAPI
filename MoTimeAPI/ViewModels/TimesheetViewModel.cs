using System.ComponentModel.DataAnnotations;

namespace MoTimeAPI.ViewModels
{
    public class TimesheetViewModel
    {
        public int? TimesheetStatusId { get; set; }
        public int? EmployeeId { get; set; }
        public int? ProjectId { get; set; }
        public int? AdminId { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateSubmitted { get; set; }
       
    }
}
