using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Core.Entities.ViewModels
{
    public class AssignmentViewModel
    {
        public List<Document> Documents { get; set; }
        public List<FileModel> Files { get; set; }
    }
}
