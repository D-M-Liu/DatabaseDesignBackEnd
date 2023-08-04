using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StaffsController : ControllerBase
    {
        private readonly ModelContext _context;

        public StaffsController(ModelContext context)
        {
            _context = context;
        }

        // GET: api/Staffs
        [HttpGet]
        public ActionResult<IEnumerable<Staff>> GetStaff()
        {
            if (_context.Staff == null)
            {
                return NotFound();
            }

            return _context.Staff.ToList();
        }

        // GET: api/Staffs/5
        [HttpGet("{id}")]
        public ActionResult<Staff> GetStaff(string id)
        {
            if (_context.Staff == null)
            {
                return NotFound();
            }

            var staff = _context.Staff.Find(id);

            if (staff == null)
            {
                return NotFound();
            }

            return staff;
        }

        // PUT: api/Staffs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult PutStaff(string id, Staff staff)
        {
            if (id != staff.EmployeeId)
            {
                return BadRequest();
            }

            _context.Entry(staff).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StaffExists(id))
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

        // POST: api/Staffs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Staff> PostStaff(Staff staff)
        {
            if (_context.Staff == null)
            {
                return Problem("Entity set 'ModelContext.Staff' is null.");
            }

            _context.Staff.Add(staff);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (StaffExists(staff.EmployeeId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStaff", new { id = staff.EmployeeId }, staff);
        }

        // DELETE: api/Staffs/5
        [HttpDelete("{id}")]
        public IActionResult DeleteStaff(string id)
        {
            if (_context.Staff == null)
            {
                return NotFound();
            }

            var staff = _context.Staff.Find(id);
            if (staff == null)
            {
                return NotFound();
            }

            _context.Staff.Remove(staff);
            _context.SaveChanges();

            return NoContent();
        }

        private bool StaffExists(string id)
        {
            return _context.Staff?.Any(e => e.EmployeeId == id) ?? false;
        }
    }
}
