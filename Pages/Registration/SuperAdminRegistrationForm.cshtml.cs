using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.IO;

namespace GlobalSparkAcademy.Pages.Registration
{
    public class SuperAdminRegistrationFormModel : PageModel
    {
        #region -- Models or Properties --
        private readonly string connectionString = @"Data Source=MARVIN\SQLEXPRESS01;Initial Catalog=GSA;Integrated Security=True;Trust Server Certificate=True;";

        public List<string> Genders { get; set; } = new List<string> { "Male", "Female", "Other", "Prefer not to say" };
        public List<string> Statuses { get; set; } = new List<string> { "Single", "Married", "Divorced", "Widowed" };

        [BindProperty]
        public string? FirstName { get; set; }
        [BindProperty]
        public string? LastName { get; set; }
        [BindProperty]
        public string? Gender { get; set; }
        [BindProperty]
        public string? Status { get; set; }
        [BindProperty]
        public IFormFile? Profile { get; set; }
        [BindProperty]
        public string? EmailAddress { get; set; }
        [BindProperty]
        public string? MobileNumber { get; set; }
        [BindProperty]
        public string? Address { get; set; }
        [BindProperty]
        public string? Password { get; set; }

        public string? ErrorMessage { get; set; }
        public string[]? ArrayOfErrorMessages { get; set; }

        public void OnGet() { }
        #endregion

        #region -- Post Data to SQL --
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            if (await IsDuplicateDataAsync())
            {
                ErrorMessage = "Duplicate information found.";
                ArrayOfErrorMessages = new[] { "Please enter unique details.", "Check all inputs and try again." };
                return Page();
            }

            string profileImagePath = string.Empty;
            if (Profile != null)
            {
                var uploadsFolder = Path.Combine("wwwroot", "Super Admin Files");
                Directory.CreateDirectory(uploadsFolder);
                profileImagePath = Path.Combine(uploadsFolder, Profile.FileName);
                using var fileStream = new FileStream(profileImagePath, FileMode.Create);
                await Profile.CopyToAsync(fileStream);
                profileImagePath = "/uploads/" + Profile.FileName;
            }

            using (SqlConnection connection = new(connectionString))
            {
                await connection.OpenAsync();
                string query = @"
                    INSERT INTO SuperAdminRegistrations 
                    (FirstName, LastName, Gender, Status, ProfileImage, EmailAddress, MobileNumber, Address, Password, CreatedAt) 
                    VALUES (@FirstName, @LastName, @Gender, @Status, @ProfileImage, @EmailAddress, @MobileNumber, @Address, @Password, GETDATE())";

                using SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@FirstName", FirstName);
                command.Parameters.AddWithValue("@LastName", LastName);
                command.Parameters.AddWithValue("@Gender", Gender);
                command.Parameters.AddWithValue("@Status", Status);
                command.Parameters.AddWithValue("@ProfileImage", profileImagePath);
                command.Parameters.AddWithValue("@EmailAddress", EmailAddress);
                command.Parameters.AddWithValue("@MobileNumber", MobileNumber);
                command.Parameters.AddWithValue("@Address", Address);
                command.Parameters.AddWithValue("@Password", Password);

                await command.ExecuteNonQueryAsync();
            }

            return RedirectToPage("Login");
        }
        #endregion

        #region -- Duplicate Data Validator --
        private async Task<bool> IsDuplicateDataAsync()
        {
            using SqlConnection connection = new(connectionString);
            await connection.OpenAsync();

            string query = @"
                SELECT COUNT(1) 
                FROM SuperAdminRegistrations
                WHERE FirstName = @FirstName AND LastName = @LastName 
                AND EmailAddress = @EmailAddress AND MobileNumber = @MobileNumber";

            using SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@EmailAddress", EmailAddress);
            command.Parameters.AddWithValue("@MobileNumber", MobileNumber);

            int count = (int)await command.ExecuteScalarAsync();
            return count > 0;
        }
        #endregion
    }
}
