using Microsoft.AspNetCore.Identity;

namespace BlossomWebApi.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateCreated { get; set; }
        public ApplicationUser()
        {

        }
    }
}
