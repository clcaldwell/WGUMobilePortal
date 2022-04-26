using System.Collections.ObjectModel;
using System.Threading.Tasks;

using WGUMobilePortal.Models;
using WGUMobilePortal.Services;
using WGUMobilePortal.Views;

using Xamarin.Forms;

namespace WGUMobilePortal.ViewModels
{
    public class CoursesViewModel : BaseViewModel
    {
        public CoursesViewModel()
        {
            Title = "Courses View";

            Courses = new ObservableCollection<Course>();

            RefreshCommand = new Command(async () => await Refresh());
            AddCommand = new Command(async () => await Add());
            RemoveCommand = new Command<Course>(Remove);
            ModifyCommand = new Command<Course>(Modify);

            IsBusy = true;
        }

        public Command AddCommand { get; }
        public ObservableCollection<Course> Courses { get; set; }

        public Command<Course> ModifyCommand { get; }
        public Command RefreshCommand { get; }
        public Command<Course> RemoveCommand { get; }

        public async Task OnAppearing()
        {
            await Load();
        }

        private async Task Add()
        {
            await AppShell.Current.GoToAsync($"{nameof(ModifyCoursesPage)}?id={null}");

            await Refresh();
        }

        private async Task Load()
        {
            await Task.Run(() => Refresh());
        }

        private async void Modify(Course course)
        {
            await AppShell.Current.GoToAsync($"{nameof(ModifyCoursesPage)}?id={course.Id}&name={course.Name}&startDate={course.StartDate}&endDate={course.EndDate}");
        }

        private async Task Refresh()
        {
            IsBusy = true;
            Courses.Clear();
            var courses = await DBService.GetAllCourse();
            foreach (Course course in courses)
            {
                Courses.Add(course);
            }
            IsBusy = false;
        }

        private async void Remove(Course course)
        {
            if (await Shell.Current.DisplayAlert("Confirm Deletion", $"Are you sure you want to delete {course.Name}", "Delete", "Cancel"))
            {
                await DBService.RemoveCourse(course.Id);
                Courses.Remove(course);
            }
        }
    }
}