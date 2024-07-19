using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CalendarTodoList.Server.Models
{
    public partial class Assignment
    {
        public AssignmentDto AsDto()
        {
            return new AssignmentDto(
                Id,
                Date,
                Title,
                Description,
                IsComplete
            );
        }
    }

    public record AssignmentDto(
        Guid Id,
        DateOnly Date,
        string Title,
        string Description,
        bool IsComplete
    );

    public record CreateAssignmentDto(
        [Required]DateOnly Date,
        [Required][StringLength(50)]string Title,
        [StringLength(100)] string Description,
        [DefaultValue(false)] bool IsComplete
    );

    public record UpdateAssignmentDto(
        [Required] DateOnly Date,
        [Required][StringLength(50)] string Title,
        [StringLength(100)] string Description,
        [DefaultValue(false)] bool IsComplete
    );

    public record UpdateComplateAssignmentDto(
        [Required] bool IsComplete
    );
}
