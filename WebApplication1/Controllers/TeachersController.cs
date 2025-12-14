using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebApplication1.Service;

namespace WebApplication1.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly DataService _dataService;
        public TeachersController(DataService dataService)
        {
            _dataService = dataService;
        }
        [HttpGet("Teachers")]
        public async Task<IActionResult> GetTeachers()
        {
            string query = "select * from teachers";
            var parameters = new Dictionary<string, object>();

            var dataTable = await _dataService.ExecuteQueryAsync(query, parameters);

            var teachers = new List<TeacherModel>();
            for (int i = 0; i <= dataTable.Rows.Count - 1; i++)
            {
                DataRow row = dataTable.Rows[i];
                teachers.Add(new TeacherModel
                {
                    Id = Convert.ToInt32(row["TeacherId"]),
                    FirstName = row["FirstName"].ToString() ?? "",
                    LastName = row["LastName"].ToString() ?? ""
                });

            }
            return Ok(teachers);

        }
         [HttpPost("AddTeacher")]
        public async Task<IActionResult> AddStudent([FromBody] TeacherModel teacher)
        {
            if (teacher == null || string.IsNullOrWhiteSpace(teacher.FirstName) || string.IsNullOrWhiteSpace(teacher.LastName) || teacher.FirstName = "string"
            || teacher.LastName == "string")
            {
                return BadRequest("Firstname and Lastname are required.");
            }

            string query = "INSERT INTO teachers (FirstName, LastName) VALUES (@FirstName, @LastName)";
            var parameters = new Dictionary<string, object>
            {
                { "@FirstName", teacher.FirstName },
                { "@LastName", teacher.LastName }
            };

            int rowsAffected = await _dataService.ExecuteNonQueryAsync(query, parameters);

            if (rowsAffected > 0)
            {
                return Ok(new { message = "Teacher added successfully!" });
            }
            else
            {
                return StatusCode(500, "Error inserting teacher.");
            }
        }
    }
}
