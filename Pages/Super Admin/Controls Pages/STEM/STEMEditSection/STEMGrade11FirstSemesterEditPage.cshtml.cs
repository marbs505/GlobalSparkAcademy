using GlobalSparkAcademy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace GlobalSparkAcademy.Pages.Super_Admin.Controls_Pages.STEM.STEMEditSection
{
    public class STEMGrade11FirstSemesterEditPageModel : PageModel
    {
        private readonly string connectionString = @"Data Source=MARVIN\SQLEXPRESS01;Initial Catalog=GSA;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";

        [BindProperty]
        public string FirstName { get; set; }

        [BindProperty]
        public string LastName { get; set; }

        [BindProperty]
        public string MiddleName { get; set; }

        [BindProperty]
        public string SuffixName { get; set; }

        [BindProperty]
        public string Gender { get; set; }

        [BindProperty]
        public string Status { get; set; }

        [BindProperty]
        public string Citizenship { get; set; }

        [BindProperty]
        public DateTime DateOfBirth { get; set; }

        [BindProperty]
        public string Birthplace { get; set; }

        [BindProperty]
        public string StreetNumber { get; set; }

        [BindProperty]
        public string Street { get; set; }

        [BindProperty]
        public string City { get; set; }

        [BindProperty]
        public string Province { get; set; }

        [BindProperty]
        public string ZipCode { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string MobileNumber { get; set; }

        [BindProperty]
        public string SelectedYearLevel { get; set; }

        [BindProperty]
        public string SelectedSemester { get; set; }

        [BindProperty]
        public IFormFile BirthCertificate { get; set; }

        [BindProperty]
        public IFormFile ReportCard { get; set; }

        [BindProperty]
        public IFormFile GoodMoralCharacter { get; set; }

        [BindProperty]
        public IFormFile IDPicture { get; set; }

        [BindProperty]
        public IFormFile Form137 { get; set; }

        [BindProperty]
        public IFormFile Form138 { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string tableName = GetTableName();
            if (string.IsNullOrEmpty(tableName))
            {
                ModelState.AddModelError(string.Empty, "Invalid year level or semester selection.");
                return Page();
            }

            string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            Directory.CreateDirectory(uploadPath);

            string birthCertificatePath = await SaveFileAsync(BirthCertificate, uploadPath);
            string reportCardPath = await SaveFileAsync(ReportCard, uploadPath);
            string goodMoralPath = await SaveFileAsync(GoodMoralCharacter, uploadPath);
            string idPicturePath = await SaveFileAsync(IDPicture, uploadPath);
            string form137Path = await SaveFileAsync(Form137, uploadPath);
            string form138Path = await SaveFileAsync(Form138, uploadPath);

            if (await IsDuplicateDataAsync())
            {
                await UpdateEnrollmentDataAsync(birthCertificatePath, reportCardPath, goodMoralPath, idPicturePath, form137Path, form138Path);
            }
            else
            {
                await InsertEnrollmentDataAsync(birthCertificatePath, reportCardPath, goodMoralPath, idPicturePath, form137Path, form138Path);
            }

            return RedirectToPage("/Payment Pages/STEMPaymentPage", new { yearLevel = SelectedYearLevel, semester = SelectedSemester });
        }

        private string GetTableName()
        {
            return SelectedYearLevel switch
            {
                "Grade11" when SelectedSemester == "FirstSemester" => "STEMTable",
                // Add other conditions as needed
                _ => null,
            };
        }

        private async Task<string> SaveFileAsync(IFormFile file, string uploadPath)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            string fileName = $"{Guid.NewGuid()}_{file.FileName}";
            string filePath = Path.Combine(uploadPath, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/uploads/{fileName}";
        }

        private async Task<bool> IsDuplicateDataAsync()
        {
            string tableName = GetTableName();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = $"SELECT COUNT(1) FROM {tableName} WHERE FirstName = @FirstName AND LastName = @LastName AND SelectedYearLevel = @SelectedYearLevel AND SelectedSemester = @SelectedSemester;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@SelectedYearLevel", SelectedYearLevel);
                    command.Parameters.AddWithValue("@SelectedSemester", SelectedSemester);

                    int count = (int)await command.ExecuteScalarAsync();
                    return count > 0;
                }
            }
        }

        private async Task InsertEnrollmentDataAsync(
            string birthCertificatePath, string reportCardPath, string goodMoralPath,
            string idPicturePath, string form137Path, string form138Path)
        {
            string tableName = GetTableName();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = $@"
                    INSERT INTO {tableName} (FirstName, LastName, MiddleName, SuffixName, Gender, Status, Citizenship,
                        DateOfBirth, Birthplace, StreetNumber, Street, City, Province, ZipCode, Email, MobileNumber,
                        BirthCertificatePath, ReportCardPath, GoodMoralCharacterPath, IDPicturePath, Form137Path,
                        Form138Path, SelectedYearLevel, SelectedSemester)
                    VALUES (@FirstName, @LastName, @MiddleName, @SuffixName, @Gender, @Status, @Citizenship,
                        @DateOfBirth, @Birthplace, @StreetNumber, @Street, @City, @Province, @ZipCode, @Email,
                        @MobileNumber, @BirthCertificatePath, @ReportCardPath, @GoodMoralCharacterPath,
                        @IDPicturePath, @Form137Path, @Form138Path, @SelectedYearLevel, @SelectedSemester);";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    AddParameters(command, birthCertificatePath, reportCardPath, goodMoralPath, idPicturePath, form137Path, form138Path);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        private async Task UpdateEnrollmentDataAsync(
            string birthCertificatePath, string reportCardPath, string goodMoralPath,
            string idPicturePath, string form137Path, string form138Path)
        {
            string tableName = GetTableName();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = $@"
                    UPDATE {tableName}
                    SET MiddleName = @MiddleName, SuffixName = @SuffixName, Gender = @Gender, Status = @Status,
                        Citizenship = @Citizenship, DateOfBirth = @DateOfBirth, Birthplace = @Birthplace,
                        StreetNumber = @StreetNumber, Street = @Street, City = @City, Province = @Province,
                        ZipCode = @ZipCode, Email = @Email, MobileNumber = @MobileNumber,
                        BirthCertificatePath = @BirthCertificatePath, ReportCardPath = @ReportCardPath,
                        GoodMoralCharacterPath = @GoodMoralCharacterPath, IDPicturePath = @IDPicturePath,
                        Form137Path = @Form137Path, Form138Path = @Form138Path
                    WHERE FirstName = @FirstName AND LastName = @LastName AND SelectedYearLevel = @SelectedYearLevel
                        AND SelectedSemester = @SelectedSemester;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    AddParameters(command, birthCertificatePath, reportCardPath, goodMoralPath, idPicturePath, form137Path, form138Path);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        private void AddParameters(SqlCommand command, string birthCertificatePath, string reportCardPath, string goodMoralPath,
            string idPicturePath, string form137Path, string form138Path)
        {
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@MiddleName", MiddleName ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@SuffixName", SuffixName ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Gender", Gender);
            command.Parameters.AddWithValue("@Status", Status);
            command.Parameters.AddWithValue("@Citizenship", Citizenship);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Birthplace", Birthplace);
            command.Parameters.AddWithValue("@StreetNumber", StreetNumber ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Street", Street);
            command.Parameters.AddWithValue("@City", City);
            command.Parameters.AddWithValue("@Province", Province);
            command.Parameters.AddWithValue("@ZipCode", ZipCode);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@MobileNumber", MobileNumber);
            command.Parameters.AddWithValue("@BirthCertificatePath", birthCertificatePath ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@ReportCardPath", reportCardPath ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@GoodMoralCharacterPath", goodMoralPath ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@IDPicturePath", idPicturePath ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Form137Path", form137Path ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Form138Path", form138Path ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@SelectedYearLevel", SelectedYearLevel);
            command.Parameters.AddWithValue("@SelectedSemester", SelectedSemester);
        }
    }
}
