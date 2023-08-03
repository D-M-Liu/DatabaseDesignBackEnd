using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.IO;
using System.Text;
using System.Text.Json;


namespace webapi
{
    public class OracleHelper
    {
        public static string lastMessage = "";
        public static Boolean lastRe = false;
        public static string _connectString = null;
        public static string connectString
        {
            get
            {
                if (_connectString == null)
                {
                    String value = "";
                    StreamReader sr = new StreamReader("./appsettings.json");
                    value = sr.ReadToEnd();
                    sr.Close();
                    try
                    {
                        string[] strTmps = value.Split('"');
                        for (int i = 0; i < strTmps.Length; i++)
                        {
                            if (strTmps[i].Equals("BloggingDatabase"))
                            {
                                return strTmps[i + 2];
                            }
                        }
                        return "";
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        throw;
                    }
                    return "";
                }
                else
                    return _connectString;
            }

        }

        #region 数据库连接
        public static OracleConnection DbConn(ref string message, ref Boolean re)
        {

            //数据库的连接的方式
            OracleConnection conn;
            re = false;
            message = "";
            conn = new OracleConnection(connectString);
            try
            {
                conn.Open();
                re = true;
            }
            catch (Exception ex)
            {
                message = "错误：" + ex.Message.ToString();
                re = false;
                return null;
            }
            finally
            {
                conn.Close();
            }

            return conn;
        }
        public static OracleConnection DbConn()
        {
            return DbConn(ref lastMessage, ref lastRe);
        }
        #endregion

        #region 增
        public static Boolean AddSql(string sql, ref string message)
        {
            bool re = false;
            try
            {
                OracleConnection conn = DbConn(ref message, ref re);
                if (conn == null)
                {
                    re = false;
                    message = "数据库连接对象为空";
                }
                else
                {
                    try
                    {
                        if (conn.State == ConnectionState.Closed)
                        {
                            conn.Open();
                        }
                    }
                    catch (Exception eee)
                    {
                        re = false;
                        message = eee.Message.ToString();
                    }
                    OracleCommand cmd = new OracleCommand(sql, conn);
                    int count = cmd.ExecuteNonQuery();
                    if (count > 0)
                    {
                        re = true;
                    }
                    else
                    {
                        re = false;
                        message = "数据插入失败";
                    }
                    cmd.Dispose();
                    conn.Close();
                }
            }
            catch (Exception ee)
            {
                re = false;
                message = "add数据出错,原因:" + ee.Message.ToString();
            }
            return re;
        }
        public static Boolean AddSql(string sql)
        {
            return AddSql(sql, ref lastMessage);
        }
        #endregion

        #region 删
        public static Boolean DeleteSql(string sql, ref string message)
        {
            bool re = false;
            try
            {
                OracleConnection conn = DbConn(ref message, ref re);
                if (conn == null)
                {
                    re = false;
                    message = "数据库连接对象为空";
                }
                else
                {
                    try
                    {
                        if (conn.State == ConnectionState.Closed)
                        {
                            conn.Open();
                        }
                    }
                    catch (Exception eee)
                    {
                        re = false;
                        message = eee.Message.ToString();
                    }
                    OracleCommand cmd = new OracleCommand(sql, conn);
                    int count = cmd.ExecuteNonQuery();
                    if (count > 0)
                    {
                        re = true;
                    }
                    else
                    {
                        re = false;
                        message = "数据删除失败";
                    }
                    cmd.Dispose();
                    conn.Close();
                }
            }
            catch (Exception ee)
            {
                re = false;
                message = "add数据出错,原因:" + ee.Message.ToString();
            }
            return re;
        }
        #endregion

        #region 改每次调用都会调用代码
        public static Boolean UpdateSql(string sql, ref string message)
        {
            bool re = false;
            try
            {
                OracleConnection conn = DbConn(ref message, ref re);
                if (conn == null)
                {
                    re = false;
                    message = "数据库连接对象为空";
                }
                else
                {
                    try
                    {
                        if (conn.State == ConnectionState.Closed)
                        {
                            conn.Open();
                        }
                    }
                    catch (Exception eee)
                    {
                        re = false;
                        message = eee.Message.ToString();
                    }
                    OracleCommand cmd = new OracleCommand(sql, conn);
                    int count = cmd.ExecuteNonQuery();
                    if (count > 0)
                    {
                        re = true;
                    }
                    else
                    {
                        re = false;
                        message = "数据更新失败";
                    }
                    cmd.Dispose();
                    conn.Close();
                }
            }
            catch (Exception ee)
            {
                re = false;
                message = "add数据出错,原因:" + ee.Message.ToString();
            }
            return re;
        }
        public static Boolean UpdateSql(string sql)
        {
            return UpdateSql(sql, ref lastMessage);
        }
        #endregion

