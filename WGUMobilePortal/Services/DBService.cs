using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using SQLite;

using WGUMobilePortal.Models;

using Xamarin.Essentials;

namespace WGUMobilePortal.Services
{
    public static class DBService
    {
        static SQLiteAsyncConnection db;
        static async Task InitDB()
        {
            if (db != null)
            {
                return;
            }

            string databasePath = Path.Combine(FileSystem.AppDataDirectory, "portal.db");
            
            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTablesAsync<Term, Course, Assessment>();
        }

        // Term Tasks
        public static async Task<Term> GetTerm(int id)
        {
            await InitDB();
            return await db.GetAsync<Term>(id);
        }
        public static async Task<IEnumerable<Term>> GetAllTerms()
        {
            await InitDB();
            return await db.Table<Term>().ToListAsync();
        }
        public static async Task AddTerm(Term term)
        {
            await InitDB();
            await db.InsertAsync(term);
        }
        public static async Task AddTerm(string name, DateTime startdate, DateTime enddate)
        {
            await InitDB();

            Term term = new Term
            {
                Name = name,
                StartDate = startdate,
                EndDate = enddate,
                Course = null
            };

            await db.InsertAsync(term);
        }
        public static async Task EditTerm(Term term)
        {
            await InitDB();
            await db.UpdateAsync(term);
        }
        public static async Task RemoveTerm(int id)
        {
            await InitDB();
            await db.DeleteAsync<Term>(id);
        }
        public static async Task RemoveTerm(Term term)
        {
            await InitDB();
            await db.DeleteAsync<Term>(term);
        }

        // Course Tasks
        public static async Task<Course> GetCourse(int id)
        {
            await InitDB();

            Course course = await db.GetAsync<Course>(id);
            return course;
        }
        public static async Task<IEnumerable<Course>> GetAllCourses()
        {
            await InitDB();

            List<Course> term = await db.Table<Course>().ToListAsync();
            return term;
        }
        public static async Task AddCourse(Course course)
        {
            await InitDB();
            await db.InsertAsync(course);
        }
        public static async Task AddCourse(string name, DateTime startdate, DateTime enddate, Status status)
        {
            await InitDB();

            Course term = new Course
            {
                Name = name,
                StartDate = startdate,
                EndDate = enddate,
                Status = status
            };

            //int id = await db.InsertAsync(term);
            await db.InsertAsync(term);
        }
        public static async Task EditCourse(Course course)
        {
            await InitDB();
            await db.UpdateAsync(course);
        }
        public static async Task RemoveCourse(int id)
        {
            await InitDB();
            await db.DeleteAsync<Course>(id);
        }
        public static async Task RemoveCourse(Course course)
        {
            await InitDB();
            await db.DeleteAsync<Course>(course);
        }

        // Course Instructor
        public static async Task<Course.Instructor> GetInstructor(int id)
        {
            await InitDB();
            return await db.GetAsync<Course.Instructor>(id);
        }
        public static async Task<IEnumerable<Course.Instructor>> GetAllInstructors()
        {
            await InitDB();
            return await db.Table<Course.Instructor>().ToListAsync();
        }
        public static async Task AddInstructor(Course.Instructor instructor)
        {
            await InitDB();
            await db.InsertAsync(instructor);
        }
        public static async Task AddInstructor(string firstname, string lastname, string phonenumber, string emailaddress)
        {
            await InitDB();

            Course.Instructor instructor = new Course.Instructor
            {
                FirstName = firstname,
                LastName = lastname,
                PhoneNumber = phonenumber,
                EmailAddress = emailaddress
            };

            //var id = await db.InsertAsync(instructor);
            await db.InsertAsync(instructor);
        }
        public static async Task EditInstructor(Course.Instructor instructor)
        {
            await InitDB();
            await db.UpdateAsync(instructor);
        }
        public static async Task RemoveInstructor(int id)
        {
            await InitDB();
            await db.DeleteAsync<Course.Instructor>(id);
        }
        public static async Task RemoveInstructor(Course.Instructor instructor)
        {
            await InitDB();
            await db.DeleteAsync<Course.Instructor>(instructor);
        }


