using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json.Serialization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webapi.Controllers
{
    public class Name
    {
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }
        [JsonPropertyName("lastName")]
        public string LastName { get; set; }
        public Name() { FirstName = string.Empty; LastName = string.Empty; }
        public Name(string first,string last) { FirstName = first; LastName = last; }
    }

    [Route("[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ValuesController>/details?id= &s=
        [HttpGet("details")]
        public IActionResult Get(int id,string s)
        {
            Console.WriteLine(id + s);
            return Content("GET success");
        }

        // POST api/<ValuesController>
        [HttpPost]
        public IActionResult Post([FromBody] Name value)
        {
            Console.WriteLine(value.FirstName);
            return Content("POST success");
        }

        // POST api/<ValuesController>/img
        [HttpPost("img")]
        public IActionResult PostIMG([FromBody] Name value)
        {
            byte[] fileByte;
            // 如果是内存流，可以直接读到字节数组
            using (FileStream fs = new FileStream(@"D:\Demo\e-Woodblock\src\assets\adjustment.png", FileMode.Open))
            {
                BinaryReader br = new BinaryReader(fs);
                br.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开
                fileByte = br.ReadBytes((int)br.BaseStream.Length); // 将流读
                return File(fileByte, "application/octet-stream");
            }
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
