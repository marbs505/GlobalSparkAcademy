# Global Spark Academy

## Overview
**Global Spark Academy** is a web-based enrollment management system designed to streamline the enrollment process for educational institutions. Built with modern technologies, this system offers features for managing student enrollments, tracking records, and generating reports efficiently.

---

## Key Features
- **Student Enrollment Management**: Simplified registration and tracking of student data across various strands.
- **Database Integration**: Seamless data handling with SQL Server.
- **Financial Dashboard**: Displays financial statistics and student counts per strand.
- **File Storage**: Efficient file path storage mechanism for managing uploaded documents.
- **Cross-Platform Deployment**: Supports containerized deployment with Docker.

---

## Technologies Used

### Backend:
- **ASP.NET Core**: A modern, open-source framework for building robust web applications.
- **ADO.NET**: Handles data access to SQL Server with optimized performance.

### Database:
- **SQL Server**: A reliable and scalable database solution for storing enrollment data.

### Deployment:
- **Docker**: Containerized deployment to ensure consistency and portability.

---

## Installation and Setup

### Prerequisites
1. [.NET SDK](https://dotnet.microsoft.com/download) (ASP.NET Core 7.0 or later recommended)
2. [SQL Server](https://www.microsoft.com/en-us/sql-server/) (Local or cloud-based)
3. [Docker](https://www.docker.com/products/docker-desktop)

### Steps
1. **Clone the Repository**:
   ```bash
   git clone https://github.com/YourUsername/GlobalSparkAcademy.git
   cd GlobalSparkAcademy
   ```

2. **Configure the Database**:
   - Update the connection string in `appsettings.json` to point to your SQL Server instance.
   ```json
   "ConnectionStrings": {
       "DefaultConnection": "Server=your_server;Database=GlobalSparkAcademyDB;User Id=your_user;Password=your_password;"
   }
   ```
   - Run the provided SQL scripts to initialize the database schema.

3. **Build and Run the Application**:
   - Using .NET CLI:
     ```bash
     dotnet build
     dotnet run
     ```

4. **Deploy with Docker**:
   - Build the Docker image:
     ```bash
     docker build -t globalsparkacademy .
     ```
   - Run the Docker container:
     ```bash
     docker run -d -p 5000:80 globalsparkacademy
     ```

5. **Access the Application**:
   Open your web browser and navigate to `http://localhost:5000`.

---

## Contributing
We welcome contributions! To contribute:
1. Fork the repository.
2. Create a feature branch.
3. Commit your changes and push them to the branch.
4. Open a pull request for review.

---

## License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

## Contact
For inquiries, reach out to **Global Spark Academy Team** at support@globalsparkacademy.com.

