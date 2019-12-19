using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi01.Meta;
using webapi01.Models;

namespace webapi01.Controllers
{
    [Route("api/[controller]")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ContosoUniversityContext _context;

        public CourseController(ContosoUniversityContext context)
        {
            _context = context;
        }

        // GET: api/Course
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourse()
        {
            return await _context.Course
                .Where(e=>e.IsDeleted==0)
                .ToListAsync();
        }

        // GET: api/Course/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Course
                .Where(e=>e.DepartmentId==id & e.IsDeleted==0)
                .FirstOrDefaultAsync();

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // PUT: api/Course/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            if (id != course.CourseId)
            {
                return BadRequest();
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPatch("patch/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PatchCourse(int id, PatchCourse course)
        {
            var findcouse=await this._context.Course.FindAsync(id);

            findcouse.Credits = course.Credits;
            findcouse.Title = course.Title;

            if (!TryValidateModel(findcouse))
            {
                return BadRequest();
            }

            //_context.Entry(findcouse).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // POST: api/Course
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            _context.Course.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCourse), new { id = course.CourseId }, course);

            //return CreatedAtAction("GetCourse", new { id = course.CourseId }, course);
        }

        // DELETE: api/Course/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Course>> DeleteCourse(int id)
        {
            var course = await _context.Course
                .Where(e => e.IsDeleted == 0 && e.CourseId == id)
                .FirstOrDefaultAsync();

            //var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            
            _context.Course.Remove(course);
            await _context.SaveChangesAsync();

            return course;
        }

        


        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.CourseId == id && e.IsDeleted==0);
        }
        
        [HttpGet("VwCourseStudentCount")]
        public async Task<IActionResult> GetVwCourseStudentCount()
        {
            return Ok(await _context.VwCourseStudentCount.ToListAsync());
        }

        [HttpGet("VwCourseStudents")]
        public async Task<IActionResult> GetVwCourseStudent()
        {
            return Ok(await _context.VwCourseStudents.ToListAsync());
        }
    }
}
