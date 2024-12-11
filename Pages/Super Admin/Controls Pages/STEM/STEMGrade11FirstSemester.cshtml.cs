using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GlobalSparkAcademy.Pages.Super_Admin.Controls_Pages
{
    public class STEMGrade11FirstSemesterModel : PageModel
    {
        private readonly string _connectionString = @"Data Source=MARVIN\SQLEXPRESS01;Initial Catalog=GSA;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";

        public List<Student> Students { get; set; }

        public void OnGet()
        {
            Students = GetStudents();
        }

        private List<Student> GetStudents()
        {
            List<Student> students = new List<Student>();
            string query = "SELECT Id, FirstName, LastName, Email, MobileNumber, AccountPassword, Status FROM STEMGrade11FirstSemester";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        students.Add(new Student
                        {
                            Id = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            Email = reader.GetString(3),
                            MobileNumber = reader.GetString(4),
                            AccountPassword = reader.GetString(5),
                            Status = reader.GetString(6)
                        });
                    }
                }
            }

            return students;
        }

        public class Student
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string MobileNumber { get; set; }
            public string AccountPassword { get; set; }
            public string Status { get; set; }
        }
    }
}
