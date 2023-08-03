using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Models;
using Oracle.ManagedDataAccess.Client;
using Idcreator;
namespace webapi.Controllers
{
    public class AdministratorOperations
    {
        public void PublishAnnouncement(string title,string content,string location)
        {
            string sql = @"insert into ANNOUNCEMENT(
PUBLISH_TIME
, PUBLISH_POS
, ANNOUNCEMENT_ID
, VIEW_COUNT
, TITLE
, LIKES
, CONTENTS
) values(
:p_PUBLISH_TIME
, :p_PUBLISH_POS
, :p_ANNOUNCEMENT_ID
, :p_VIEW_COUNT
, :p_TITLE
, :p_LIKES
, :p_CONTENTS
);";
            List<OracleSpecialFields> a = new List<OracleSpecialFields>()
            {
                new OracleSpecialFields("PUBLISH_TIME",OracleDbType.TimeStamp,DateTime.UtcNow),
                new OracleSpecialFields("PUBLISH_POS",OracleDbType.Varchar2,location),
                new OracleSpecialFields("ANNOUNCEMENT_ID",OracleDbType.Varchar2,SnowflakeIDcreator.nextId()),
                new OracleSpecialFields("VIEW_COUNT",OracleDbType.Single,0),
                new OracleSpecialFields("TITLE",OracleDbType.Varchar2,title),
                new OracleSpecialFields("LIKES",OracleDbType.Single,0),
                new OracleSpecialFields("CONTENTS",OracleDbType.Varchar2,content)
        };
            OracleBLobHelper.InsertSpecialInfo(sql, a);
        }
        public void AddStaff(string stationID,string staffID)
        {
            string sql = "insert into STAFF_IN_SWITCH_STATIONS (SWITCH_STATIONS_ID,EMPLOYEE_ID)=('"+stationID+"','"+staffID +"')";
            OracleHelper.UpdateSql(sql);
        }
        public void DropStaff(string id)
        {
            string sql = "delete * from STAFF_IN_SWITCH_STATIONS where EMPLOYEE_ID='" + id + "';" +
                        "delete * from STAFF_IN_SWITCH_STATIONS where EMPLOYEE_ID='" + id + "';";
            OracleHelper.UpdateSql(sql);
            // OracleHelper.Operation(
            // OperationType.DROP,
            // "Staff",
            // new OracleSpecialFields("ID", OracleDbType.Varchar2, id)
            // );
        }
        public void ChangeStationName(string id,string name)
        {
            ManageStation(id, new OracleSpecialFields[] { new OracleSpecialFields("StationName", OracleDbType.Single, name) });
        }
        public void ChangeStationState(string id,string state)
        {
            ManageStation(id, new OracleSpecialFields[] { new OracleSpecialFields("StationState", OracleDbType.Single, state) });
        }
        public void ManageStation(string id,params OracleSpecialFields[] fields)
        {
            if(fields.Length==0)
                return;

            string column = "("+fields[0].Fieldname;
            for (int i = 1; i < fields.Length;i++)
            {
                column += ","+fields[i].Fieldname;
            }
            column += ")";

            string values = "(:" + fields[0].Fieldname;
            for (int i = 1; i < fields.Length; i++)
            {
                values += ",:" + fields[i].Fieldname;
            }
            values += ")";

            string sql = "update Switch_Station set"+
            column+"="+values+
            "where SWITCH_STATION_ID=" + id + ";";

            OracleBLobHelper.InsertSpecialInfo(sql, fields.ToList());
        }
    }
}