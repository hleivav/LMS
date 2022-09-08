namespace LMS.Core.Entities
{
#nullable disable
    public class ActivityType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();

    }
}