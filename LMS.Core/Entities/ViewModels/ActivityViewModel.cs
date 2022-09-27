using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable
namespace LMS.Core.Entities.ViewModels
{
    public class ActivityViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime StartDate { get; set; }
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime EndDate { get; set; }

        public int ModuleId { get; set; }
        [DisplayName("ActivityType")]
        public int ActivityTypeId { get; set; }
        public string ActivityName { get; set; }

        public int ForwardCourseId { get; set; }
        public ICollection<Document> Documents { get; set; } = new List<Document>();
        public List<ActivityType> ListOfActivityType { get; set; } = new List<ActivityType>();
    }
}
