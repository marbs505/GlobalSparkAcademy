using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GlobalSparkAcademy.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        #region -- Index Page Contents Instances --
        /* Para hindi na tayo mag ddisplay ng mahabang text sa HTML dito nalang naten ia-access */
        public string[] Titles { get; private set; }
        public string[] Descriptions { get; private set; }
        public string[] CardsDescriptions { get; private set; }

        [BindProperty]
        public string? EmployeeID { get; set; }

        public bool ShowErrorModal { get; set; }
        public string? ErrorMessage { get; set; }
        #endregion

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;

            #region -- Index Page Contents --
            /* I-store naten sa array yung mga contents para malinis tingnan */
            Titles = new[]
            {
                "Welcome to Global Spark Academy",
                "Streamlined Enrollment Process",
                "User-Centric Application Portal",
                "Insightful Reporting & Analytics",
                "Comprehensive Student Management"
            };

            CardsDescriptions = new[]
            {
                "At Global Spark Academy, we are committed to providing a streamlined enrollment process. Our web-based system enables students and parents to complete applications online, eliminating the stress of paperwork and long wait times. With real-time updates and automated notifications, we ensure a smooth and efficient enrollment journey.",
                "Our enrollment process is designed for efficiency, allowing students and parents to complete applications easily and reducing wait times significantly.",
                "The application portal simplifies the submission of documents, making enrollment straightforward and ensuring a seamless experience for every user.",
                "Leverage our advanced reporting tools to gain insights into enrollment patterns, helping us enhance recruitment strategies and improve school performance.",
                "Manage student information efficiently with our comprehensive system, ensuring that students and staff have access to the resources and support they need."
            };

            Descriptions = new[]
            {
                "Welcome to Global Spark Academy, where we are committed to providing a supportive and innovative educational environment. Our goal is to empower students to achieve their fullest potential through personalized learning and a focus on academic success.",
                "Our streamlined enrollment process eliminates the stress of paperwork and long queues. With our digital platform, students and parents can submit applications quickly and efficiently, ensuring a hassle-free start to their academic journey.",
                "Our user-centric application portal is designed to make the enrollment process easy. Students can fill out digital application forms and securely upload necessary documents, making the entire experience smooth and stress-free.",
                "Gain valuable insights with our robust reporting and analytics features. Monitor application statuses and track enrollment trends, allowing us to make data-driven decisions that enhance educational outcomes.",
                "Our comprehensive student management system supports effective tracking and communication throughout each student�s journey. By ensuring easy access to vital information, we enhance the overall enrollment experience.",


                "\r\nOur comprehensive student management system is designed to revolutionize the way educational institutions manage their student-related processes, ensuring seamless communication, tracking, and accessibility throughout every stage of a student's academic journey. This state-of-the-art platform streamlines the enrollment process and simplifies data management, enabling schools, colleges, and universities to operate more efficiently while prioritizing the needs of their students.\r\n\r\nOne of the core features of our system is its ability to track every aspect of a student's lifecycle, starting from initial inquiries and applications to enrollment, attendance, academic performance, and graduation. This ensures that educational institutions have a centralized and organized repository of student information that is easy to access and update. Administrators, teachers, and support staff can all benefit from this unified approach, as it reduces redundant manual tasks, minimizes errors, and enhances productivity.\r\n\r\nMoreover, our system fosters effective communication between all stakeholders, including students, parents, faculty, and administrative staff. Automated notifications and alerts ensure that students and parents are kept informed about important deadlines, events, and updates, such as enrollment status, class schedules, exam dates, or tuition payment reminders. This transparency helps to build trust and ensures that no critical information is overlooked.\r\n\r\nBy providing easy access to vital information, our system empowers students and parents to stay informed and engaged in the educational process. With user-friendly interfaces, online portals, and mobile compatibility, users can view and update their information, check academic progress, access learning resources, or communicate with school staff from anywhere at any time. This convenience significantly enhances the overall experience, making it easier for students to focus on their academic goals.\r\n\r\nThe enrollment experience, in particular, is transformed with our platform�s streamlined workflows and intelligent tools. From submitting applications to uploading required documents and completing registrations, the system simplifies every step. Built-in validation checks ensure that all necessary information is provided, reducing processing delays and administrative burdens. Additionally, integration with financial systems allows institutions to manage tuition payments and scholarships efficiently, giving students and parents a hassle-free experience.\r\n\r\nData security and privacy are also key priorities in our student management system. We leverage advanced encryption and secure protocols to protect sensitive information, ensuring compliance with data protection regulations and safeguarding user trust. Role-based access controls allow institutions to manage who can view or edit specific data, maintaining confidentiality while promoting accountability.\r\n\r\nThe system also supports detailed reporting and analytics, providing administrators with insights into key performance indicators and trends. Whether tracking enrollment rates, analyzing attendance patterns, or identifying students who may need additional support, these tools empower decision-makers to act proactively. By leveraging data-driven insights, institutions can refine their processes and improve outcomes for students and staff alike.\r\n\r\nFinally, our system is highly customizable and scalable, making it suitable for institutions of all sizes and types. Whether you�re managing a small private school, a large university, or a multi-campus network, the platform adapts to meet your unique needs. Its modular design allows you to choose features that are most relevant to your organization, ensuring a tailored solution that grows with your institution."
            };
            #endregion
        }
    }
}
