using System.Collections.Generic;

namespace StudentMarksManagementSystem.Models
{
    public class MarksViewModel
    {
        public Module Module { get; set; } = new Module();

        public List<Assessment> Assessments { get; set; } = new List<Assessment>
        {
          new Assessment(),
          new Assessment(),
          new Assessment(),
          new Assessment(),
          new Assessment(),
          new Assessment()
        };

        public decimal? ExamMark { get; set; }

        public MarksResult? Result { get; set; }
    }
}
