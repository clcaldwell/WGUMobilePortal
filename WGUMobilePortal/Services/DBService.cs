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
                return;

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "portal.db");
            
            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTablesAsync<Term, Course, Assessment, Instructor, Note>();
        }

        // Term Tasks
        public static async Task<Term> GetTerm(int id)
        {
            await InitDB();

            var term = await db.GetAsync<Term>(id);
            return term;
        }
        public static async Task<IEnumerable<Term>> GetAllTerm()
        {
            await InitDB();

            var term = await db.Table<Term>().ToListAsync();
            return term;
        }
        public static async Task AddTerm(string name, DateTime startdate, DateTime enddate)
        {
            await InitDB();
            
            var term = new Term
            {
                Name = name,
                StartDate = startdate,
                EndDate = enddate
            };

            var id = await db.InsertAsync(term);
        }
        public static async Task EditTerm()
        {
            await InitDB();
        }
        public static async Task RemoveTerm(int id)
        {
            await InitDB();

            await db.DeleteAsync<Term>(id);
        }

        // Course Tasks
        public static async Task<Course> GetCourse(int id)
        {
            await InitDB();

            return await db.FindAsync<Course>(id);
            //return await db.GetAsync<Course>(id);
        }
        public static async Task<IEnumerable<Course>> GetAllCourse()
        {
            await InitDB();

            var term = await db.Table<Course>().ToListAsync();
            return term;
        }
        public static async Task<int> AddCourse(string name, DateTime startdate, DateTime enddate, CourseStatus status)
        {
            await InitDB();

            var course = new Course
            {
                Name = name,
                StartDate = startdate,
                EndDate = enddate,
                Status = status
            };

            await db.InsertAsync(course);
            return course.Id;
        }
        public static async Task EditCourse()
        {
            await InitDB();
        }
        public static async Task RemoveCourse(int id)
        {
            await InitDB();

            await db.DeleteAsync<Course>(id);
        }
        public static async Task AddCourseNote(Course course, Note note)
        {
            await InitDB();

            course.CourseNote = note;

            //CourseNote(note);

            //Course course.CourseNote = note;

            //course.CourseNote = note;
            //var term = new Course
            //{
            //Id = course.Id,
            //CourseNote = note
            //};

            //var id = await db.InsertAsync(term);
            await db.UpdateAsync(course);
        }
        public static async Task AddCourseNote(int courseid, int noteid)
        {
            await InitDB();

            Course course = await db.GetAsync<Course>(courseid);
            Note note = await db.GetAsync<Note>(noteid);

            course.CourseNote = note;

            await db.UpdateAsync(course);
        }

        // Course Instructor
        public static async Task<Instructor> GetInstructor(int id)
        {
            await InitDB();

            var instructor = await db.GetAsync<Instructor>(id);
            return instructor;
        }
        public static async Task<IEnumerable<Instructor>> GetAllInstructor()
        {
            await InitDB();

            var instructor = await db.Table<Instructor>().ToListAsync();
            return instructor;
        }
        public static async Task AddInstructor(string firstname, string lastname, string phonenumber, string emailaddress)
        {
            await InitDB();

            var instructor = new Instructor
            {
                FirstName = firstname,
                LastName = lastname,
                PhoneNumber = phonenumber,
                EmailAddress = emailaddress
            };

            var id = await db.InsertAsync(instructor);
        }
        public static async Task EditInstructor()
        {
            await InitDB();
        }
        public static async Task RemoveInstructor(int id)
        {
            await InitDB();

            await db.DeleteAsync<Instructor>(id);
        }

        // course Notes
        public static async Task<Note> GetNote(int id)
        {
            await InitDB();

            var note = await db.GetAsync<Note>(id);
            return note;
        }
        public static async Task<IEnumerable<Note>> GetAllNote()
        {
            await InitDB();

            var note = await db.Table<Note>().ToListAsync();
            return note;
        }
        public static async Task<int> AddNote(string contents)
        {
            await InitDB();

            var note = new Note
            { 
                Contents = contents,
                TimeStamp = DateTime.Now
            };

            await db.InsertAsync(note);
            return note.Id;
        }
        public static async Task EditNote()
        {
            await InitDB();
        }
        public static async Task RemoveNote(int id)
        {
            await InitDB();

            await db.DeleteAsync<Note>(id);
        }

        // Assessment Tasks
        public static async Task<Assessment> GetAssessment(int id)
        {
            await InitDB();

            var assessment = await db.GetAsync<Assessment>(id);
            return assessment;
        }
        public static async Task<IEnumerable<Assessment>> GetAllAssessment()
        {
            await InitDB();

            var term = await db.Table<Assessment>().ToListAsync();
            return term;
        }
        public static async Task AddAssessment(string name, DateTime startdate, DateTime enddate, AssessmentStyle style)
        {
            await InitDB();

            var term = new Assessment
            {
                Name = name,
                StartDate = startdate,
                EndDate = enddate,
                Style = style
            };

            var id = await db.InsertAsync(term);
        }
        public static async Task EditAssessment()
        {
            await InitDB();
        }
        public static async Task RemoveAssessment(int id)
        {
            await InitDB();

            await db.DeleteAsync<Assessment>(id);
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
