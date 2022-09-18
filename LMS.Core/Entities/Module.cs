using System.ComponentModel.DataAnnotations;

namespace LMS.Core.Entities
{
#nullable disable // kan vi ha det så?
    public class Module
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime StartDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime EndDate { get; set; }

        public int CourseId { get; set; }

        //nav prop
        public Course Course { get; set; }
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();   
        public ICollection<Document> Documents { get; set; } = new List<Document>();    
    }
}