using SQLite;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using WGUMobilePortal.Models;

using Xamarin.Essentials;

namespace WGUMobilePortal.Services
{
    public static class DBService
    {
        private static SQLiteAsyncConnection db;

        public static List<SQLiteConnection.ColumnInfo> TermTableInfo { get; private set; }
        public static List<SQLiteConnection.ColumnInfo> CourseTableInfo { get; private set; }
        public static List<SQLiteConnection.ColumnInfo> AssessmentTableInfo { get; private set; }
        public static List<SQLiteConnection.ColumnInfo> InstructorTableInfo { get; private set; }
        public static List<SQLiteConnection.ColumnInfo> NoteTableInfo { get; private set; }

        private static async Task InitDB()
        {
            if (db != null)
            {
                TermTableInfo = await db.GetTableInfoAsync("Term");
                CourseTableInfo = await db.GetTableInfoAsync("Course");
                AssessmentTableInfo = await db.GetTableInfoAsync("Assessment");
                InstructorTableInfo = await db.GetTableInfoAsync("Instructor");
                NoteTableInfo = await db.GetTableInfoAsync("Note");
                TermTableInfo.ToString();
                CourseTableInfo.ToString();
                AssessmentTableInfo.ToString();
                InstructorTableInfo.ToString();
                NoteTableInfo.ToString();
                return;
            }

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "portal.db");

            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTablesAsync<Term, Course, Assessment, Instructor, Note>();

            TermTableInfo = await db.GetTableInfoAsync("Term");
            CourseTableInfo = await db.GetTableInfoAsync("Course");
            AssessmentTableInfo = await db.GetTableInfoAsync("Assessment");
            InstructorTableInfo = await db.GetTableInfoAsync("Instructor");
            NoteTableInfo = await db.GetTableInfoAsync("Note");
            TermTableInfo.ToString();
            CourseTableInfo.ToString();
            AssessmentTableInfo.ToString();
            InstructorTableInfo.ToString();
            NoteTableInfo.ToString();
        }

        public static async Task ResetAll()
        {
            await InitDB();
            await db.DeleteAllAsync<Term>();
            await db.DeleteAllAsync<Course>();
            await db.DeleteAllAsync<Assessment>();
            await db.DeleteAllAsync<Instructor>();
            await db.DeleteAllAsync<Note>();
        }

        // Term Tasks
        public static async Task<Term> GetTerm(int id)
        {
            await InitDB();

            try
            {
                var term = await db.GetAsync<Term>(id);
                return term;
            }
            catch
            {
                return null;
            }
        }

        public static async Task<IEnumerable<Term>> GetAllTerm()
        {
            await InitDB();
            return await db.Table<Term>().ToListAsync();
        }

        public static async Task<int> AddTerm(string name, DateTime startdate, DateTime enddate)
        {
            await InitDB();

            var term = new Term
            {
                Name = name,
                StartDate = startdate,
                EndDate = enddate
            };

            await PropagateTermIdToCourse(term);
            await db.InsertAsync(term);

            return term.Id;
        }

        public static async Task<int> AddTerm(Term term)
        {
            await InitDB();

            await PropagateTermIdToCourse(term);
            await db.InsertAsync(term);

            return term.Id;
        }

        public static async Task EditTerm(int id, string name, DateTime startdate, DateTime enddate)
        {
            await InitDB();

            Term term = await db.GetAsync<Term>(id);

            term.Name = name;
            term.StartDate = startdate;
            term.EndDate = enddate;

            await PropagateTermIdToCourse(term);
            await db.UpdateAsync(term);
        }

        public static async Task EditTerm(Term term)
        {
            await InitDB();

            await PropagateTermIdToCourse(term);
            await db.UpdateAsync(term);
        }

        public static async Task RemoveTerm(int id)
        {
            await InitDB();

            await RemoveTermIdFromAllCourse(await GetTerm(id));
            await db.DeleteAsync<Term>(id);
        }

        private static async Task PropagateTermIdToCourse(Term term)
        {
            if (await GetTerm(term.Id) != null)
            {
                Term dbTerm = await GetTerm(term.Id);
                if (dbTerm.CourseId != null && dbTerm.CourseId != term.CourseId)
                {
                    List<int> inDbOnly = dbTerm.CourseId.Except(term.CourseId).ToList();
                    List<int> inLocalOnly = term.CourseId.Except(dbTerm.CourseId).ToList();

                    inDbOnly.ForEach(
                        async course => await RemoveTermIdFromCourse(course));
                }
            }

            if (term.CourseId != null)
            {
                foreach (int courseId in term.CourseId)
                {
                    await AddTermIdToCourse(term.Id, courseId);
                }
            }
        }

        private static async Task AddTermIdToCourse(int termId, int courseId)
        {
            Course course = await GetCourse(courseId);
            course.TermId = termId;
            await EditCourse(course);
        }

        private static async Task RemoveTermIdFromCourse(int courseId)
        {
            Course course = await GetCourse(courseId);
            course.TermId = null;
            await EditCourse(course);
        }

        private static async Task RemoveTermIdFromAllCourse(Term term)
        {
            if (term.CourseId != null)
            {
                foreach (int id in term.CourseId)
                {
                    Course course = await GetCourse(id);
                    course.TermId = null;
                    await EditCourse(course);
                }
            }
        }

        // Course Tasks
        public static async Task<Course> GetCourse(int id)
        {
            await InitDB();
            return await db.GetAsync<Course>(id);
        }

        public static async Task<IEnumerable<Course>> GetAllCourse()
        {
            await InitDB();
            return await db.Table<Course>().ToListAsync();
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

        public static async Task AddCourseNote(Course course, Note note)
        {
            await InitDB();

            //await db.UpdateAsync(note);
            course.NoteId = note.Id;
            await db.UpdateAsync(course);
        }

        public static async Task AddCourseNote(int courseid, int noteid)
        {
            await InitDB();

            Course course = await db.GetAsync<Course>(courseid);
            Note note = await db.GetAsync<Note>(noteid);

            course.NoteId = note.Id;

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

        public static async Task<int> AddInstructor(string firstname, string lastname, string phonenumber, string emailaddress)
        {
            await InitDB();

            var instructor = new Instructor
            {
                FirstName = firstname,
                LastName = lastname,
                PhoneNumber = phonenumber,
                EmailAddress = emailaddress
            };

            await db.InsertAsync(instructor);
            return instructor.Id;
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

        public static async Task<int> AddAssessment(string name, DateTime startdate, DateTime enddate, AssessmentStyle style)
        {
            await InitDB();

            var assessment = new Assessment
            {
                Name = name,
                StartDate = startdate,
                EndDate = enddate,
                Style = style
            };

            await db.InsertAsync(assessment);
            return assessment.Id;
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

        public Task<int> SaveAssessmentAsync(Assessment assessment)
        {
            if (assesssment.ID == 0)
            {
                return database.InsertAsync(assessment);
            }
            else
            {
                return database.UpdateAsync(assessment);
            }
        }

        public Task<int> DeleteAssessmentAsync(Assessment assessment)
        {
            return database.DeleteAsync(assessment);
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