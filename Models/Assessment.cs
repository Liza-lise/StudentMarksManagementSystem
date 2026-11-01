namespace StudentMarksManagementSystem.Models
{
    public class Assessment
    {
        public string Name { get; set; } = string.Empty;

        public decimal Mark { get; set; }

        public decimal Weight { get; set; }
    }
}