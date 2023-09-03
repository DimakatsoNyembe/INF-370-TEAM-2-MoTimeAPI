using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MoTimeAPI.ViewModels
{
    public class CalendarViewModel
    {

        public string CalendarItemName { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        //public int TaskID { get; set; }
        //public int TimeCardID { get; set; }

        public string CalendarItemDescription { get; set; }
        public string Location { get; set; }



        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public DateTime? StartTime { get; set; }


        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public DateTime? EndTime { get; set; }

    }
}

