global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.RazorPages;
global using Microsoft.Data.SqlClient;

namespace Global_Spark_Academy.Pages.Super_Admin
{
    public class DashboardModel : PageModel
    {
        private readonly string connectionString = @"Data Source=MARVIN\SQLEXPRESS01;Initial Catalog=GSA;Integrated Security=True;Trust Server Certificate=True;";
        public string? ProfileImageUrl { get; set; }

        // Variables for storing the count of students in each semester
        #region -- STEM --
        public int STEMGrade11FirstSemesterCount { get; set; }
        public int STEMGrade11SecondSemesterCount { get; set; }
        public int STEMGrade12FirstSemesterCount { get; set; }
        public int STEMGrade12SecondSemesterCount { get; set; }
        #endregion

        #region -- ABM --
        public int ABMGrade11FirstSemesterCount { get; set; }
        public int ABMGrade11SecondSemesterCount { get; set; }
        public int ABMGrade12FirstSemesterCount { get; set; }
        public int ABMGrade12SecondSemesterCount { get; set; }
        #endregion

        #region -- HUMSS --
        public int HUMSSGrade11FirstSemesterCount { get; set; }
        public int HUMSSGrade11SecondSemesterCount { get; set; }
        public int HUMSSGrade12FirstSemesterCount { get; set; }
        public int HUMSSGrade12SecondSemesterCount { get; set; }
        #endregion

        #region -- SMAW --
        public int SMAWGrade11FirstSemesterCount { get; set; }
        public int SMAWGrade11SecondSemesterCount { get; set; }
        public int SMAWGrade12FirstSemesterCount { get; set; }
        public int SMAWGrade12SecondSemesterCount { get; set; }
        #endregion

        public void OnGet()
        {
            ProfileImageUrl = TempData["ProfileImageUrl"] as string;

            // SQL queries to get the count for each semester and strand
            string queryStemGrade11FirstSemester = "SELECT COUNT(*) FROM STEMGrade11FirstSemester";
            string queryStemGrade11SecondSemester = "SELECT COUNT(*) FROM STEMGrade11SecondSemester";
            string queryStemGrade12FirstSemester = "SELECT COUNT(*) FROM STEMGrade12FirstSemester";
            string queryStemGrade12SecondSemester = "SELECT COUNT(*) FROM STEMGrade12SecondSemester";

            string queryAbmGrade11FirstSemester = "SELECT COUNT(*) FROM ABMGrade11FirstSemester";
            string queryAbmGrade11SecondSemester = "SELECT COUNT(*) FROM ABMGrade11SecondSemester";
            string queryAbmGrade12FirstSemester = "SELECT COUNT(*) FROM ABMGrade12FirstSemester";
            string queryAbmGrade12SecondSemester = "SELECT COUNT(*) FROM ABMGrade12SecondSemester";

            string queryHumssGrade11FirstSemester = "SELECT COUNT(*) FROM HUMSSGrade11FirstSemester";
            string queryHumssGrade11SecondSemester = "SELECT COUNT(*) FROM HUMSSGrade11SecondSemester";
            string queryHumssGrade12FirstSemester = "SELECT COUNT(*) FROM HUMSSGrade12FirstSemester";
            string queryHumssGrade12SecondSemester = "SELECT COUNT(*) FROM HUMSSGrade12SecondSemester";

            string querySmawGrade11FirstSemester = "SELECT COUNT(*) FROM SMAWGrade11FirstSemester";
            string querySmawGrade11SecondSemester = "SELECT COUNT(*) FROM SMAWGrade11SecondSemester";
            string querySmawGrade12FirstSemester = "SELECT COUNT(*) FROM SMAWGrade12FirstSemester";
            string querySmawGrade12SecondSemester = "SELECT COUNT(*) FROM SMAWGrade12SecondSemester";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Get counts for STEM
                    STEMGrade11FirstSemesterCount = GetStudentCount(queryStemGrade11FirstSemester, connection);
                    STEMGrade11SecondSemesterCount = GetStudentCount(queryStemGrade11SecondSemester, connection);
                    STEMGrade12FirstSemesterCount = GetStudentCount(queryStemGrade12FirstSemester, connection);
                    STEMGrade12SecondSemesterCount = GetStudentCount(queryStemGrade12SecondSemester, connection);

                    // Get counts for ABM
                    ABMGrade11FirstSemesterCount = GetStudentCount(queryAbmGrade11FirstSemester, connection);
                    ABMGrade11SecondSemesterCount = GetStudentCount(queryAbmGrade11SecondSemester, connection);
                    ABMGrade12FirstSemesterCount = GetStudentCount(queryAbmGrade12FirstSemester, connection);
                    ABMGrade12SecondSemesterCount = GetStudentCount(queryAbmGrade12SecondSemester, connection);

                    // Get counts for HUMSS
                    HUMSSGrade11FirstSemesterCount = GetStudentCount(queryHumssGrade11FirstSemester, connection);
                    HUMSSGrade11SecondSemesterCount = GetStudentCount(queryHumssGrade11SecondSemester, connection);
                    HUMSSGrade12FirstSemesterCount = GetStudentCount(queryHumssGrade12FirstSemester, connection);
                    HUMSSGrade12SecondSemesterCount = GetStudentCount(queryHumssGrade12SecondSemester, connection);

                    // Get counts for SMAW
                    SMAWGrade11FirstSemesterCount = GetStudentCount(querySmawGrade11FirstSemester, connection);
                    SMAWGrade11SecondSemesterCount = GetStudentCount(querySmawGrade11SecondSemester, connection);
                    SMAWGrade12FirstSemesterCount = GetStudentCount(querySmawGrade12FirstSemester, connection);
                    SMAWGrade12SecondSemesterCount = GetStudentCount(querySmawGrade12SecondSemester, connection);

                }
                catch (Exception ex)
                {
                    // Handle exceptions and log error messages
                    Console.WriteLine($"Error fetching student counts: {ex.Message}");
                }
            }
        }

        private int GetStudentCount(string query, SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                return (int)command.ExecuteScalar();
            }
        }
    }
}
