using Microsoft.AspNetCore.Identity;

namespace ShopSphere.Identity
{
    public class ApplicationUser: IdentityUser
    {
        public string FullName { get; set; }

    }
}
