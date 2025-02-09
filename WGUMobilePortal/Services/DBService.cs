﻿using SQLite;

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

        public static List<SQLiteConnection.ColumnInfo> AssessmentTableInfo { get; private set; }
        public static List<SQLiteConnection.ColumnInfo> CourseTableInfo { get; private set; }
        public static List<SQLiteConnection.ColumnInfo> NoteTableInfo { get; private set; }
        public static List<SQLiteConnection.ColumnInfo> TermTableInfo { get; private set; }

        public static async Task<int> AddAssessment(string name, DateTime dueDate, bool dueDateShouldNotify, AssessmentStyle style)
        {
            await InitDB();

            var assessment = new Assessment
            {
                Name = name,
                DueDate = dueDate,
                DueDateShouldNotify = dueDateShouldNotify,
                Style = style
            };

            await db.InsertAsync(assessment);
            return assessment.Id;
        }

        public static async Task<int> AddAssessment(Assessment assessment)
        {
            await InitDB();

            await db.InsertAsync(assessment);

            return assessment.Id;
        }

        public static async Task<int> AddCourse(string name, DateTime startdate, DateTime enddate, CourseStatus status, string instructorName, string instructorPhone, string instructorEmail)
        {
            await InitDB();

            var course = new Course
            {
                Name = name,
                StartDate = startdate,
                EndDate = enddate,
                Status = status,
                InstructorName = instructorName,
                InstructorPhone = instructorPhone,
                InstructorEmail = instructorEmail
            };

            await db.InsertAsync(course);
            return course.Id;
        }

        public static async Task<int> AddCourse(Course course)
        {
            await InitDB();

            await db.InsertAsync(course);

            return course.Id;
        }

        public static async Task AddCourseNote(Course course, Note note)
        {
            await InitDB();

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

        public static async Task EditAssessment(Assessment assessment)
        {
            await InitDB();
            await db.UpdateAsync(assessment);
        }

        public static async Task EditCourse(Course course)
        {
            await InitDB();
            await db.UpdateAsync(course);
        }

        public static async Task EditNote(Note note)
        {
            await InitDB();
            await db.UpdateAsync(note);
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

        public static async Task<IEnumerable<Assessment>> GetAllAssessment()
        {
            await InitDB();

            var term = await db.Table<Assessment>().ToListAsync();
            return term;
        }

        public static async Task<IEnumerable<Course>> GetAllCourse()
        {
            await InitDB();
            return await db.Table<Course>().ToListAsync();
        }

        public static async Task<IEnumerable<Note>> GetAllNote()
        {
            await InitDB();

            var note = await db.Table<Note>().ToListAsync();
            return note;
        }

        public static async Task<IEnumerable<Term>> GetAllTerm()
        {
            await InitDB();
            return await db.Table<Term>().ToListAsync();
        }

        public static async Task<Assessment> GetAssessment(int id)
        {
            await InitDB();

            var assessment = await db.GetAsync<Assessment>(id);
            return assessment;
        }

        public static async Task<Course> GetCourse(int id)
        {
            await InitDB();
            return await db.GetAsync<Course>(id);
        }

        public static async Task<Note> GetNote(int id)
        {
            await InitDB();

            var note = await db.GetAsync<Note>(id);
            return note;
        }

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

        public static async Task RemoveAssessment(int id)
        {
            await InitDB();

            await db.DeleteAsync<Assessment>(id);
        }

        public static async Task RemoveCourse(int id)
        {
            await InitDB();

            await db.DeleteAsync<Course>(id);
        }

        public static async Task RemoveNote(int id)
        {
            await InitDB();

            await db.DeleteAsync<Note>(id);
        }

        public static async Task RemoveTerm(int id)
        {
            await InitDB();

            await RemoveTermIdFromAllCourse(await GetTerm(id));
            await db.DeleteAsync<Term>(id);
        }

        public static async Task ResetAll()
        {
            await InitDB();
            await db.DeleteAllAsync<Term>();
            await db.DeleteAllAsync<Course>();
            await db.DeleteAllAsync<Assessment>();
            await db.DeleteAllAsync<Note>();
        }

        private static async Task AddTermIdToCourse(int termId, int courseId)
        {
            Course course = await GetCourse(courseId);
            course.TermId = termId;
            await EditCourse(course);
        }

        private static async Task InitDB()
        {
            if (db != null)
            {
                TermTableInfo = await db.GetTableInfoAsync("Term");
                CourseTableInfo = await db.GetTableInfoAsync("Course");
                AssessmentTableInfo = await db.GetTableInfoAsync("Assessment");
                NoteTableInfo = await db.GetTableInfoAsync("Note");
                return;
            }

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "portal.db");

            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTablesAsync<Term, Course, Assessment, Note>();

            TermTableInfo = await db.GetTableInfoAsync("Term");
            CourseTableInfo = await db.GetTableInfoAsync("Course");
            AssessmentTableInfo = await db.GetTableInfoAsync("Assessment");
            NoteTableInfo = await db.GetTableInfoAsync("Note");
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

        private static async Task RemoveTermIdFromAllCourse(Term term)
        {
            if (term.CourseId != null)
            {
                foreach (int id in term.CourseId)
                {
                    Course course = await GetCourse(id);
                    course.TermId = 0;
                    await EditCourse(course);
                }
            }
        }

        private static async Task RemoveTermIdFromCourse(int courseId)
        {
            Course course = await GetCourse(courseId);
            course.TermId = 0;
            await EditCourse(course);
        }
    }
}