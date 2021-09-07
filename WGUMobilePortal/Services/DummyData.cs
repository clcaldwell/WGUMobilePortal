using System;

using WGUMobilePortal.Models;


namespace WGUMobilePortal.Services
{
    public class DummyData
    {
        public static async void Main()
        {
            //IEnumerable<Term> terms = (IEnumerable<Term>)DBService.GetAllTerm();
            //foreach(Term term in terms)
            //{
            //DBService.RemoveTerm(term.Id);
            //};

            // Add Terms
            await DBService.AddTerm("Current Term 1", DateTime.Today, DateTime.Today.AddMonths(6));
            await DBService.AddTerm("Current Term 2", DateTime.Today, DateTime.Today.AddMonths(6));
            await DBService.AddTerm("Future Term 1", DateTime.Today.AddMonths(6), DateTime.Today.AddMonths(12));

            // Add Courses
            await DBService.AddCourse("Course 1", DateTime.Today, DateTime.Today.AddMonths(1), CourseStatus.Started);
            await DBService.AddCourse("Course 2", DateTime.Today.AddMonths(1), DateTime.Today.AddMonths(2), CourseStatus.Planned);

            // Add Instructor
            await DBService.AddInstructor("Harper", "Setton", "111-222-1234", "test@gmail.com");

            // Add Note
            int noteid = await DBService.AddNote("This is a test of notes");

            // Add Assessment
            await DBService.AddAssessment("Objective Assessment 1", DateTime.Today, DateTime.Today.AddMonths(1), AssessmentStyle.Objective);
            await DBService.AddAssessment("Performance Assessment 1", DateTime.Today, DateTime.Today.AddMonths(1), AssessmentStyle.Performance);

            // Add note to Course
            //Course course1 = await DBService.GetCourse(course1id);
            //Note note = await DBService.GetNote(noteid);
            //await DBService.AddCourseNote(course1, note);
            //await DBService.AddCourseNote(course1id, noteid);

            int counter = 0;
            foreach (Course course in await DBService.GetAllCourse())
            {
                int currentnoteid = await DBService.AddNote($"Note test, counter: {counter}");
                await DBService.AddCourseNote(course.Id, currentnoteid);
                //course.CourseNoteString = (await DBService.GetNote(currentnoteid)).Contents;
                counter++;
            }

            //await DBService.GetAllAssessment();
            //await DBService.GetAllCourse();
            //await DBService.GetAllInstructor();
            //await DBService.GetAllNote();
            //await DBService.GetAllTerm();
        }

    }
}
