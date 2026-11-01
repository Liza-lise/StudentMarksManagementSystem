
namespace StudentMarksManagementSystem.Models
{
    public class MarksResult
    {
        public decimal DP { get; set; }

        public decimal? ExamMark { get; set; }

        public decimal? FinalMark { get; set; }

        public string Status { get; set; } = string.Empty;

        // Minimum exam mark needed for a pass.
        public decimal? ExamMarkNeededForPass { get; set; }

        // Minimum exam mark needed for a distinction.
        public decimal? ExamMarkNeededForDistinction { get; set; }
    }
}
