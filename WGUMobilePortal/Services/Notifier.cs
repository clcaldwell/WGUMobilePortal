using SQLite;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WGUMobilePortal.Models;

using Notify = Plugin.LocalNotifications.CrossLocalNotifications;

namespace WGUMobilePortal.Services
{
    public class NotificationObject
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }

        public string Message { get; set; }
        public DateTime Time { get; set; }
        public string Title { get; set; }
    }

    public class Notifier
    {
        public List<NotificationObject> Notifications { get; set; }

        public async Task EvaluateNotifications()
        {
            Notifications = new List<NotificationObject>();

            IEnumerable<Assessment> Assessments = await DBService.GetAllAssessment();
            Assessments.Where(x => x.DueDateShouldNotify).ToList().ForEach(assessment =>
            {
                Notifications.Add(
                    new NotificationObject
                    {
                        Title = $"Assessment: {assessment.Name}",
                        Message = $"Assessment is Due: {assessment.DueDate}",
                        Time = DateTime.Now
                    });
            });

            IEnumerable<Course> Courses = await DBService.GetAllCourse();
            Courses.Where(x => x.StartDateShouldNotify).ToList().ForEach(course =>
            {
                Notifications.Add(
                    new NotificationObject
                    {
                        Title = $"Course: {course.Name}",
                        Message = $"Course '{course.Name}' starts {course.StartDate}",
                        Time = DateTime.Now
                    });
            });
            Courses.Where(x => x.EndDateShouldNotify).ToList().ForEach(course =>
            {
                Notifications.Add(
                    new NotificationObject
                    {
                        Title = $"Course: {course.Name}",
                        Message = $"Course '{course.Name}' ends {course.StartDate}",
                        Time = DateTime.Now
                    });
            });
        }

        public async Task OnStartNotifications()
        {
            await EvaluateNotifications();
            Notifications.ForEach(x =>
            Notify.Current.Show(
                x.Title, x.Message, x.Id, x.Time)
            );
        }
    }
}