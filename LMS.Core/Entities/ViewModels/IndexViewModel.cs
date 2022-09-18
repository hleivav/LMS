using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Core.Entities.ViewModels
{
    public class IndexViewModel

    {
        public List<Course> ListOfCourses { get; set; }
        public List<User> ListOfTeachers { get; set; }
        public List<User> ListOfStudents { get; set; }
        public List<Module> ListOfModules { get; set; }
        public List<Activity> ListOfActivity { get; set; }
        public List<ActivityType> ListOfActivityType { get; set; }

    }
}
