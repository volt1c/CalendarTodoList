using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using CalendarTodoList.Server.Migrations;
using CalendarTodoList.Server.Models;

namespace CalendarTodoList.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AssignmentsController : ControllerBase
    {
        private ApplicationDbContext _context;

        public AssignmentsController(ApplicationDbContext context)
        {
            _context = context;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssignmentDto>>> GetAll()
        {
            var user = _context.Users.Where(e => e.Email == User.Identity!.Name).First();

            var assigments = await _context.Assignments
                .Where(e => e.User.Id == user.Id)
                .Select(e => e.AsDto()).ToArrayAsync();
            return Ok(assigments);
        }

        [HttpGet("today")]
        public async Task<ActionResult<IEnumerable<AssignmentDto>>> GetToday(DateOnly date)
        {
            var user = _context.Users.Where(e => e.Email == User.Identity!.Name).First();
            var today = DateOnly.FromDateTime(DateTime.Now);

            var assigments = await _context.Assignments
                .Where(e => e.User.Id == user.Id && e.Date == today)
                .Select(e => e.AsDto()).ToArrayAsync();
            return Ok(assigments);
        }

        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<AssignmentDto>>> GetPending(DateOnly date)
        {
            var user = _context.Users.Where(e => e.Email == User.Identity!.Name).First();
            var today = DateOnly.FromDateTime(DateTime.Now);

            var assigments = await _context.Assignments
                .Where(e => e.User.Id == user.Id && !e.IsComplete && e.Date < today)
                .Select(e => e.AsDto()).ToArrayAsync();
            return Ok(assigments);
        }

        [HttpGet("{year}/{month}")]
        public async Task<ActionResult<IEnumerable<AssignmentDto>>> GetByDate(int year, int month)
        {
            var user = _context.Users.Where(e => e.Email == User.Identity!.Name).First();

            var assigments = await _context.Assignments
                .Where(e => e.User.Id == user.Id && e.Date.Year == year && e.Date.Month == month)
                .Select(e => e.AsDto()).ToArrayAsync();
            return Ok(assigments);
        }


        [HttpGet("{id:guid}")]
        public async Task<ActionResult<AssignmentDto>> GetOne(Guid id)
        {
            var assigment = await _context.Assignments.FindAsync(id);
            if (assigment == null)
            {
                return NotFound();
            }
            return Ok(assigment.AsDto());
        }

        [HttpPost]
        public async Task<ActionResult<AssignmentDto>> Add([FromBody] CreateAssignmentDto assignmentDto)
        {
            var user = _context.Users.Where(e => e.Email == User.Identity!.Name).FirstOrDefault();
            if (user == null)
            {
                return BadRequest();
            }

            Assignment assignment = new()
            {
                Title = assignmentDto.Title,
                Description = assignmentDto.Description,
                Date = assignmentDto.Date,
                IsComplete = assignmentDto.IsComplete,
                User = user,
            };

            AssignmentDto dto = _context.Assignments.Add(assignment).Entity.AsDto();

            await _context.SaveChangesAsync();

            return Ok(dto);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdateAssignmentDto assignmentDto)
        {
            var foundAssignment = await _context.Assignments.FindAsync(id);
            if (foundAssignment == null)
                return NotFound();

            var user = _context.Users.Where(e => e.Email == User.Identity!.Name).First();

            if (foundAssignment.User.Id != user.Id)
                return BadRequest();

            foundAssignment.Date = assignmentDto.Date;
            foundAssignment.Title = assignmentDto.Title;
            foundAssignment.Description = assignmentDto.Description;
            foundAssignment.IsComplete = assignmentDto.IsComplete;

            _context.Assignments.Update(foundAssignment);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPatch("{id:guid}")]
        public async Task<ActionResult> ChangeSelected(Guid id, [FromBody] UpdateComplateAssignmentDto assignmentDto)
        {
            var assignment = _context.Assignments.Find(id);
            if (assignment == null)
                return NotFound();
            assignment.IsComplete = assignmentDto.IsComplete;
            await _context.SaveChangesAsync();
            return Ok();

        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var assignment = _context.Assignments.Find(id);
            if (assignment == null)
                return NotFound();
            _context.Assignments.Remove(assignment);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
