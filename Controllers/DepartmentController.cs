using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using webapi01.Models;
using webapi01.Meta;

namespace webapi01.Controllers
{
    [Route("api/[controller]")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly ContosoUniversityContext _context;

        public DepartmentController(ContosoUniversityContext context)
        {
            _context = context;
        }

        // GET: api/Department
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartment()
        {

            throw new Exception("test");

            return await _context.Department
                .Where(e=>e.IsDeleted==0)
                .ToListAsync();
        }

        // GET: api/Department/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            //var department = await _context.Department.FindAsync(id);
            var department = await _context.Department
                .Where(e => e.IsDeleted == 0 && e.DepartmentId == id)
                .FirstOrDefaultAsync();
            
            if (department == null)
            {
                return NotFound();
            }

            return department;
        }

        // PUT: api/Department/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PutDepartment(int id, Department department)
        {
            if (id != department.DepartmentId)
            {
                return BadRequest();
            }

            _context.Entry(department).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
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

        // POST: api/Department
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Department>> PostDepartment(Department department)
        {
            _context.Department.Add(department);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDepartment", new { id = department.DepartmentId }, department);
        }

        // DELETE: api/Department/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Department>> DeleteDepartment(int id)
        {
            //var department = await _context.Department.FindAsync(id);
            var department = await _context.Department
                .Where(e=>e.IsDeleted==0 && e.DepartmentId==id)
                .FirstOrDefaultAsync();

            if (department == null)
            {
                return NotFound();
            }

            _context.Department.Remove(department);
            await _context.SaveChangesAsync();

            return department;
        }

        /// <summary>
        /// 第四題-1
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost("sp_update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateDepartment(UpdateObj obj)
        {
            try
            {
                var dip = new SqlParameter("dip", System.Data.SqlDbType.Int);
                dip.Value = obj.DepartmentID;

                var name = new SqlParameter("name", System.Data.SqlDbType.NVarChar);
                name.Value = obj.Name;

                var budget = new SqlParameter("budget", System.Data.SqlDbType.Float);
                budget.Value = obj.Budget;

                var StartDate = new SqlParameter("startdate", System.Data.SqlDbType.VarChar);
                StartDate.Value = obj.StartDate;

                var InstructorID = new SqlParameter("InstructorID", System.Data.SqlDbType.Int);
                InstructorID.Value = obj.InstructorID;

                var RowVersion = new SqlParameter("RowVersion", System.Data.SqlDbType.Binary);
                RowVersion.Value =Encoding.UTF8.GetBytes(obj.RowVersion);

                await this._context.Department.FromSqlRaw(@"execute dbo.Department_Update
                    @dip , @name, @budget ,@startdate,@InstructorID,@RowVersion",
                    dip,name,budget, StartDate, InstructorID, RowVersion).ToListAsync();

                return NoContent();
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 第四題-2
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPut("sp_insert")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> InsertDepartment(InsertObj obj)
        {

            try
            {
                var name = new SqlParameter("name", System.Data.SqlDbType.NVarChar);
                name.Value = obj.Name;

                var budget = new SqlParameter("Budget", System.Data.SqlDbType.Float);
                budget.Value = obj.Budget;

                var startdate = new SqlParameter("startdate", System.Data.SqlDbType.VarChar);
                startdate.Value = obj.StartDate;

                var id = new SqlParameter("id", System.Data.SqlDbType.Int);
                id.Value = obj.InstructorID;

                var result=await this
                ._context
                .Department
                .FromSqlRaw(@"execute dbo.Department_Insert @name,@Budget,@startdate,@id", 
                name, 
                budget, 
                startdate, 
                id)
                .ToListAsync();

                return Ok(result);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 第四題-3
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns
        [HttpDelete("sp_delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteDepartment(DeleteObj obj)
        {
            var result =await this
                ._context
                .Database.
                ExecuteSqlInterpolatedAsync($@"execute dbo.Department_Delete 
                {obj.DepartmentID},{Encoding.UTF8.GetBytes(obj.RowVersion_Original)}");
              
            return NoContent();
        }

        /// <summary>
        /// 
        /// vwDepartmentCourseCount
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [HttpGet("get_view")]
        public async Task<IActionResult> GetvwDepartmentCourseCount()
        {         
            var result =await this._context.VwDepartmentCourseCount
                .FromSqlInterpolated($@"select DepartmentID,Name,CourseCount 
                from vwDepartmentCourseCount")
                .ToListAsync();

            return Ok(result);
        }

        private bool DepartmentExists(int id)
        {
            return _context.Department.Any(e => e.DepartmentId == id && e.IsDeleted==0);
        }
    }

}
