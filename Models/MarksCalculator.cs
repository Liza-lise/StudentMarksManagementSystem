
using System;
using System.Collections.Generic;

namespace StudentMarksManagementSystem.Models
{
    public class MarksCalculator
    {
        public decimal CalculateDP(List<Assessment> assessments)
        {
            decimal dp = 0;

            foreach (Assessment assessment in assessments)
            {
                dp += assessment.Mark * assessment.Weight / 100;
            }

            return dp;
        }

        public MarksResult Calculate(
            Module module,
            decimal dp,
            decimal? examMark)
        {
            MarksResult result = new MarksResult
            {
                DP = dp,
                ExamMark = examMark
            };

            // Rule 1: Non-examinable modules use DP as the final mark.
            if (module.Type == ModuleType.NonExaminable)
            {
                result.FinalMark = dp;

                if (dp >= 75)
                {
                    result.Status = "Distinction";
                }
                else if (dp >= 50)
                {
                    result.Status = "Pass";
                }
                else
                {
                    result.Status = "Fail";
                }

                return result;
            }

            // Rule 2: DP below 40 means the student is not qualified.
            if (dp < 40)
            {
                result.FinalMark = null;
                result.Status = "NotQualified";
                return result;
            }

            // Rule 3: Calculate the exam mark needed for each target.
            decimal passTarget = (50 - (dp * 0.4m)) / 0.6m;
            decimal distinctionTarget = (75 - (dp * 0.4m)) / 0.6m;

            result.ExamMarkNeededForPass =
                Math.Max(0, Math.Ceiling(passTarget));

            result.ExamMarkNeededForDistinction =
                distinctionTarget > 100
                    ? null
                    : Math.Max(0, Math.Ceiling(distinctionTarget));

            // Rule 4: Before the exam, show qualification and targets only.
            if (examMark == null)
            {
                result.FinalMark = null;
                result.Status = "Qualified";
                return result;
            }

            // Rule 5: Calculate the final mark: 40% DP + 60% exam.
            result.FinalMark = (dp * 0.4m) + (examMark.Value * 0.6m);

            // Rule 6: Determine the final result.
            if (result.FinalMark >= 75)
            {
                result.Status = "Distinction";
            }
            else if (result.FinalMark >= 50)
            {
                result.Status = "Pass";
            }
            else
            {
                result.Status = "Fail";
            }

            return result;
        }
    }
}
