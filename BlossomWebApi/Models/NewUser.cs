using System.ComponentModel.DataAnnotations;

namespace BlossomWebApi.Models
{
    public class Student
    {
        public Student()
        {
            
        }
        [Key]
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ContactNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Address3 { get; set; }
        public string? Address4 { get; set; }
        public bool Confirmed { get; set; }

    }
}
