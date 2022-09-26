using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Core.Entities.ViewModels
{
    public class CoursesViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = String.Empty;
        [Required]
        public string Description { get; set; } = String.Empty;
        [DataType(DataType.Date)]
        [Required]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        [Required]
        public DateTime EndDate { get; set; }
        [DisplayName("Name")]
        [Required]
        public string ModuleName { get; set; } = String.Empty;
        [DisplayName("Description")]
        [Required]
        public string ModuleDescription { get; set; } =String.Empty;
        [DisplayName("Start")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime ModuleStartDate { get; set; }
        [DisplayName("End")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime ModuleEndDate { get; set; }

        public int ForwardCourseId { get; set; }
        
        public List<Activity> ListOfActivity { get; set; }= new List<Activity>();
        public List<ActivityType> ListOfActivityType { get; set; } = new List<ActivityType>();
        public List<Module> listOfModules { get; set; } = new List<Module>();
    }
}
