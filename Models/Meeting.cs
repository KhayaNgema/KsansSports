using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyField.Models
{
    public class Meeting
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MeetingId { get; set; } 

        public string MeetingTitle { get; set;}

        public string MeetingDescription { get; set;}

        public string Venue { get; set;}

        public DateTime MeetingDate { get; set;}
        
        public DateTime MeetingTime { get; set;}  

        public string AdditionalComments { get; set;}

        public DateTime CreatedDateTime { get; set; }

        public DateTime ModifiedDateTime { get; set; }

        public string CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public virtual SportsMember CreatedBy { get; set; }

        public string ModifiedById { get; set; }

        [ForeignKey("ModifiedById")]
        public virtual SportsMember ModifiedBy { get; set; }
    }
}
