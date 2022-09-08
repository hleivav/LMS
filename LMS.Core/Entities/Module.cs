namespace LMS.Core.Entities
{
#nullable disable // kan vi ha det så?
    public class Module
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StarDate { get; set; }
        public DateTime EndDate { get; set; }

        //nav prop
        public Course Course { get; set; }
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();   
        public ICollection<Document> Documents { get; set; } = new List<Document>();    
    }
}