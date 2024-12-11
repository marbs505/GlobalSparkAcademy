using Microsoft.Data.SqlClient;

namespace GlobalSparkAcademy.Connections
{
    public class SuperAdminLoginConnection
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public string? Status { get; set; }
        public string? ProfileImage { get; set; }
        public string? EmailAddress { get; set; }
        public string? MobileNumber { get; set; }
        public string? Address { get; set; }

        private readonly string connectionString = @"Data Source=MARVIN\SQLEXPRESS01;Initial Catalog=GSA;Integrated Security=True;Trust Server Certificate=True;";

        public SuperAdminLoginConnection(string email, string password)
        {
            string query = @"
                SELECT FirstName, LastName, Gender, Status, ProfileImagePath, EmailAddress, MobileNumber, Address
                FROM SuperAdminRegistrations
                WHERE EmailAddress = @Email AND Password = @Password";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                FirstName = reader["FirstName"]?.ToString();
                                LastName = reader["LastName"]?.ToString();
                                Gender = reader["Gender"]?.ToString();
                                Status = reader["Status"]?.ToString();
                                ProfileImage = reader["ProfileImagePath"]?.ToString();
                                EmailAddress = reader["EmailAddress"]?.ToString();
                                MobileNumber = reader["MobileNumber"]?.ToString();
                                Address = reader["Address"]?.ToString();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error fetching SuperAdmin data: {ex.Message}");
                    }
                }
            }
        }
    }
}
