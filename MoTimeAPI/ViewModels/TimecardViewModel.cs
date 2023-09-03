using System.ComponentModel.DataAnnotations;

namespace MoTimeAPI.ViewModels
{
    public class TimecardViewModel
    {
        public int? EmployeeId { get; set; }
        public int? ProjectId { get; set; }
        public int? TimesheetId { get; set; }
        public int? AdminId { get; set; }
        public string? Title { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan StartTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan EndTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime TimecardDate { get; set; }

        public string? Comment { get; set; }
       
    }
}
