using GlobalSparkAcademy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace GlobalSparkAcademy.Pages.Enrollment_Pages
{
    public class SMAWEnrollmentPageModel : PageModel
    {
        #region -- SQL Connection --
        private static string dataSource = @"MARVIN\SQLEXPRESS01";
        private static string initialCatalog = @"GSA";
        private static string integratedSecurity = @"True";

        private readonly string connectionString = $@"Data Source={dataSource};Initial Catalog={initialCatalog};Integrated Security={integratedSecurity};Trust Server Certificate=True;";
        #endregion

        #region -- Regex Patterns for Email and Philippine Mobile Number --
        public string EmailPattern => @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        public string MobilePattern => @"^(09|\+639)\d{9}$";
        #endregion

        #region -- Lists to Populate Dropdowns --
        public List<string> Genders { get; set; } = new List<string> { "Male", "Female", "Other", "Prefer not to say" };
        public List<string> Statuses { get; set; } = new List<string> { "Single", "Married", "Divorced", "Widowed" };
        public List<string> YearLevels { get; set; } = new List<string> { "Grade 11", "Grade 12" };
        public List<string> Semesters { get; set; } = new List<string> { "First Semester", "Second Semester" };
        #endregion

        #region -- SMAW Enrollment Properties --
        /* Yung BindProperty annotation jan is yan yung kumukuha ng values na galing sa forms or (user inputs) sa HTML Forms
         para ma-process neto yung logic or ma-istore yung values sa Database */

        // Yung Required naman is nandon na sa word mismo, required malagyan ng value, kapag walang value mag babato eto ng error
        [BindProperty, Required]
        public string? FirstName { get; set; }

        [BindProperty]
        public string? MiddleName { get; set; }

        [BindProperty, Required]
        public string? LastName { get; set; }

        [BindProperty]
        public string? SuffixName { get; set; }

        [BindProperty, Required]
        public string? Gender { get; set; }

        [BindProperty, Required]
        public string? Status { get; set; }

        [BindProperty, Required]
        public string? Citizenship { get; set; }

        [BindProperty, Required, RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Date of Birth must be in YYYY-MM-DD format")]
        public string? DateOfBirth { get; set; }

        [BindProperty, Required]
        public string? Birthplace { get; set; }

        [BindProperty]
        public string? StreetNumber { get; set; }

        [BindProperty, Required]
        public string? Street { get; set; }

        [BindProperty]
        public string? Subdivision { get; set; }

        [BindProperty]
        public string? Barangay { get; set; }

        [BindProperty, Required]
        public string? City { get; set; }

        [BindProperty, Required]
        public string? Province { get; set; }

        [BindProperty, Required]
        public string? ZipCode { get; set; }

        [BindProperty, Required, EmailAddress]
        public string? Email { get; set; }

        [BindProperty, Required, RegularExpression(@"^(09|\+639)\d{9}$", ErrorMessage = "Invalid mobile number")]
        public string? MobileNumber { get; set; }

        [BindProperty]
        public string? TelephoneNumber { get; set; }

        [BindProperty]
        public string? GuardianName { get; set; }

        [BindProperty]
        public string? GuardianEmail { get; set; }

        [BindProperty]
        public string? GuardianMobileNumber { get; set; }

        [BindProperty]
        public string? GuardianTelephoneNumber { get; set; }

        [BindProperty]
        public string? GuardianOccupation { get; set; }

        [BindProperty]
        public string? GuardianCompany { get; set; }

        [BindProperty]
        public string? GuardianRelationship { get; set; }

        [BindProperty]
        public IFormFile? BirthCertificate { get; set; }

        [BindProperty]
        public IFormFile? ReportCard { get; set; }

        [BindProperty]
        public IFormFile? GoodMoralCharacter { get; set; }

        [BindProperty]
        public IFormFile? IDPicture { get; set; }

        [BindProperty]
        public IFormFile? Form137 { get; set; }

        [BindProperty]
        public IFormFile? Form138 { get; set; }

        [BindProperty]
        public string? AccountFirstName { get; set; }

        [BindProperty]
        public string? AccountLastName { get; set; }

        [BindProperty]
        public string? AccountEmail { get; set; }

        [BindProperty]
        public string? AccountMobile { get; set; }

        [BindProperty]
        public string? AccountPassword { get; set; }

        [BindProperty]
        public string? SelectedYearLevel { get; set; }

        [BindProperty]
        public string? SelectedSemester { get; set; }

        public string? ErrorMessage { get; set; }

        public string[]? ArrayOfErrorMessages { get; set; }

        // accessor and mutator (setters and getters) para ma pass naten yung chosen year level and semester sa payment page.
        public YearAndTermConnection? YearAndTerm { get; private set; }

        #endregion

        #region -- Methods --
        public void OnGet()
        {
            YearAndTerm = new YearAndTermConnection(SelectedYearLevel, SelectedSemester);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            YearAndTerm = new YearAndTermConnection(SelectedYearLevel, SelectedSemester);

            if (await IsDuplicateDataAsync())
            {
                ErrorMessage = "Enrollment data with matching details (First Name, Last Name, Email, and Mobile Number) already exists for the account with Account First Name, Account Last Name, Account Email, and Account Mobile Number.";

                ArrayOfErrorMessages = new string[]
                {
                    "Duplicate information is strictly prohibited in our school's enrollment system. Please ensure all details entered are unique and accurately represent the applicant's identity and records to maintain our data integrity and streamline processing.",

                    "If you believe an error has occurred, please double-check the information and make any necessary adjustments. Our system cross-references key fields such as name, email, and contact details to prevent duplicate entries. Thank you for your cooperation in ensuring accurate submissions."
                };
                return Page();
            }

            string birthCertificatePath = SaveFile(BirthCertificate, "BirthCertificate");
            string reportCardPath = SaveFile(ReportCard, "ReportCard");
            string goodMoralPath = SaveFile(GoodMoralCharacter, "GoodMoralCharacter");
            string idPicturePath = SaveFile(IDPicture, "IDPicture");
            string form137Path = SaveFile(Form137, "Form137");
            string form138Path = SaveFile(Form138, "Form138");

            await InsertEnrollmentDataAsync(birthCertificatePath, reportCardPath, goodMoralPath, idPicturePath, form137Path, form138Path);

            return RedirectToPage("/Payment Pages/SMAWPaymentPage", new { yearLevel = YearAndTerm.YearLevel, semester = YearAndTerm.Semester });
        }

        private async Task<bool> IsDuplicateDataAsync()
        {
            string tableName = GetTableName();
            if (string.IsNullOrEmpty(tableName)) return false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string query = $@"
                    SELECT COUNT(1) 
                    FROM {tableName} 
                    WHERE FirstName = @FirstName AND LastName = @LastName 
                          AND Email = @Email AND MobileNumber = @MobileNumber 
                          AND AccountFirstName = @AccountFirstName AND AccountLastName = @AccountLastName 
                          AND AccountEmail = @AccountEmail AND AccountMobile = @AccountMobile;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@MobileNumber", MobileNumber);
                    command.Parameters.AddWithValue("@AccountFirstName", AccountFirstName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@AccountLastName", AccountLastName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@AccountEmail", AccountEmail ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@AccountMobile", AccountMobile ?? (object)DBNull.Value);

                    int count = (int)await command.ExecuteScalarAsync();
                    return count > 0;
                }
            }
        }
        #endregion

        #region -- File Handling and Database Operations --
        private string SaveFile(IFormFile file, string folderName)
        {
            if (file == null) return null;

            string uploadsFolder = Path.Combine("wwwroot", "SMAW Files", folderName);
            Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return Path.Combine("uploads", folderName, uniqueFileName).Replace("\\", "/");
        }

        private async Task InsertEnrollmentDataAsync(
    string birthCertificatePath, string reportCardPath, string goodMoralPath,
    string idPicturePath, string form137Path, string form138Path)
        {
            string tableName = GetTableName();

            if (string.IsNullOrEmpty(tableName))
            {
                throw new InvalidOperationException("Invalid year level or semester selection.");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = $@"
                    INSERT INTO {tableName} (FirstName, MiddleName, LastName, SuffixName, Gender, Status, Citizenship, DateOfBirth, Birthplace, 
                        StreetNumber, Street, City, Province, ZipCode, Email, MobileNumber, BirthCertificatePath, ReportCardPath, 
                        GoodMoralCharacterPath, IDPicturePath, Form137Path, Form138Path, AccountFirstName, AccountLastName, 
                        AccountEmail, AccountMobile, AccountPassword, GuardianName, GuardianEmail, GuardianMobileNumber, 
                        GuardianTelephoneNumber, GuardianOccupation, GuardianCompany, GuardianRelationship)
                    VALUES (@FirstName, @MiddleName, @LastName, @SuffixName, @Gender, @Status, @Citizenship, @DateOfBirth, @Birthplace, 
                        @StreetNumber, @Street, @City, @Province, @ZipCode, @Email, @MobileNumber, @BirthCertificatePath, @ReportCardPath, 
                        @GoodMoralCharacterPath, @IDPicturePath, @Form137Path, @Form138Path, @AccountFirstName, @AccountLastName, 
                        @AccountEmail, @AccountMobile, @AccountPassword, @GuardianName, @GuardianEmail, @GuardianMobileNumber, 
                        @GuardianTelephoneNumber, @GuardianOccupation, @GuardianCompany, @GuardianRelationship);";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@MiddleName", MiddleName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@LastName", LastName);
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
                    command.Parameters.AddWithValue("@AccountFirstName", AccountFirstName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@AccountLastName", AccountLastName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@AccountEmail", AccountEmail ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@AccountMobile", AccountMobile ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@AccountPassword", AccountPassword ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@GuardianName", GuardianName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@GuardianEmail", GuardianEmail ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@GuardianMobileNumber", GuardianMobileNumber ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@GuardianTelephoneNumber", GuardianTelephoneNumber ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@GuardianOccupation", GuardianOccupation ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@GuardianCompany", GuardianCompany ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@GuardianRelationship", GuardianRelationship ?? (object)DBNull.Value);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        private string GetTableName()
        {
            string tableName = null;

            switch (SelectedYearLevel)
            {
                case "Grade 11":
                    switch (SelectedSemester)
                    {
                        case "First Semester":
                            tableName = "SMAWGrade11FirstSemester";
                            break;
                        case "Second Semester":
                            tableName = "SMAWGrade11SecondSemester";
                            break;
                    }
                    break;

                case "Grade 12":
                    switch (SelectedSemester)
                    {
                        case "First Semester":
                            tableName = "SMAWGrade12FirstSemester";
                            break;
                        case "Second Semester":
                            tableName = "SMAWGrade12SecondSemester";
                            break;
                    }
                    break;

                default:
                    tableName = null;
                    break;
            }

            return tableName;
        }
        #endregion
    }
}
