using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Global_Spark_Academy.Pages.Enrollment_Pages
{
    public class SelectStrandModel : PageModel
    {
        public List<Strand>? Strands { get; set; }

        public void OnGet()
        {
            Strands = new List<Strand>
            {
                new Strand
                {
                    Title = "Science Technology Engineering & Mathematics",
                    Description = "Dive into the world of STEM at Golden Spark Academy, where we inspire innovation and critical thinking. Our comprehensive curriculum combines rigorous academic standards with hands-on experiences, preparing students for a future in science and technology. With access to cutting-edge labs and mentorship from industry experts, you'll develop the skills necessary to tackle real-world challenges and excel in a rapidly evolving landscape.",
                },
                new Strand
                {
                    Title = "Accountancy, Business & Management",
                    Description = "Step into the dynamic realm of business with our ABM strand. At Golden Spark Academy, we equip students with a robust understanding of accounting principles, management strategies, and entrepreneurial skills. Our curriculum emphasizes practical applications, allowing you to engage in real-world projects and internships. With a focus on leadership and decision-making, you will be well-prepared for a successful career in the fast-paced business environment.",
                },
                new Strand
                {
                    Title = "Humanities and Social Sciences",
                    Description = "Embark on a journey of exploration and understanding with our HUMSS strand. Here at Golden Spark Academy, you will analyze the complexities of human behavior, society, and culture. Our interdisciplinary approach combines literature, history, psychology, and sociology, fostering critical thinking and communication skills. With opportunities for community engagement and research projects, you will develop a well-rounded perspective that prepares you for diverse careers in social sciences, education, and beyond.",
                },
                new Strand
                {
                    Title = "Shielded Metal Arc Welding",
                    Description = "The SMAW (Shielded Metal Arc Welding) strand is a technical program focused on equipping students with essential skills in manual welding techniques using electrodes to join metal components. This strand emphasizes practical training, safety protocols, and an understanding of welding processes, preparing students for careers in various industries such as construction and manufacturing.",
                }
            };
        }
    }

    public class Strand
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}
