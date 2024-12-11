namespace GlobalSparkAcademy.Services
{
    public class YearAndTermConnection
    {
        public string YearLevel { get; set; }
        public string Semester { get; set; }

        public YearAndTermConnection(string yearLevel, string semester)
        {
            this.YearLevel = yearLevel;
            this.Semester = semester;
        }
    }
}
