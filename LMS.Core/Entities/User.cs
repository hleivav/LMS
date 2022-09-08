using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Core.Entities
{
#nullable disable
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";
       

        //foreign key
        public int? CourseId { get; set; }

        //Navigation prop
        public Course Course { get; set; }
        public ICollection<Document> Documents { get; set; } = new List<Document>();    
    }
}
