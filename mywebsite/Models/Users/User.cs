using Microsoft.AspNetCore.Identity;

namespace mywebsite.Models.Users
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
