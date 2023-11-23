using Microsoft.AspNetCore.Identity;

namespace AfterTaste.Data
{
    public class User : IdentityUser
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Address { get; set; }

    }
}
