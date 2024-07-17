using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TestV2.Server.Migrations;
using TestV2.Server.Models;

namespace TestV2.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AssignmentController : ControllerBase
    {
        private ApplicationDbContext _context;

        public AssignmentController(ApplicationDbContext context)
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
        public async Task<ActionResult> Add([FromBody] CreateAssignmentDto assignmentDto)
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
            _context.Assignments.Add(assignment);

            await _context.SaveChangesAsync();
            return Ok();
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

            foundAssignment.Title = assignmentDto.Title;
            foundAssignment.Description = assignmentDto.Description;
            foundAssignment.IsComplete = assignmentDto.IsComplete;

            _context.Assignments.Update(foundAssignment);
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
