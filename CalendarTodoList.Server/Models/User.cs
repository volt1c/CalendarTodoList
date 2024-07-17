using Microsoft.AspNetCore.Identity;

namespace TestV2.Server.Models
{
    public class User : IdentityUser
    {
        public virtual ICollection<Assignment> Assignments { get; set; } = null!;
    }
}
