using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PurdueUniversity.Models
{
    public class Appointment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppointmentID { get; set; }
        public int? StudentID { get; set; }
        public int? CounselorID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        public string Time { get; set; }

        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        public bool Alloted { get; set; }

        public virtual Student Student { get; set; }
        public virtual Counselor Counselor { get; set; }
    }
}