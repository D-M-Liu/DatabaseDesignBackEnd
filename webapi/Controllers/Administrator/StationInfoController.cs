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
    [Route("administrator/stationInfo")]
    [ApiController]
    public class StationInfoController : ControllerBase
    {
        private readonly ModelContext _context;

        public StationInfoController(ModelContext context)
        {
            _context = context;
        }

        [HttpGet("query")]
        public ActionResult<IEnumerable<Employee>> GetPage_(int pageIndex, int pageSize, string station_name = "", string station_id = "", string employee_id = "",string faliure_status="")
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
            string pattern1 = "'%" + (station_name == String.Empty ? "" : station_name) + "%'";
            string pattern2 = "'%" + (station_id == String.Empty ? "" : station_id) + "%'";
            string pattern3 = employee_id == String.Empty ? "" : " AND " + "employee.employee_id like "+"'%" +  employee_id + "%'";
            string pattern4 = "'%" + (faliure_status == String.Empty ? "" : faliure_status) + "%'";
            string where_cause = "and " + "station_name like " + pattern1 +
                " AND " + "SWITCH_STATION.station_id like " + pattern2 +
                  pattern3 +
                " AND " + "faliure_status like " + pattern4;
            string sql_info = "SELECT SWITCH_STATION.station_id,employee.employee_id,station_name,LONGTITUDE,latitude,faliure_status,BATTERY_CAPACITY,available_battery_count,electricity_fee,service_fee " +
                "FROM " +
                @"employee Right OUTER JOIN EMPLOYEE_SWITCH_STATION ON EMPLOYEE.employee_id = EMPLOYEE_SWITCH_STATION.employee_id
Right OUTER JOIN SWITCH_STATION ON EMPLOYEE_SWITCH_STATION.station_id = SWITCH_STATION.station_id
WHERE positions = '管理员' or employee.employee_id IS NULL "
 + 
                where_cause +
                "ORDER BY station_id " +
                "OFFSET " + offset.ToString() + " ROWS " +
                "FETCH NEXT " + limit.ToString() + " ROWS ONLY";
            Console.Write(sql_info);
            DataTable df = OracleHelper.SelectSql(sql_info);
            string sql_total = "SELECT COUNT(*) " +
                "FROM EMPLOYEE " +
                "NATURAL JOIN EMPLOYEE_SWITCH_STATION " +
                "NATURAL JOIN SWITCH_STATION " + where_cause;
            int totalNum = df.Rows.Count;
            var obj = new
            {
                code=0,
                msg="success",
                totalData = totalNum,
                data = df,
            };
            return Content(JsonConvert.SerializeObject(obj), "application/json");
        }

        [HttpPatch]
        public IActionResult PutStaff([FromBody] dynamic _station)
        {
            dynamic station = JsonConvert.DeserializeObject(Convert.ToString(_station));
            string station_id = $"{station.station_id}";
            var staff = _context.SwitchStations.Find(station_id);

            if (staff == null)
            {
                return NewContent(1,"没找到该station");
            }

            

            //staff.StationId = $"{station.station_id}";
            if ($"{station.station_name}" != String.Empty)
                staff.StationName = $"{station.station_name}";
            if ($"{station.battety_capacity}" != String.Empty)
                staff.BatteryCapacity = Convert.ToDecimal(station.battety_capacity);
            if ($"{station.longitude}" != String.Empty)
                staff.Longtitude = Convert.ToDecimal(station.longitude);
            if ($"{station.latitude}" != String.Empty)
                staff.Latitude = Convert.ToDecimal(station.latitude);
            if ($"{station.faliure_status}" != String.Empty)
                staff.FaliureStatus = $"{station.faliure_status}";
            if ($"{station.available_battery_count}" != String.Empty)
                staff.AvailableBatteryCount = Convert.ToDecimal(station.available_battery_count);

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                return NewContent(1, e.InnerException?.Message+"");
            }

            if ($"{station.employee_id}" != String.Empty)
            {
                var staff_stations = _context.EmployeeSwitchStations.Where(e => e.StationId == staff.StationId);
                EmployeeSwitchStation? staff_station = null;
                
                foreach (var a in staff_stations)
                    if (a.StationId == $"{station.station_id}")
                        if (_context.Employees.Find(a.EmployeeId)?.Positions == "管理员")
                        {
                            staff_station = a;
                            break;   
                        }

                var newEmployee= _context.Employees.Find($"{station.employee_id}");

                if (newEmployee == null)
                {
                    return NewContent(1, "没有该id的管理员");
                }
                if (newEmployee.Positions != "管理员")
                    return NewContent(1, "指定id的员工不是管理员");
                if (staff_station!=null)
                {
                    _context.EmployeeSwitchStations.Remove(staff_station);
                    _context.SaveChanges();
                    staff_station.Employee = newEmployee;

                    //_context.EmployeeSwitchStations.Add(new EmployeeSwitchStation(){ EmployeeId = $"{station.employee_id}", StationId = station_id, });
                    staff_station.EmployeeId = $"{station.employee_id}";
                    _context.EmployeeSwitchStations.Add(staff_station);
                }
                else
                {
                    staff_station = new EmployeeSwitchStation()
                    {
                        EmployeeId = $"{station.employee_id}",
                        StationId = station_id
                    };
                    _context.EmployeeSwitchStations.Add(staff_station);
                }
            }
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                return NewContent(1, e.InnerException?.Message+"");
            }
            
            return NewContent();
        }

        [HttpPost]
        public ActionResult<string> PostStaff([FromBody] dynamic _station)
        {
            dynamic station = JsonConvert.DeserializeObject(Convert.ToString(_station));
            string employee_id = $"{station.employee_id}";
            SwitchStation new_station = new SwitchStation()
            {
                StationId = $"{station.station_id}",
                StationName= $"{station.station_name}",
                BatteryCapacity = Convert.ToDecimal(station.battety_capacity),
                Longtitude = Convert.ToDecimal(station.longitude),
                Latitude=Convert.ToDecimal(station.latitude),
                FaliureStatus= $"{station.faliure_status}",
                AvailableBatteryCount= Convert.ToDecimal(station.available_battery_count)
            };
    
            _context.SwitchStations.Add(new_station);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                String msg = e.InnerException?.Message+"";
                if (_context.SwitchStations.Any(t=>t.StationId==new_station.StationId))
                    msg = "已有该id的换电站";
                else if (_context.SwitchStations.Any(t => (t.Latitude == new_station.Latitude) && (t.Longtitude == new_station.Longtitude)))
                    msg = "已有该位置的换电站";
                return NewContent(1, msg);
            }
            if ($"{station.employee_id}" != String.Empty)
            {
                EmployeeSwitchStation new_relation = new EmployeeSwitchStation()
                {
                    EmployeeId = employee_id,
                    StationId = $"{station.station_id}"
                };
                _context.EmployeeSwitchStations.Add(new_relation);

                try
                {
                    _context.SaveChanges();
                }
                catch (DbUpdateException e)
                {
                    String msg = e.InnerException?.Message+"";
                    if (_context.EmployeeSwitchStations.Find(employee_id) != null)
                    {
                        msg = "employ_id已占用";
                    }
                    else if (_context.Employees.Find(employee_id) == null)
                    {
                        msg = "无该员工";
                    }
                    else if (_context.Employees.Find(employee_id)?.Positions != "管理员")
                    {
                        msg = "该员工不是管理员";
                    }
                    return NewContent(2,msg);
                }

            }
            var returnMessage = new
            {
                code = 0,
                msg = "success"
            };
            return Content(JsonConvert.SerializeObject(returnMessage), "application/json");
        }

        [HttpDelete]
        public IActionResult DeleteStaff(string station_id)
        {

            var station = _context.SwitchStations.Find(station_id);
            var staff_stations = _context.EmployeeSwitchStations.Where(e => e.StationId == station_id);
            if (station == null )
            {
                return NewContent(1,"找不到该station");
            }

            foreach (var a in staff_stations)
            {
                _context.EmployeeSwitchStations.Remove(a);
            }
            _context.SwitchStations.Remove(station);
            try
            {
                _context.SaveChanges();
            }
            catch(DbUpdateException e)
            {
                return NewContent(1, e.InnerException?.Message+"");
            }
            return NewContent();
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

        ContentResult NewContent(int _code=0,string _msg="success")
        {
            var a= new
            {
                code = _code,
                msg = _msg
            };
            return Content(JsonConvert.SerializeObject(a), "application/json");
        }
    }
}
