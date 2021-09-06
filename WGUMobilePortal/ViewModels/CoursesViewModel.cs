using System;
using System.Threading.Tasks;

using MvvmHelpers;
using MvvmHelpers.Commands;

using WGUMobilePortal.Models;
using WGUMobilePortal.Services;

namespace WGUMobilePortal.ViewModels
{
    public class CoursesViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Course> Course { get; set; }
        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand AddCommand { get; }
        public AsyncCommand<Course> RemoveCommand { get; }

        public CoursesViewModel()
        {

            Title = "Courses View";

            Course = new ObservableRangeCollection<Course>();

            RefreshCommand = new AsyncCommand(Refresh);
            AddCommand = new AsyncCommand(Add);
            RemoveCommand = new AsyncCommand<Course>(Remove);

            Load();
        }

        async Task Add()
        {

            string name = await App.Current.MainPage.DisplayPromptAsync("Name", "Name of Course");

            DateTime startdate = new DateTime(2020, 01, 01);
            DateTime enddate = new DateTime(2020, 06, 30);
            Status status = Status.Started;

            await DBService.AddCourse(name, startdate, enddate, status);
            await Refresh();
        }

        async Task Remove(Course course)
        {
            await DBService.RemoveCourse(course);
            await Refresh();
        }

        async Task Refresh()
        {
            IsBusy = true;
            Course.Clear();

            System.Collections.Generic.IEnumerable<Course> courses = await DBService.GetAllCourses();
            Course.AddRange(courses);
            IsBusy = false;
        }

        async void Load()
        {
            IsBusy = true;
            Course.Clear();
            System.Collections.Generic.IEnumerable<Course> courses = await DBService.GetAllCourses();
            Course.AddRange(courses);
            IsBusy = false;
        }
    }
}