using System;
using System.Data.SqlClient;
using GlobalSparkAcademy.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Global_Spark_Academy.Pages.Registration
{
    public class LoginModel : PageModel
    {
        private SuperAdminLoginConnection SuperAdminLoginConnection;

        private readonly string connectionString = @"Data Source=MARVIN\SQLEXPRESS01;Initial Catalog=GSA;Integrated Security=True;Trust Server Certificate=True;";

        #region -- Descriptions --
        public string[]? Descriptions { get; set; }
        #endregion

        #region -- Form Data --
        public bool IsFormValid { get; set; } = true;

        [BindProperty]
        public string? Email { get; set; }

        [BindProperty]
        public string? Password { get; set; }

        [BindProperty]
        public bool RememberMe { get; set; }
        #endregion

        public LoginModel()
        {
            Descriptions = new[] {
                "Welcome to the Global Spark Academy login portal, designed for secure access by Students, Admins, and Super Admins. Each user role is provided tailored access to their respective features and dashboards."
            };
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                IsFormValid = false;
                return Page();
            }

            string userRole = ValidateLogin(Email, Password);

            if (!string.IsNullOrEmpty(userRole))
            {
                if (userRole == "SuperAdmin")
                {
                    var superAdminData = new SuperAdminLoginConnection(Email, Password);

                    // Optionally store data in TempData or ViewData for further use
                    TempData["SuperAdminName"] = $"{superAdminData.FirstName} {superAdminData.LastName}";
                    TempData["ProfileImageUrl"] = superAdminData.ProfileImage;

                    return RedirectToPage("/Super Admin/Dashboard");
                }
                else if (userRole == "STEMStudent")
                {
                    return RedirectToPage("/Student/Dashboard");
                }
            }

            IsFormValid = false;
            return Page();
        }


        public void OnGet()
        {
        }

        private string ValidateLogin(string email, string password)
        {
            string userRole = string.Empty;

            string query = @"
                SELECT CASE
                    WHEN EXISTS (SELECT 1 FROM SuperAdminRegistrations WHERE EmailAddress = @Email AND Password = @Password)
                        THEN 'SuperAdmin'
                    WHEN EXISTS (SELECT 1 FROM STEMGrade11FirstSemester WHERE Email = @Email AND AccountPassword = @Password)
                        THEN 'STEMStudent'
                    WHEN EXISTS (SELECT 1 FROM STEMGrade11SecondSemester WHERE Email = @Email AND AccountPassword = @Password)
                        THEN 'STEMStudent'
                    WHEN EXISTS (SELECT 1 FROM STEMGrade12FirstSemester WHERE Email = @Email AND AccountPassword = @Password)
                        THEN 'STEMStudent'
                    WHEN EXISTS (SELECT 1 FROM STEMGrade12SecondSemester WHERE Email = @Email AND AccountPassword = @Password)
                        THEN 'STEMStudent'
                    ELSE NULL
                END AS UserRole";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            userRole = result.ToString() ?? string.Empty;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error during login validation: {ex.Message}");
                    }
                }
            }

            return userRole;
        }

        private string? GetUserProfileImage(string email, string password)
        {
            string? profileImageUrl = null;

            string query = @"
                SELECT ProfileImagePath 
                FROM (
                    SELECT ProfileImagePath FROM STEMGrade11FirstSemester WHERE Email = @Email AND AccountPassword = @Password
                    UNION ALL
                    SELECT ProfileImagePath FROM STEMGrade11SecondSemester WHERE Email = @Email AND AccountPassword = @Password
                    UNION ALL
                    SELECT ProfileImagePath FROM STEMGrade12FirstSemester WHERE Email = @Email AND AccountPassword = @Password
                    UNION ALL
                    SELECT ProfileImagePath FROM STEMGrade12SecondSemester WHERE Email = @Email AND AccountPassword = @Password
                    UNION ALL
                    SELECT ProfileImagePath FROM SuperAdminRegistrations WHERE EmailAddress = @Email AND Password = @Password
                ) AS Users WHERE ProfileImagePath IS NOT NULL";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            profileImageUrl = result.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error retrieving profile image URL: " + ex.Message);
                    }
                }
            }

            return profileImageUrl;
        }
    }
}
