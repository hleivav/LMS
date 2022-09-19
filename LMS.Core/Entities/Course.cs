using System.ComponentModel.DataAnnotations;

namespace LMS.Core.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime StartDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime EndDate { get; set; }

        //nav prop

        public ICollection<User> Users { get; set; } = new List<User>();    
        public ICollection<Module> Modules { get; set; } = new List<Module>();
        public ICollection<Document> Documents { get; set; } = new List<Document>();
    }
}