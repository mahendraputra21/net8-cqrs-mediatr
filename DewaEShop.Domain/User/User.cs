using Microsoft.AspNetCore.Identity;

namespace DewaEShop.Domain.User
{
    public class User : IdentityUser
    {
        public string? Initials { get; set; }
    }
}
