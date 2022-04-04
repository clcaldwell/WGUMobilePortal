using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WGUMobilePortal.Models;

namespace WGUMobilePortal.Services
{
    public class DummyData
    {
        public static async Task Main()
        {
            await DBService.ResetAll(); // Delete all existing items

            // Add Terms
            var term1 = await DBService.GetTerm(await DBService.AddTerm("Current Term 1", DateTime.Today, DateTime.Today.AddMonths(6)));
            var term2 = await DBService.GetTerm(await DBService.AddTerm("Future Term 1", DateTime.Today.AddMonths(6), DateTime.Today.AddMonths(12)));
            var term3 = await DBService.GetTerm(await DBService.AddTerm("Future Term 2", DateTime.Today.AddMonths(12), DateTime.Today.AddMonths(18)));
            List<Term> termList = new List<Term>() { term1, term2, term3 };

            // Add Instructor
            var instructor1 = await DBService.AddInstructor("Harper", "Setton", "111-222-1234", "test@gmail.com");
            var instructor2 = await DBService.AddInstructor("Test", "Test_2", "111-222-1234", "test@gmail.com");
            var instructor3 = await DBService.AddInstructor("Coby", "Caldwell", "111-222-1234", "test@gmail.com");
            List<int> instructors = new List<int>() { instructor1, instructor2, instructor3 };

            // Add Courses

            var rand = new Random();
            var courseStatusEnum = Enum.GetValues(typeof(CourseStatus));
            var termCourseList = new List<int>();
            int termcounter = 0;
            for (int i = 0; i < 17; i++)
            {
                var course = new Course()
                {
                    Name = $"Course {i}",
                    StartDate = DateTime.Today.AddMonths(i),
                    EndDate = DateTime.Today.AddMonths(i + 1),
                    Status = (CourseStatus)courseStatusEnum.GetValue(rand.Next(courseStatusEnum.Length)),
                    InstructorId = instructors[rand.Next(instructors.Count)],
                    ObjectiveAssessmentId = await DBService.AddAssessment(
                        $"Objective Assessment {i}",
                        DateTime.Today.AddMonths(i),
                        DateTime.Today.AddMonths(i + 1),
                        AssessmentStyle.Objective),
                    PerformanceAssessmentId = await DBService.AddAssessment(
                        $"Performance Assessment {i}",
                        DateTime.Today.AddMonths(i),
                        DateTime.Today.AddMonths(i + 1),
                        AssessmentStyle.Performance),
                    NoteId = await DBService.AddNote($"This is a test of notes, for Course: {i}")
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
                var assessment = new Assessment()
                {
                    Name = $"Random Assessment {i}",
                    StartDate = DateTime.Today.AddMonths(i),
                    EndDate = DateTime.Today.AddMonths(i + 1),
                    Style = (AssessmentStyle)assessmentStyleEnum.GetValue(rand.Next(assessmentStyleEnum.Length))
                };

                await DBService.AddAssessment(assessment);
            }

            //var course11 = await DBService.GetCourse(await DBService.AddCourse("Course 1", DateTime.Today.AddMonths(0), DateTime.Today.AddMonths(1), CourseStatus.Started, instructors[rand.Next(instructors.Count)]));
            //var course21 = await DBService.GetCourse(await DBService.AddCourse("Course 2", DateTime.Today.AddMonths(1), DateTime.Today.AddMonths(2), CourseStatus.Planned, instructors[rand.Next(instructors.Count)]));
            //var course31 = await DBService.GetCourse(await DBService.AddCourse("Course 3", DateTime.Today.AddMonths(2), DateTime.Today.AddMonths(3), CourseStatus.Started, instructors[rand.Next(instructors.Count)]));
            //var course41 = await DBService.GetCourse(await DBService.AddCourse("Course 4", DateTime.Today.AddMonths(3), DateTime.Today.AddMonths(4), CourseStatus.Planned, instructors[rand.Next(instructors.Count)]));
            //var course51 = await DBService.GetCourse(await DBService.AddCourse("Course 5", DateTime.Today.AddMonths(4), DateTime.Today.AddMonths(5), CourseStatus.Started, instructors[rand.Next(instructors.Count)]));
            //var course61 = await DBService.GetCourse(await DBService.AddCourse("Course 6", DateTime.Today.AddMonths(5), DateTime.Today.AddMonths(6), CourseStatus.Planned, instructors[rand.Next(instructors.Count)]));
            //term1.CourseId = new List<int>() { course11.Id, course21.Id, course31.Id, course41.Id, course51.Id, course61.Id };
            //await DBService.EditTerm(term1);

            //var course12 = await DBService.GetCourse(await DBService.AddCourse("Course 7", DateTime.Today.AddMonths(6), DateTime.Today.AddMonths(7), CourseStatus.Started, instructors[rand.Next(instructors.Count)]));
            //var course22 = await DBService.GetCourse(await DBService.AddCourse("Course 8", DateTime.Today.AddMonths(7), DateTime.Today.AddMonths(8), CourseStatus.Planned, instructors[rand.Next(instructors.Count)]));
            //var course32 = await DBService.GetCourse(await DBService.AddCourse("Course 9", DateTime.Today.AddMonths(8), DateTime.Today.AddMonths(9), CourseStatus.Started, instructors[rand.Next(instructors.Count)]));
            //var course42 = await DBService.GetCourse(await DBService.AddCourse("Course 10", DateTime.Today.AddMonths(9), DateTime.Today.AddMonths(10), CourseStatus.Planned, instructors[rand.Next(instructors.Count)]));
            //var course52 = await DBService.GetCourse(await DBService.AddCourse("Course 11", DateTime.Today.AddMonths(10), DateTime.Today.AddMonths(11), CourseStatus.Started, instructors[rand.Next(instructors.Count)]));
            //var course62 = await DBService.GetCourse(await DBService.AddCourse("Course 12", DateTime.Today.AddMonths(11), DateTime.Today.AddMonths(12), CourseStatus.Planned, instructors[rand.Next(instructors.Count)]));
            //term2.CourseId = new List<int>() { course12.Id, course22.Id, course32.Id, course42.Id, course52.Id, course62.Id };
            //await DBService.EditTerm(term2);

            //var course13 = await DBService.GetCourse(await DBService.AddCourse("Course 13", DateTime.Today.AddMonths(12), DateTime.Today.AddMonths(13), CourseStatus.Started, instructors[rand.Next(instructors.Count)]));
            //var course23 = await DBService.GetCourse(await DBService.AddCourse("Course 14", DateTime.Today.AddMonths(13), DateTime.Today.AddMonths(14), CourseStatus.Planned, instructors[rand.Next(instructors.Count)]));
            //var course33 = await DBService.GetCourse(await DBService.AddCourse("Course 15", DateTime.Today.AddMonths(14), DateTime.Today.AddMonths(15), CourseStatus.Started, instructors[rand.Next(instructors.Count)]));
            //var course43 = await DBService.GetCourse(await DBService.AddCourse("Course 16", DateTime.Today.AddMonths(15), DateTime.Today.AddMonths(16), CourseStatus.Planned, instructors[rand.Next(instructors.Count)]));
            //var course53 = await DBService.GetCourse(await DBService.AddCourse("Course 17", DateTime.Today.AddMonths(16), DateTime.Today.AddMonths(17), CourseStatus.Started, instructors[rand.Next(instructors.Count)]));
            //var course63 = await DBService.GetCourse(await DBService.AddCourse("Course 18", DateTime.Today.AddMonths(17), DateTime.Today.AddMonths(18), CourseStatus.Planned, instructors[rand.Next(instructors.Count)]));
            //term3.CourseId = new List<int>() { course13.Id, course23.Id, course33.Id, course43.Id, course53.Id, course63.Id };
            //await DBService.EditTerm(term3);

            //// Add Note
            //await DBService.AddNote("This is a test of notes");

            //// Add Assessment
            //await DBService.AddAssessment("Objective Assessment 1", DateTime.Today, DateTime.Today.AddMonths(1), AssessmentStyle.Objective);
            //await DBService.AddAssessment("Performance Assessment 1", DateTime.Today, DateTime.Today.AddMonths(1), AssessmentStyle.Performance);

            //// Add note to Course
            ////Course course1 = await DBService.GetCourse(course1id);
            ////Note note = await DBService.GetNote(noteid);
            ////await DBService.AddCourseNote(course1, note);
            ////await DBService.AddCourseNote(course1id, noteid);

            //int counter = 0;
            //foreach (Course course in await DBService.GetAllCourse())
            //{
            //    int currentnoteid = await DBService.AddNote($"Note test, counter: {counter}");
            //    await DBService.AddCourseNote(course.Id, currentnoteid);
            //    //course.CourseNoteString = (await DBService.GetNote(currentnoteid)).Contents;
            //    counter++;
            //}

            //await DBService.GetAllAssessment();
            //await DBService.GetAllCourse();
            //await DBService.GetAllInstructor();
            //await DBService.GetAllNote();
            //await DBService.GetAllTerm();
        }
    }
}