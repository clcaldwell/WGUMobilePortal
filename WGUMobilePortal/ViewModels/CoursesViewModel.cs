using System;
using System.Threading.Tasks;

using MvvmHelpers;
using MvvmHelpers.Commands;

using WGUMobilePortal.Services;

namespace WGUMobilePortal.ViewModels
{
    public class CoursesViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Models.Course> Course { get; set; }
        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand AddCommand { get; }
        public AsyncCommand<Models.Course> RemoveCommand { get; }

        public CoursesViewModel()
        {

            Title = "Courses View";

            Course = new ObservableRangeCollection<Models.Course>();

            RefreshCommand = new AsyncCommand(Refresh);
            AddCommand = new AsyncCommand(Add);
            RemoveCommand = new AsyncCommand<Models.Course>(Remove);

            Load();
        }

        async Task Add()
        {

            string name = await App.Current.MainPage.DisplayPromptAsync("Name", "Name of Course");

            DateTime startdate = new DateTime(2020, 01, 01);
            DateTime enddate = new DateTime(2020, 06, 30);

            await DBService.AddCourse(name, startdate, enddate);
            await Refresh();
        }

        async Task Remove(Models.Course term)
        {
            await DBService.RemoveCourse(term.Id);
            await Refresh();
        }

        async Task Refresh()
        {
            IsBusy = true;

            Course.Clear();

            var courses = await DBService.GetAllCourse();

            Course.AddRange(courses);

            IsBusy = false;
        }

        async void Load()
        {
            IsBusy = true;
            Course.Clear();
            var courses = await DBService.GetAllCourse();
            Course.AddRange(courses);
            IsBusy = false;
        }
    }
}