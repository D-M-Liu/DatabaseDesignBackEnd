using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.ContentModel;
using NuGet.Protocol;
using System.Data;
using System.Xml.Linq;
using webapi.Models;

namespace webapi.Controllers.Administrator
{
    [Route("administrator/staff-info")]
    [ApiController]
    public class StaffInfoController : ControllerBase
    {
        private readonly ModelContext _context;

        public StaffInfoController(ModelContext context)
        {
            _context = context;
        }

        [HttpGet("query")]
        public ActionResult<IEnumerable<Employee>> GetPage_(int pageIndex, int pageSize, string employee_id="", string username = "", string gender = "", string phone_number = "", string salary = "", string station_id = "")
        {
            int offset = (pageIndex - 1) * pageSize;
            int limit = pageSize;
            if (offset < 0 || limit <= 0)
                return BadRequest();
            string pattern1 = "'%" + (employee_id  == String.Empty ? "" : employee_id) + "%'";
            string pattern2 = "'%" + (username == String.Empty ? "" : username) + "%'";
            string pattern3 = "'%" + (gender == String.Empty ? "" : gender) + "%'";
            string pattern4 = "'%" + (phone_number == String.Empty ? "" : phone_number) + "%'";
            string pattern5 = "'%" + (salary == String.Empty ? "" : salary.ToString()) + "%'";
            string pattern6 = "'%" + (station_id == String.Empty ? "" : station_id) + "%'";
            string where_cause = "WHERE " + "employee_id like " + pattern1 +
                " AND " + "username like " + pattern2 +
                " AND " + "gender like " + pattern3 +
                " AND " + "phone_number like " + pattern4 +
                " AND " + "salary like " + pattern5 +
                " AND " + "STATION_ID like " + pattern6 + " ";
            string sql_info = "SELECT employee_id,username,phone_number,gender,STATION_ID,STATION_NAME,salary " +
                "FROM EMPLOYEE " +
                "NATURAL JOIN EMPLOYEE_SWITCH_STATION " +
                "NATURAL JOIN SWITCH_STATION " + where_cause +
                "ORDER BY employee_id " +
                "OFFSET " + offset.ToString() + " ROWS " +
                "FETCH NEXT " + limit.ToString() + " ROWS ONLY";
            DataTable df = OracleHelper.SelectSql(sql_info);
            string sql_total = "SELECT COUNT(*) " +
                "FROM EMPLOYEE " +
                "NATURAL JOIN EMPLOYEE_SWITCH_STATION " +
                "NATURAL JOIN SWITCH_STATION " + where_cause;
            DataTable df_count = OracleHelper.SelectSql(sql_total);
            int totalNum = df_count != null ? Convert.ToInt32(df_count.Rows[0][0]) : 0;
            var obj = new
            {
                totalData = totalNum,
                data = df,
            };
            return Content(JsonConvert.SerializeObject(obj), "application/json");
        }

        [HttpPatch]
        public IActionResult PutStaff([FromBody] dynamic _param)
        {
            dynamic param = JsonConvert.DeserializeObject(Convert.ToString(_param));
            string employee_id = $"{param.employee_id}";
            var staff = _context.Employees.Find(employee_id);
            var staff_station = _context.EmployeeSwitchStations.Find(employee_id);
            if (staff == null || staff_station == null)
            {
                return NotFound();
            }
            
            staff.PhoneNumber = $"{param.phone_number}";
            staff.Gender = $"{param.gender}";
            staff.Salary = Convert.ToDecimal(param.salary);
            staff_station.StationId = $"{param.station_id}";
            Console.WriteLine(_context.Employees);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StaffExists(employee_id))
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

        [HttpPost]
        public ActionResult<string> PostStaff([FromBody] dynamic _employee)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set 'ModelContext.Employee' is null.");
            }
            dynamic employee = JsonConvert.DeserializeObject(Convert.ToString(_employee));
            string sql = "SELECT count(*) FROM EMPLOYEE";
            DataTable df = OracleHelper.SelectSql(sql);
            int df_count = df != null ? Convert.ToInt32(df.Rows[0][0]) : 0;
            string uid = "uid" + df_count.ToString("D10");

            Employee new_employee = new Employee()
            {
                EmployeeId = uid,
                Username = $"{employee.username}",
                Password = "123456",
                CreateTime = System.DateTime.Now,
                PhoneNumber = $"{employee.phone_number}",
                IdentityNumber = "xxxxxxxxxxxxxxxxxx",
                Gender = employee.gender=="男"?"m":"f",
                Positions = "fresh",
                Name = "lihua",
                Salary = Convert.ToDecimal(employee.salary)
            };

            EmployeeSwitchStation new_relation = new EmployeeSwitchStation()
            {
                EmployeeId = uid,
                StationId = $"{employee.station_id}"
            };

            _context.Employees.Add(new_employee);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (StaffExists(new_employee.EmployeeId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            _context.EmployeeSwitchStations.Add(new_relation);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (StaffInSwitchStationExists(new_relation.EmployeeId) || SwitchStationExists(new_relation.StationId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            var obj = new
            {
                employee_id = uid,
            };
            return Content(JsonConvert.SerializeObject(obj), "application/json");
        }

        [HttpDelete]
        public IActionResult DeleteStaff(string employee_id)
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }

            var staff = _context.Employees.Find(employee_id);
            var staff_station = _context.EmployeeSwitchStations.Find(employee_id);
            if (staff == null || staff_station == null)
            {
                return NotFound();
            }
            _context.EmployeeSwitchStations.Remove(staff_station);
            _context.SaveChanges();
            _context.Employees.Remove(staff);
            _context.SaveChanges();

            return NoContent();
        }

        private bool StaffExists(string id)
        {
            return _context.Employees?.Any(e => e.EmployeeId == id) ?? false;
        }

        private bool StaffInSwitchStationExists(string id)
        {
            return _context.EmployeeSwitchStations?.Any(e => e.EmployeeId == id) ?? false;
        }

        private bool SwitchStationExists(string id)
        {
            return _context.SwitchStations?.Any(e => e.StationId == id) ?? false;
        }
    }
}