        #region 查
        public static DataTable SelectSql(string sql, ref string message)
        {

            DataTable dt = null;
            OracleConnection conn;
            message = string.Empty;
            string err = "";
            bool re = false;
            try
            {
                conn = DbConn(ref err, ref re);
                if (conn == null)
                {
                    message = "数据库连接对象为空";
                    return null;
                }
                if (conn.State == ConnectionState.Closed)
                {
                    try
                    {
                        conn.Open();
                    }
                    catch (Exception ex)
                    {
                        message = ex.Message;
                        return null;
                    }
                }
            }
            catch (Exception ee)
            {
                message = "数据库连接失败,原因:" + ee.Message.ToString();
                return null;
            }
            try
            {
                OracleDataAdapter adapter = new OracleDataAdapter(sql, conn);
                DataSet set = new DataSet();
                adapter.Fill(set);
                dt = set.Tables[0];
                conn.Close();
                conn.Dispose();
                adapter.Dispose();
                set.Dispose();
            }
            catch (Exception ex)
            {
                message = ex.Message.ToString(); ;
            }
            return dt;
        }
        public static DataTable SelectSql(string sql)
        {
            return SelectSql(sql, ref lastMessage);
        }
        #endregion

        public static void Operation(OperationType operationType,string table,OracleSpecialFields id,List<OracleSpecialFields> fields=null)
        {
            string column = string.Empty, values = string.Empty;
            if (fields==null||fields.Count != 0&&operationType!=OperationType.DROP)
                return;
            else
            {
                column = "(" + fields[0].Fieldname;
                for (int i = 1; i < fields.Count; i++)
                {
                    column += "," + fields[i].Fieldname;
                }
                column += ")";

                values = "(:" + fields[0].Fieldname;
                for (int i = 1; i < fields.Count; i++)
                {
                    values += ",:" + fields[i].Fieldname;
                }
                values += ")";
            }
            string sql=string.Empty;
            switch (operationType)
            {
                case OperationType.UPDATE:
                    sql = "update "+ table +" set " +
                    column + "=" + values +
                    " where "+id.Fieldname+"='" + id.FieldValue + "';";
                    break;
                case OperationType.ADD:
                    sql = "insert into " + table + 
                    column + "=" + values+";";
                    break;
                case OperationType.DROP:
                    sql="drop * from "+table+
                    " where " + id.Fieldname + "='" + id.FieldValue + "';";
                    break;
                default:
                    break;
            }

            OracleBLobHelper.InsertSpecialInfo(sql, fields.ToList());
        }
    }

    public class OracleBLobHelper
    {
        public static byte[] getBlobContext(string sql)
        {
            byte[] blob = null;
            using (OracleConnection conn = OracleHelper.DbConn())
            {
                try
                {
                    conn.Open();
                    OracleCommand cmd = new OracleCommand(sql, conn);
                    OracleDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    blob = new Byte[(reader.GetBytes(0, 0, null, 0, int.MaxValue))];
                    reader.GetBytes(0, 0, blob, 0, blob.Length);
                    reader.Close();
                }
                catch (Exception ex)
                {
                    // LogHelper.LogHelper.logHelper.ErrorLog(ex.Message, "");
                    //LogHelper.LogHelper.logHelper.ErrorLog(sql, "");                 
                }
                finally
                {
                    conn.Close();
                }
            }
            return blob;
        }

        /**
         * 插入特殊字符信息
         * */
        public static bool InsertSpecialInfo(string sql, List<OracleSpecialFields> lst)
        {

            using (OracleConnection conn = OracleHelper.DbConn())
            {
                try
                {
                    conn.Open();
                    OracleCommand cmd = new OracleCommand(sql, conn);
                    cmd.CommandType = CommandType.Text;
                    foreach (OracleSpecialFields sf in lst)
                    {
                        OracleParameter oracleParameter = new OracleParameter(sf.Fieldname, sf.Fieldtype);
                        if (sf.FieldValue == "" || sf.FieldValue == null)
                        {
                            oracleParameter.Value = DBNull.Value;
                        }
                        else
                        {
                            oracleParameter.Value = sf.FieldValue;
                        }
                        cmd.Parameters.Add(oracleParameter);
                    }
                    //LogHelper.LogHelper.logHelper.InfoLog(sql, "");
                    int rows = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
                finally
                {
                    conn.Close();
                }
                return false;
            }

        }
    }
    public class OracleSpecialFields
    {
        public OracleSpecialFields() { }
        public OracleSpecialFields(string _fieldname, OracleDbType _fieldtype, object _fieldValue)
        {
            Fieldname = _fieldname;
            Fieldtype = _fieldtype;
            FieldValue = _fieldValue;
        }
        private string fieldname;

        public string Fieldname
        {
            get { return fieldname; }
            set { fieldname = value; }
        }
        private OracleDbType fieldtype;

        public OracleDbType Fieldtype
        {
            get { return fieldtype; }
            set { fieldtype = value; }
        }
        private object fieldValue;

        public object FieldValue
        {
            get { return fieldValue; }
            set { fieldValue = value; }
        }
    }

    public enum OperationType
    {
        ADD, DROP, UPDATE
    }
}