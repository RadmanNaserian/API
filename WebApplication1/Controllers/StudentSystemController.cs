using Microsoft.AspNetCore.Mvc;
using WebApplication1.Service;
using System.Data;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentSystemController : ControllerBase
    {
        private readonly DataService _dataService;

        public StudentSystemController(DataService dataService)
        {
            _dataService = dataService;
        }
        [HttpGet("Students")]
        public async Task<IActionResult> GetStudents()
        {
            string query = "SELECT * FROM Students";
            var parameters = new Dictionary<string, object>();

            var dataTable = await _dataService.ExecuteQueryAsync(query, parameters);

            var students = new List<StudentModel>();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                students.Add(new StudentModel
                {
                    Id = Convert.ToInt32(row["Id"]),
                    FirstName = row["FirstName"].ToString() ?? "",
                    LastName = row["LastName"].ToString() ?? ""
                });
            }

            return Ok(students);
        }

        [HttpPost("AddStudent")]
        public async Task<IActionResult> AddStudent([FromBody] StudentModel student)
        {
            if (student == null || string.IsNullOrWhiteSpace(student.FirstName) || string.IsNullOrWhiteSpace(student.LastName)
            ||student.FirstName == "string" || student.LastName == "string") 
            {
                return BadRequest("Firstname and Lastname are required.");
            }

            string query = "INSERT INTO Students (FirstName, LastName) VALUES (@FirstName, @LastName)";
            var parameters = new Dictionary<string, object>
            {
                { "@FirstName", student.FirstName },
                { "@LastName", student.LastName }
            };

            int rowsAffected = await _dataService.ExecuteNonQueryAsync(query, parameters);

            if (rowsAffected > 0)
            {
                return Ok(new { message = "Student added successfully!" });
            }
            else
            {
                return StatusCode(500, "Error inserting student.");
            }
        }
    }
}