using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WGUMobilePortal.Models;

namespace WGUMobilePortal.Services
{
    public static class DummyData
    {
        public static async Task Main()
        {
            await DBService.ResetAll(); // Delete all existing items

            // Add Terms
            var term1 = await DBService.GetTerm(await DBService.AddTerm("Current Term 1", DateTime.Today, DateTime.Today.AddMonths(6)));
            var term2 = await DBService.GetTerm(await DBService.AddTerm("Future Term 1", DateTime.Today.AddMonths(6), DateTime.Today.AddMonths(12)));
            var term3 = await DBService.GetTerm(await DBService.AddTerm("Future Term 2", DateTime.Today.AddMonths(12), DateTime.Today.AddMonths(18)));
            List<Term> termList = new List<Term>() { term1, term2, term3 };

            // Add Instructors
            var defaultInstructor = await DBService.AddInstructor("Coby", "Caldwell", "808-690-7792", "ccald15@wgu.edu");
            await DBService.AddInstructor("Sample", "Instructor", "111-222-1234", "SampleInstructor@wgu.edu");

            // Add Courses
            var rand = new Random();
            var courseStatusEnum = Enum.GetValues(typeof(CourseStatus));
            var termCourseList = new List<int>();
            int termcounter = 0;
            for (int i = 0; i < 17; i++)
            {
                var course = new Course()
                {
                    Name = $"Course {i + 1}",
                    StartDate = DateTime.Today.AddMonths(i),
                    EndDate = DateTime.Today.AddMonths(i + 1),
                    Status = (CourseStatus)courseStatusEnum.GetValue(rand.Next(courseStatusEnum.Length)),
                    InstructorId = defaultInstructor,
                    ObjectiveAssessmentId = await DBService.AddAssessment(
                        $"Objective Assessment {i + 1}",
                        DateTime.Today.AddMonths(i + 1),
                        false,
                        AssessmentStyle.Objective),
                    PerformanceAssessmentId = await DBService.AddAssessment(
                        $"Performance Assessment {i + 1}",
                        DateTime.Today.AddMonths(i + 1),
                        false,
                        AssessmentStyle.Performance),
                    NoteId = await DBService.AddNote($"Notes for Course: {i + 1}")
                };
                var created = await DBService.AddCourse(course);

                if (termCourseList.Count == 6)
                {
                    termList[termcounter].CourseId = termCourseList;
                    await DBService.EditTerm(termList[termcounter]);

                    termCourseList.Clear();
                    termcounter++;
                }
                else if (termCourseList.Count < 6)
                {
                    termCourseList.Add(created);
                }
            }

            // Add Some more assessments for selection list
            var assessmentStyleEnum = Enum.GetValues(typeof(AssessmentStyle));
            for (int i = 0; i < 6; i++)
            {
                var style = (AssessmentStyle)assessmentStyleEnum.GetValue(rand.Next(assessmentStyleEnum.Length));
                var assessment = new Assessment()
                {
                    Name = $"Random {style} Assessment {i + 1}",
                    DueDate = DateTime.Today.AddMonths(i),
                    Style = style
                };

                await DBService.AddAssessment(assessment);
            }
        }
    }
}