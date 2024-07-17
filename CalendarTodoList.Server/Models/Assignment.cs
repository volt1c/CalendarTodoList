using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestV2.Server.Models
{
    public class Assignment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; }

        public DateOnly Date { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = string.Empty;

        public bool IsComplete { get; set; } = false;

        public User User { get; set; } = null!;
    }
}
