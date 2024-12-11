CREATE TABLE SuperAdminRegistrations (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100),
    LastName NVARCHAR(100),
    Gender NVARCHAR(50),
    Status NVARCHAR(50),
    ProfileImage NVARCHAR(255),
    EmailAddress NVARCHAR(100) UNIQUE,
    MobileNumber NVARCHAR(20) UNIQUE,
    Address NVARCHAR(255),
    Password NVARCHAR(255),
    CreatedAt DATETIME
);
