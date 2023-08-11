using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using NuGet.ContentModel;
using NuGet.Packaging.Signing;
using NuGet.Protocol;
using System.Data;
using System.Net;
using System.Reflection.Metadata;
using System.Xml.Linq;
using webapi.Models;

namespace webapi.Controllers.Administrator
{
    [Route("administrator/owner-info")]
    [ApiController]
    public class VehicleOwnerController : ControllerBase
    {
        private readonly ModelContext _context;

        public VehicleOwnerController(ModelContext context)
        {
            _context = context;
        }

        [HttpGet("message")]
        public ActionResult<IEnumerable<VehicleOwner>> GetPage(int pageIndex, int pageSize)
        {
            int offset = (pageIndex - 1) * pageSize;
            int limit = pageSize;
            if (offset < 0 || limit <= 0)
            {
                var t = new
                {
                    code = 1,
                    msg = "页码或页大小非正",
                    totalData = 0,
                    data = "",
                };
                return Content(JsonConvert.SerializeObject(t), "application/json");
            }
            string sql_info = "SELECT owner_id, username, gender, phone_number, address, password " +
                "FROM vehicle_owner " +
                "ORDER BY owner_id " +
                "OFFSET " + offset.ToString() + " ROWS " +
                "FETCH NEXT " + limit.ToString() + " ROWS ONLY";
            DataTable df = OracleHelper.SelectSql(sql_info);
            string sql_total = "SELECT COUNT(*) " +
                "FROM vehicle_owner ";
            DataTable df_count = OracleHelper.SelectSql(sql_total);
            int totalNum = df_count != null ? Convert.ToInt32(df_count.Rows[0][0]) : 0;
            var obj = new
            {
                code = 0,
                msg = "success",
                totalData = totalNum,
                data = df,
            };
            return Content(JsonConvert.SerializeObject(obj), "application/json");
        }

        [HttpGet("query")]
        public ActionResult<IEnumerable<VehicleOwner>> GetPage_(int pageIndex, int pageSize, string owner_id = "", string username = "", string gender = "", string phone_number = "", string address = "", string password = "")
        {
            int offset = (pageIndex - 1) * pageSize;
            int limit = pageSize;
            if (offset < 0 || limit <= 0)
            {
                var t = new
                {
                    code = 1,
                    msg = "页码或页大小非正",
                    totalData = 0,
                    data = "",
                };
                return Content(JsonConvert.SerializeObject(t), "application/json");
            }
            string pattern1 = "'%" + (owner_id == String.Empty ? "" : owner_id) + "%'";
            string pattern2 = "'%" + (username == String.Empty ? "" : username) + "%'";
            string pattern3 = "'%" + (gender == String.Empty ? "" : gender) + "%'";
            string pattern4 = "'%" + (phone_number == String.Empty ? "" : phone_number) + "%'";
            string pattern5 = "'%" + (address == String.Empty ? "" : address) + "%'";
            string pattern6 = "'%" + (password == String.Empty ? "" : password) + "%'";
            string where_cause = "WHERE " + "owner_id like " + pattern1 +
                " AND " + "username like " + pattern2 +
                " AND " + "gender like " + pattern3 +
                " AND " + "phone_number like " + pattern4 +
                " AND " + "address like " + pattern5 +
                " AND " + "password like " + pattern6 + " ";
            string sql_info = "SELECT owner_id, username, gender, phone_number, address, password " +
                "FROM vehicle_owner " + where_cause +
                "ORDER BY owner_id " +
                "OFFSET " + offset.ToString() + " ROWS " +
                "FETCH NEXT " + limit.ToString() + " ROWS ONLY";
            DataTable df = OracleHelper.SelectSql(sql_info);
            string sql_total = "SELECT COUNT(*) " +
                "FROM vehicle_owner " + where_cause;
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
        public IActionResult PutOwner([FromBody] dynamic _param)
        {
            dynamic param = JsonConvert.DeserializeObject(Convert.ToString(_param));
            string owner_id = $"{param.owner_id}";
            var owner = _context.VehicleOwners.Find(owner_id);
            if (owner == null)
            {
                return NotFound();
            }
            owner.OwnerId = $"{param.owner_id}";
            owner.Gender = $"{param.gender}";
            owner.PhoneNumber = $"{param.phone_number}";
            owner.Address = $"{param.address}";
            owner.Password = $"{param.password}";
            owner.Username = $"{param.username}";
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                return NewContent(1, e.InnerException.Message);
            }

            return NewContent();
        }

        [HttpPost]
        public ActionResult<string> PostOwner([FromBody] dynamic _owner)
        {
            if (_context.VehicleOwners == null)
            {
                return Problem("Entity set 'ModelContext.VehicleOwner' is null.");
            }
            dynamic owner = JsonConvert.DeserializeObject(Convert.ToString(_owner));
            string sql = "SELECT MAX(owner_id) FROM EMPLOYEE";
            DataTable df = OracleHelper.SelectSql(sql);
            int df_count = df != null ? Convert.ToInt32(df.Rows[0][0]) : 0;
            string uid = "uid" + df_count.ToString("D15");

            VehicleOwner new_owner = new VehicleOwner()
            {
                OwnerId = uid,
                Username = $"{owner.username}",
                Nickname = "user_0.00" + df_count.ToString("D8"),
                Password = "123456",
                ProfilePhoto = null,
                CreateTime = System.DateTime.Now,
                PhoneNumber = $"{owner.phone_number}",
                Email = "wl@car.com",
                Gender = $"{owner.gender}",
                Birthday = null,
                Address = $"{owner.address}"
            };

            _context.VehicleOwners.Add(new_owner);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                var a = new
                {
                    code = 1,
                    msg = e.InnerException?.Message
                };

                return Conflict(a);
            }

            var returnMessage = new
            {
                code = 0,
                msg = "success"
            };
            return Content(JsonConvert.SerializeObject(returnMessage), "application/json");
        }

        [HttpDelete]
        public IActionResult DeleteOwner(string owner_id)
        {
            if (_context.VehicleOwners == null)
            {
                return NotFound();
            }

            var owner = _context.VehicleOwners.Find(owner_id);
            if (owner == null)
            {
                return NewContent(1, "找不到该车主");
            }
            _context.VehicleOwners.Remove(owner);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                return NewContent(1, e.InnerException?.Message);
            }
            return NewContent();
        }

        private bool OwnerExists(string id)
        {
            return _context.VehicleOwners?.Any(e => e.OwnerId == id) ?? false;
        }

        ContentResult NewContent(int _code = 0, string _msg = "success")
        {
            var a = new
            {
                code = _code,
                msg = _msg
            };
            return Content(JsonConvert.SerializeObject(a), "application/json");
        }
    }
}
