using System.ComponentModel.DataAnnotations;

namespace BlossomWebApi.Models
{
    public class LearningOutcome
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<LearningOutcomeSubItem> LearningOutcomeSubItem { get; set; } = new ();

    }
}
