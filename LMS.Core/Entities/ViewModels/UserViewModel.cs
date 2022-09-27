using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Core.Entities.ViewModels
{
    public class UserViewModel
    {
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Password { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        [Required]
        public string UserName { get; set; }
        public string Email { get; set; }



        //foreign key
        public int? CourseId { get; set; }
    }
}
