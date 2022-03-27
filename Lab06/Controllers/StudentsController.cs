using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab06.Data;
using Lab06.Models;

namespace Lab06.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly StudentDbContext _context;

        public StudentsController(StudentDbContext context)
        {
            _context = context;
        }

        // GET: api/Students1
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)] // returned when we return list of Students successfully 
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // returned when there is an error in processing the request 
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }

        // GET: api/Students1/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)] // returned when we return student successfully -- returning the student with requested id
        [ProducesResponseType(StatusCodes.Status404NotFound)] // returned when there is an error in (error in the code or error from the server side)processing the request i.e. STUDENT NOT FOUND
        public async Task<ActionResult<Student>> GetStudent(Guid id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound(); //error 404 Not found
            }

            return student;
        }

        // PUT: api/Students1/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)] //returned when the student is updated successfully
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //returned when there is an error in the request (id not found)
        [ProducesResponseType(StatusCodes.Status404NotFound)] //returned when the server is not able to update the student in the database
        public async Task<IActionResult> PutStudent(Guid id, Student student)
        {
            var student01 = await _context.Students.FindAsync(id);
            if (student01 == null)
            {
                return BadRequest("Student ID Not Found");
            }
            if (student01 != null)
            {
                student01.FirstName = student.FirstName;
                student01.LastName = student.LastName;
                student01.Program = student.Program;
                _context.SaveChanges();
            }
            else
            {
                return NotFound("Not found");
            }
            return Ok("Student updated Successfully\n" + student);
        }

        // POST: api/Students1
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)] // returned when the POST is executed successfully
        [ProducesResponseType(StatusCodes.Status409Conflict)] // returned when there is an error in processing the request (id already exists)
        [ProducesResponseType(StatusCodes.Status201Created)] // returned the request is posted successfully
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            _context.Students.Add(student);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StudentExists(student.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }

        // DELETE: api/Students1/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //returned when the server is unable to get the id
        [ProducesResponseType(StatusCodes.Status204NoContent)] //returned when the student is deleted and there is no content to delete
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(Guid id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
