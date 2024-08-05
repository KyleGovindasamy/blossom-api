using System.ComponentModel.DataAnnotations;

namespace BlossomWebApi.Models
{
    public class Programme
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<LearningOutcome> LearningOutcomes { get; set; } = new();
        public List<LearningOutcomeSubItem> PrerequisiteSkills { get; set; } = new();
    }
}
