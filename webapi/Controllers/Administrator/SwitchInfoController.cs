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
using Oracle.ManagedDataAccess.Client;

namespace webapi.Controllers.Administrator
{
    [Route("administrator/switch-info")]
    [ApiController]
    public class SwitchInfoController : ControllerBase
    {
        private readonly ModelContext _context;

        public SwitchInfoController(ModelContext context)
        {
            _context = context;
        }

        [HttpGet("query")]
        public ActionResult<IEnumerable<Employee>> GetPage_(int page_index, int page_size, string switch_service_id = "", string employee_id = "", string vehicle_id = "")
        {
            int offset = (page_index - 1) * page_size;
            int limit = page_size;
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

            // 搞不懂 参数化查询查不到东西，普通查询可以
            /*OracleParameter paramSwitchServiceId = new OracleParameter(":pSwitchServiceId", OracleDbType.NVarchar2) { Value = $"%{switch_service_id}%" };
            OracleParameter paramEmployeeId = new OracleParameter(":pEmployeeId", OracleDbType.NVarchar2, 7, ParameterDirection.InputOutput) { Value = $"'%{(employee_id == String.Empty ? "" : employee_id)}%'" };
            OracleParameter paramVehicleId = new OracleParameter(":pVehicleId", OracleDbType.NVarchar2) { Value = $"%{vehicle_id}%" };
            OracleParameter paramOffset = new OracleParameter(":pOffset", OracleDbType.Int32) { Value = offset };
            OracleParameter paramLimit = new OracleParameter(":pLimit", OracleDbType.Int32) { Value = limit };

            string sqlQuery = "SELECT switch_service_id, employee_id, vehicle_id, switch_time, battery_id_on, battery_id_off, evaluations " +
                                  "FROM switch_log " +
                                  "WHERE switch_service_id LIKE :pSwitchServiceId " +
                                  "AND employee_id LIKE :pEmployeeId " +
                                  "AND vehicle_id LIKE :pVehicleId " +
                                  "ORDER BY switch_service_id " +
                                  "OFFSET :pOffset ROWS " +
                                  "FETCH NEXT :pLimit ROWS ONLY";
            string sqlTotal = "SELECT COUNT(*) FROM switch_log " +
                                  "WHERE switch_service_id LIKE :pSwitchServiceId " +
                                  "AND employee_id LIKE :pEmployeeId " +
                                  "AND vehicle_id LIKE :pVehicleId";*/

            string pattern1 = "'%" + (switch_service_id  == String.Empty ? "" : switch_service_id) + "%'";
            string pattern2 = "'%" + (employee_id == String.Empty ? "" : employee_id) + "%'";
            string pattern3 = "'%" + (vehicle_id == String.Empty ? "" : vehicle_id) + "%'";
            string where_cause = "WHERE " + "switch_service_id like " + pattern1 +
                " AND " + "employee_id like " + pattern2 +
                " AND " + "vehicle_id like " + pattern3 + " ";
            string sqlQuery = "SELECT switch_service_id, employee_id, vehicle_id, switch_time, battery_id_on, battery_id_off, evaluations " +
                                  "FROM switch_log " + where_cause +
                                  "ORDER BY switch_service_id " +
                                  "OFFSET " + offset.ToString() + " ROWS " +
                                  "FETCH NEXT " + limit.ToString() + " ROWS ONLY";
            string sqlTotal = "SELECT COUNT(*) " + "FROM switch_log " + where_cause;
            string errorMessage = string.Empty;
            DataTable df = OracleHelper.SelectSql(sqlQuery, ref errorMessage);
            DataTable dfCount = OracleHelper.SelectSql(sqlTotal, ref errorMessage);
            int totalNum = dfCount != null ? Convert.ToInt32(dfCount.Rows[0][0]) : 0;
            var obj = new
            {
                totalData = totalNum,
                data = df,
            };
            Console.WriteLine(df);
            return Content(JsonConvert.SerializeObject(obj), "application/json");
        }

        [HttpDelete]
        public IActionResult DeleteLog(string switch_service_id)
        {
            if (_context.SwitchLogs == null)
            {
                return NotFound();
            }

            var ServiceLog = _context.SwitchLogs.Find(switch_service_id);
            if (ServiceLog == null)
            {
                return NotFound();
            }
            _context.SwitchLogs.Remove(ServiceLog);
            _context.SaveChanges();

            return NoContent();
        }

        private bool LogExists(string id)
        {
            return _context.SwitchLogs?.Any(e => e.SwitchServiceId == id) ?? false;
        }
    }
}
