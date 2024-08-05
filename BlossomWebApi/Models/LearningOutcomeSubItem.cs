using System.ComponentModel.DataAnnotations;

namespace BlossomWebApi.Models
{
    public class LearningOutcomeSubItem
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }    
        public List<string> Steps { get; set; }    = new List<string>();


    }
}