        // course Notes
        public static async Task<Course.Note> GetNote(int id)
        {
            await InitDB();
            return await db.GetAsync<Course.Note>(id);
        }
        public static async Task<IEnumerable<Course.Note>> GetAllNotes()
        {
            await InitDB();
            return await db.Table<Course.Note>().ToListAsync();
        }
        public static async Task AddNote(string contents)
        {
            await InitDB();

            Course.Note note = new Course.Note
            {
                Contents = contents,
                TimeStamp = DateTime.Now
            };

            //var id = await db.InsertAsync(note);
            await db.InsertAsync(note);
        }
        public static async Task AddNote(Course.Note note)
        {
            await InitDB();
            //var id = await db.InsertAsync(note);
            await db.InsertAsync(note);
        }
        public static async Task EditNote(Course.Note note)
        {
            await InitDB();
            await db.UpdateAsync(note);
        }
        public static async Task RemoveNote(int id)
        {
            await InitDB();
            await db.DeleteAsync<Course.Note>(id);
        }
        public static async Task RemoveNote(Course.Note note)
        {
            await InitDB();
            await db.DeleteAsync<Course.Note>(note);
        }

        // Assessment Tasks
        public static async Task<Assessment> GetAssessment(int id)
        {
            await InitDB();
            return await db.GetAsync<Assessment>(id);
        }
        public static async Task<IEnumerable<Assessment>> GetAllAssessments()
        {
            await InitDB();
            return await db.Table<Assessment>().ToListAsync();
         }
        public static async Task AddAssessment(string name, DateTime startdate, DateTime enddate)
        {
            await InitDB();

            Assessment term = new Assessment
            {
                Name = name,
                StartDate = startdate,
                EndDate = enddate
            };

            //var id = await db.InsertAsync(term);
            await db.InsertAsync(term);
        }
        public static async Task AddAssessment(Assessment assessment)
        {
            await InitDB();
            //var id = await db.InsertAsync(assessment);
            await db.InsertAsync(assessment);
        }
        public static async Task EditAssessment(Assessment assessment)
        {
            await InitDB();
            await db.UpdateAsync(assessment);
        }
        public static async Task RemoveAssessment(int id)
        {
            await InitDB();
            await db.DeleteAsync<Assessment>(id);
        }
        public static async Task RemoveAssessment(Assessment assessment)
        {
            await InitDB();
            await db.DeleteAsync<Assessment>(assessment);
        }

    }
}



/*
        public Task<Term> GetTermAsync(int id)
        {
            return database.Table<Term>()
                .Where(i => i.ID == id)
                .FirstOrDefaultAsync();
        }

        public Task<int> SaveTermAsync(Term term)
        {
            if (term.ID == 0)
            {
                return database.InsertAsync(term);
            }
            else
            {
                return database.UpdateAsync(term);
            }
        }

        public Task<int> DeleteTermAsync(Term term)
        {
            return database.DeleteAsync(term);
        }

        public Task<List<Course>> GetCoursesAsync(Term term)
        {
            return database.Table<Course>()
                .Where(a => a.TermID == term.ID).ToListAsync();
        }

        public Task<int> SaveCourseAsync(Course course)
        {
            if (course.ID == 0)
            {
                database.InsertAsync(new Note());
                return database.InsertAsync(course);
            }
            else
            {
                return database.UpdateAsync(course);
            }
        }
        public Task<int> DeleteCourseAsync(Course course)
        {
            return database.DeleteAsync(course);
        }

        public Task<Note> GetNotesAsync(Course course)
        {
            return database.Table<Note>()
                .Where(a => a.CourseID == course.ID).FirstOrDefaultAsync();
        }

        public Task<int> SaveNoteAsync(Note note)
        {
            if (note.ID == 0)
            {
                return database.InsertAsync(note);
            }
            else
            {
                return database.UpdateAsync(note);
            }
        }

        public Task<int> DeleteNoteAsync(Note note)
        {
            return database.DeleteAsync(note);
        }

        public Task<int> SaveAssessmentAsync(Assessment assesssment)
        {
            if (assesssment.ID == 0)
            {
                return database.InsertAsync(assesssment);
            }
            else
            {
                return database.UpdateAsync(assesssment);
            }
        }

        public Task<int> DeleteAssessmentAsync(Assessment assesssment)
        {
            return database.DeleteAsync(assesssment);
        }

        public Task<Assessment> GetAssessmentObjective(int CourseID)
        {
            return database.Table<Assessment>()
                .Where(a => a.CourseID == CourseID && a.AssessmentType == "Objective").FirstOrDefaultAsync();
        }

        public Task<Assessment> GetAssessmentPerformance(int CourseID)
        {
            return database.Table<Assessment>()
                .Where(a => a.CourseID == CourseID && a.AssessmentType == "Performance").FirstOrDefaultAsync();
        } */
