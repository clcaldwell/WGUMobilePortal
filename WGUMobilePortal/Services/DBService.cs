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
        static async Task Init()
        {
            if (db != null)
                return;

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "portal.db");
            
            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTablesAsync<Term, Course, Assessment>();
        }

        public static async Task<Term> GetTerm(int id)
        {
            await Init();

            var term = await db.GetAsync<Term>(id);
            return term;
        }
        public static async Task<IEnumerable<Term>> GetAllTerm()
        {
            await Init();

            var term = await db.Table<Term>().ToListAsync();
            return term;
        }
        public static async Task AddTerm(string name, DateTime startdate, DateTime enddate)
        {
            await Init();
            
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
            await Init();
        }
        public static async Task RemoveTerm(int id)
        {
            await Init();

            await db.DeleteAsync<Term>(id);
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
