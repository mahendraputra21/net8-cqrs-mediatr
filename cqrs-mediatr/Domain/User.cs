using Microsoft.AspNetCore.Identity;

namespace cqrs_mediatr.Domain
{
    public class User : IdentityUser
    {
        public string? Initials { get; set; }
    }
}
