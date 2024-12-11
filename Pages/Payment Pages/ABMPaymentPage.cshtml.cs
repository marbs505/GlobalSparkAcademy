using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GlobalSparkAcademy.Services.EmailSender;

namespace GlobalSparkAcademy.Pages.Payment_Pages
{
    public class ABMPaymentPageModel : PageModel
    {
        private readonly EmailService _emailService;

        public ABMPaymentPageModel(EmailService emailService)
        {
            _emailService = emailService;
        }

        private readonly string connectionString = $@"Data Source=MARVIN\SQLEXPRESS01;Initial Catalog=GSA;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";

        #region -- Text contents on Payment Page --
        /* Dito ko nalang nilagay yung mga text sa webpage para malinis tingnan sa HTML */

        public string? StrandName { get; } = "ABM (Accountancy, Business, Management)";

        public int Amount { get; } = 6230;

        public string[]? Descriptions { get; } =
        {
            "Please select a payment method you want to do here, but we recommend choosing Checkout section for straightforward transaction.",

            "Please fill in your GCash payment details accurately. After submitting, you will receive an email confirmation once your payment has been successfully processed. If you encounter any issues or have questions regarding your GCash transaction, don't hesitate to contact our customer support.",

            "Please fill in your PayMaya payment details accurately. After submitting, you will receive an email confirmation once your payment has been successfully processed. If you need any assistance during this process or have questions about your PayMaya transaction, don’t hesitate to contact our customer support.",

            "Please fill in your PayPal payment details accurately. After submitting, you will receive an email confirmation once your payment has been successfully processed. If you encounter any issues or have questions regarding your PayPal transaction, don't hesitate to contact our customer support.",

            "Please fill in your payment details accurately. After submitting, you will receive an email confirmation once your payment has been successfully processed. Should you need any assistance during this process, don't hesitate to contact our customer support."

        };

        public string? EmailRegexPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        #endregion


        [BindProperty]
        public string? FirstName { get; set; }

        [BindProperty]
        public string? LastName { get; set; }

        [BindProperty]
        public string? PayorsName { get; set; }

        [BindProperty]
        public string? Email { get; set; }

        [BindProperty]
        public decimal AmountPaid { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? YearLevel { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Semester { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"INSERT INTO ABMPaymentTable (StrandName, YearLevel, Semester, FirstName, LastName, PayorsName, Email, AmountPaid) 
                             VALUES (@StrandName, @YearLevel, @Semester, @FirstName, @LastName, @PayorsName, @Email, @AmountPaid)";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@StrandName", StrandName);
                    command.Parameters.AddWithValue("@YearLevel", YearLevel ?? string.Empty);
                    command.Parameters.AddWithValue("@Semester", Semester ?? string.Empty);
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@PayorsName", PayorsName);
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@AmountPaid", AmountPaid);

                    await command.ExecuteNonQueryAsync();
                }

                var fullName = $"{FirstName} {LastName}";
                await _emailService.SendEnrollmentConfirmationEmail(Email, fullName, StrandName, YearLevel, Semester);

                return RedirectToPage("/Payment Pages/AcknowledgementPage");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting data: {ex.Message}");
                return Page();
            }
        }
    }
}
