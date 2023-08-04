using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Oracle.ManagedDataAccess.Client;
using System.Text.RegularExpressions;

namespace webapi
{
    public class TestDataOperator
    {
        public static void SetGender()
        {
            for (int i = 82; i < 500; i++)
            {
                try
                {
                    OracleHelper.UpdateSql("update C##CAR.STAFF set Gender='" + RanGender() + "' where EMPLOYEE_ID='" + (10000 + i) + "'");
                    Console.Write(" " + i);
                }
                catch
                {

                }
            }
        }
        static Random rand = new Random();
        const string assetPath = "E:\\AI\\SD_webui\\sd-webui-aki\\sd-webui-aki-v4\\sd-webui-aki-v4\\outputs\\txt2img-images";
        public static string Values(List<string> values)
        {
            string result = "values(";
            for (int i = 0; i < values.Count;i++)
            {
                if(values[i][0]!='@')
                    result += "'"+values[i] + "'";
                    //else if(to_timestamp('2006-01-01 12:10:10.1','yyyy-mm-dd hh24:mi:ss.ff'))
                    else
                    result += values[i].Remove(0,1);
                
                if(i<values.Count-1)
                    result += ",";
            }
            return result+")";
        }
        public static void AddStaffs(int num=1)
        {
            string sql = "insert into C##CAR.STAFF"+
            "(EMPLOYEE_ID,USERNAME,PASSWORD_P,AVATAR,CREATI_TIME,PHONE_NUMBER,IDENTITY_NUMBER,NAME_P,GENDER,POSITIONS,SALARY) "+
            Values(new List<string>(){RanID(),RanUserName(),RanPassWord(),"@:1","@:2",RanPhone(),RanIDNumber(),RanName(),RanGender(),"@null","@"+RanSalary().ToString()});
            List<OracleSpecialFields> a = new List<OracleSpecialFields>()
            {
                new OracleSpecialFields("AVATAR",OracleDbType.Blob,RanAvatar()),
                new OracleSpecialFields("CREATI_TIME",OracleDbType.TimeStamp,RanTime())
            };
            OracleBLobHelper.InsertSpecialInfo(sql,a);  
        }
        public static DateTime RanTime()
        {
            DateTime startDateTime = new DateTime(2000, 1, 1);
            DateTime endDateTime = DateTime.UtcNow;
            TimeSpan diff = endDateTime - startDateTime;
            Double totalSeconds = diff.TotalSeconds;
            Double randomSeconds = rand.NextDouble() * totalSeconds;
            DateTime randomDateTime = startDateTime.AddSeconds(randomSeconds);
            return randomDateTime;
        }
        public static string RanGender()
        {
            return rand.Next(0, 2) > 0 ? "m" : "f";
        }
        static int index=0;
        public static string RanID()
        {
            index++;
            return (9999+index).ToString();
        }
        public static string RanFile()
        {
            var files = Directory.GetFiles(assetPath, "*.png");
            return files[rand.Next(0, files.Length - 1)];
        }
        public static string RanUserName()
        {
            string pathName1 = Regex.Replace(Path.GetFileNameWithoutExtension(RanFile()), @"\d", "");
            string pathName2 = Regex.Replace(Path.GetFileNameWithoutExtension(RanFile()), @"\d", "");
            List<char> chs = new List<char> { ' ', '_', '.','-',',','{','}','(',')'};
            var names1 = MuliSplit(pathName1, chs);
            var names2 = MuliSplit(pathName2, chs);
            try
            {
                var name=names1[rand.Next(2, names1.Length - 1)]+" "+names2[rand.Next(2, names2.Length - 1)];
                Console.WriteLine(index - 1 + "---"+name);
                return name;
            }
            catch(Exception e)
            {
                Console.Write(e + "-------" + pathName1 + pathName2);
                return RanName();
            }
        }
        static string[] MuliSplit(string str,List<char> chs)
        {
            string[] result = {};
            if (chs.Count == 0)
                return result;

            for (int i = 0; i < chs.Count; i++)
            {
                //str.Replace(","," ");
                str=str.Replace(chs[i], chs[0]);
            }
            foreach (var a in str.Split(chs[0]))
            {
                if(a.Length>=1)
                    result=result.Append(a).ToArray();
            }
            return result;
        }
        public static string RanPassWord()
        {
            string password=string.Empty;
            int Length = rand.Next(6, 10);
            for (int i = 0; i < Length; i++)
            {
                int a = rand.Next(0, 35);
                if (0 <= a && a < 10)
                {
                    password+= (char)((int)'0'+a);
                }
                else
                {
                    password += (char)((int)'a' + (a - 10));
                }
            }
            return password;
        }
        public static byte[] RanAvatar()
        {
            string path = RanFile();
            byte[] buffByte;
            try
            {
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                buffByte = new byte[fs.Length];
                fs.Read(buffByte, 0, Convert.ToInt32(fs.Length));
                fs.Close();
            }
            catch(Exception e) { Console.WriteLine(e.Message);return null; }
            return buffByte;
        }
        public static string RanName()
        {
            List<string> surname = new List<string>(){"张","王","李","刘","赵","顾","周","马" };
            List<string> name=new List<string>(){"辉","惠","瑞","睿","涛","华","智","勇","乐","涵","嘉","仪","烨","一","德","荣","俊","凯","斌","茂","娟"};
            return surname[rand.Next(0, surname.Count - 1)] + name[rand.Next(0, name.Count - 1)] + (rand.Next(0, name.Count + 2) < name.Count ? name[rand.Next(0, name.Count - 1)] : "");
        }
        public static string RanPhone()
        {
            return rand.NextInt64(10000000000,19999999999).ToString();
        }
        public static string RanIDNumber()
        {
            return rand.NextInt64(300000000,399999999).ToString()+rand.NextInt64(100000000,999999999).ToString();
        }
        public static int RanSalary()
        {
            return rand.Next(5000, 15000);
        }
    }
}