using Microsoft.AspNetCore.Identity;

namespace CalendarTodoList.Server.Models
{
    public class User : IdentityUser
    {
        public virtual ICollection<Assignment> Assignments { get; set; } = null!;
    }
}
