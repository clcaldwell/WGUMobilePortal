using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using WGUMobilePortal.Models;
using WGUMobilePortal.Services;

using Xamarin.Forms;

namespace WGUMobilePortal.ViewModels
{
    public class CoursesViewModel : BaseViewModel
    {
        public ObservableCollection<Course> Course { get; set; }
        public Command RefreshCommand { get; }
        public Command AddCommand { get; }
        public Command<Course> RemoveCommand { get; }

        public CoursesViewModel()
        {

            Title = "Courses View";

            Course = new ObservableCollection<Course>();

            RefreshCommand = new Command(async () => await Refresh());
            AddCommand = new Command(async () => await Add());
            RemoveCommand = new Command<Course>(Remove);

            Load();
        }

        public async Task OnAppearing()
        {
            await Load();
        }

        async Task Add()
        {

            string name = await App.Current.MainPage.DisplayPromptAsync("Name", "Name of Course");
            string strstartdate = await App.Current.MainPage.DisplayPromptAsync("Start Date", "Course Start Date");
            string strenddate = await App.Current.MainPage.DisplayPromptAsync("End Date", "Course End Date");

            DateTime startdate = DateTime.Parse(strstartdate);
            DateTime enddate = DateTime.Parse(strenddate);

            // DateTime startdate = new DateTime(2020, 01, 01);
            // DateTime enddate = new DateTime(2020, 06, 30);
            CourseStatus status = CourseStatus.Started;

            await DBService.AddCourse(name, startdate, enddate, status);
            await Task.Factory.StartNew(() => Refresh());
        }

        async void Remove(Course course)
        {
            await DBService.RemoveCourse(course.Id);
            await Task.Factory.StartNew(() => Refresh());
        }

        async Task Refresh()
        {
            IsBusy = true;
            Course.Clear();
            await ReloadCourses();
            IsBusy = false;
        }

        async Task Load()
        {
            await Task.Factory.StartNew(() => Refresh());
        }

        async Task ReloadCourses()
        {

            var courses = await DBService.GetAllCourse();
            foreach (var currentCourse in courses)
            {
                Course course = new Course
                {
                    Id = currentCourse.Id,
                    Name = currentCourse.Name,
                    StartDate = currentCourse.StartDate,
                    EndDate = currentCourse.EndDate,
                    Status = currentCourse.Status,
                    Notify = currentCourse.Notify,
                    InstructorId = currentCourse.InstructorId,
                    NoteId = currentCourse.NoteId
                    //Assessment = currentCourse.Assessment,
                    //NoteContent = currentCourse.Note.Contents//,
                    //InstructorFirstName = currentCourse.Instructor.FirstName,
                    //InstructorLastName = currentCourse.Instructor.LastName,
                    //InstructorPhoneNumber = currentCourse.Instructor.PhoneNumber,
                    //InstructorEmailAddress = currentCourse.Instructor.EmailAddress
                };
                // Called after initialization to force OnPropertyChanged
                if (currentCourse.NoteId != null)
                {
                    //course.NoteContent = currentCourse.Note.Contents;
                }
                else
                {
                    //course.NoteContent = "Sample Note";
                }

                Course.Add(course);
            }
        }
    }
}

    /*
    public class Course : Models.Course
    {
        private Note _note;
        private string _noteContent;

        private Instructor _instructor;
        private string _instructorFirstName;
        private string _instructorLastName;
        private string _instructorPhoneNumber;
        private string _instructorEmailAddress;

        public new Note Note
        {
            get => _note;
            set
            {
                if (value != null && value != _note)
                {
                    SetProperty(ref _note, value);
                    OnPropertyChanged();
                    
                    NoteContent = value.Contents;
                }
            }
        }

        public string NoteContent
        {
            get => _noteContent;
            set
            {
                if (!string.Equals(_noteContent, value))
                {
                    SetProperty(ref _noteContent, value);
                    OnPropertyChanged();
                }
            }
        }

        public new Instructor Instructor
        {
            get => _instructor;
            set
            {
                if (value != null && value != _instructor)
                {
                    SetProperty(ref _instructor, value);
                    OnPropertyChanged();
                    
                    InstructorFirstName = value.FirstName;
                    InstructorLastName = value.LastName;
                    InstructorPhoneNumber = value.PhoneNumber;
                    InstructorEmailAddress = value.EmailAddress;
                }
            }
        }

        public string InstructorFirstName
        {
            get => _instructorFirstName;
            set
            {
                if (!string.Equals(_instructorFirstName, value))
                {
                    SetProperty(ref _instructorFirstName, value);
                    OnPropertyChanged();
                }
            }
        }

        public string InstructorLastName
        {
            get => _instructorLastName;
            set
            {
                if (!string.Equals(_instructorLastName, value))
                {
                    SetProperty(ref _instructorLastName, value);
                    OnPropertyChanged();
                }
            }
        }

        public string InstructorPhoneNumber
        {
            get => _instructorPhoneNumber;

            set
            {
                if (!string.Equals(_instructorPhoneNumber, value))
                {
                    SetProperty(ref _instructorPhoneNumber, value);
                    OnPropertyChanged();
                }
            }
        }

        public string InstructorEmailAddress
        {
            get => _instructorEmailAddress;
            set
            {
                if (!string.Equals(_instructorEmailAddress, value))
                {
                    SetProperty(ref _instructorEmailAddress, value);
                    OnPropertyChanged();
                }
            }
        }



    }
} 
*/