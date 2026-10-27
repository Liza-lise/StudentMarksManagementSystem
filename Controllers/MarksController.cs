using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using StudentMarksManagementSystem.Models;

namespace StudentMarksManagementSystem.Controllers
{
    public class MarksController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            MarksViewModel model = new MarksViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Calculate(MarksViewModel model)
        {
            // Ensure the assessment list exists.
            if (model.Assessments == null)
            {
                model.Assessments = new List<Assessment>();
                ModelState.AddModelError(
                    "",
                    "Enter at least one assessment.");

                return View("Index", model);
            }

            // Ignore empty assessment rows.
            model.Assessments = model.Assessments
                .Where(a => a != null &&
                            !string.IsNullOrWhiteSpace(a.Name))
                .ToList();

            // At least one assessment is required.
            if (model.Assessments.Count == 0)
            {
                ModelState.AddModelError(
                    "",
                    "Enter at least one assessment.");
            }

            // Validate assessment marks and weights.
            decimal totalWeight = 0;

            foreach (Assessment assessment in model.Assessments)
            {
                if (assessment.Mark < 0 || assessment.Mark > 100)
                {
                    ModelState.AddModelError(
                        "",
                        $"The mark for {assessment.Name} must be between 0 and 100.");
                }

                if (assessment.Weight <= 0 || assessment.Weight > 100)
                {
                    ModelState.AddModelError(
                        "",
                        $"The weight for {assessment.Name} must be greater than 0 and no more than 100.");
                }

                totalWeight += assessment.Weight;
            }

            // Assessment weights must total exactly 100%.
            if (totalWeight != 100)
            {
                ModelState.AddModelError(
                    "",
                    $"Assessment weights must total 100%. Your total is {totalWeight}%.");
            }

            // Validate the exam mark only if one was entered
            // for an examinable module.
            if (model.Module.Type == ModuleType.Examinable &&
                model.ExamMark.HasValue &&
                (model.ExamMark.Value < 0 ||
                 model.ExamMark.Value > 100))
            {
                ModelState.AddModelError(
                    "ExamMark",
                    "The exam mark must be between 0 and 100.");
            }

            // Return to the form if validation fails.
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            // Calculate the DP.
            MarksCalculator calculator = new MarksCalculator();

            decimal dp = calculator.CalculateDP(model.Assessments);

            // Calculate the result according to the module type.
            if (model.Module.Type == ModuleType.Examinable)
            {
                // Calculate() handles all three cases:
                // not qualified, qualified without an exam mark,
                // and a completed final result.
                model.Result = calculator.Calculate(
                    model.Module,
                    dp,
                    model.ExamMark);
            }
            else
            {
                // Non-examinable modules do not require an exam mark.
                model.Result = calculator.Calculate(
                    model.Module,
                    dp,
                    null);
            }

            return View("Index", model);
        }
    }
}

