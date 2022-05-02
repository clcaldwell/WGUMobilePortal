using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

using WGUMobilePortal.Models;
using WGUMobilePortal.Services;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace WGUMobilePortal.ViewModels
{
    public class ModifyCoursesViewModel : BaseViewModel, IQueryAttributable
    {
        private ObservableCollection<Assessment> _assessmentSelectionList;
        private ObservableCollection<Assessment> _courseAssessments;
        private Course _currentCourse;
        private Note _currentNote;
        private ViewType _currentView;
        private DateTime _endDate;
        private DateTime _endDateMinimum;
        private Assessment _selectedAssessment;
        private Assessment _selectedAttachAssessment;
        private Course _selectedCourse;
        private CourseStatus _selectedCourseStatus;
        private DateTime _startDate;

        public ModifyCoursesViewModel()
        {
            Title = "Add/Modify Courses Page";
            DeleteCommand = new Command(async () => await Delete());
            SaveCommand = new Command(async () => await Save());
            NewAssessmentCommand = new Command(async () => await NewAssessment());
            ModifyAssessmentCommand = new Command(async () => await ModifyAssessment());
            RemoveAssessmentCommand = new Command(async () => await RemoveAssessment());
            AttachAssessmentCommand = new Command(async () => await AttachAssessment());
            OkAssessmentSelectionCommand = new Command(async () => await OkAssessmentSelection());
            CancelAssessmentSelectionCommand = new Command(async () => await CancelAssessmentSelection());
            ShareNoteCommand = new Command(async () => await ShareNote());

            if (CourseAssessments == null)
            {
                CourseAssessments = new ObservableCollection<Assessment>();
            }
        }

        public enum ViewType
        {
            CourseModification,
            AssessmentModification
        }

        public Command AddCommand { get; }

        public ObservableCollection<Assessment> AssessmentSelectionList
        {
            get => _assessmentSelectionList;
            set
            {
                SetProperty(ref _assessmentSelectionList, value);
                OnPropertyChanged(nameof(AssessmentSelectionList));
            }
        }

        public Command AttachAssessmentCommand { get; }
        public Command BackToMainModifyCommand { get; }
        public Command CancelAssessmentSelectionCommand { get; }
        public Command CancelCourseSelectionCommand { get; }

        public ObservableCollection<Assessment> CourseAssessments
        {
            get => _courseAssessments;
            set
            {
                SetProperty(ref _courseAssessments, value);
                OnPropertyChanged(nameof(CourseAssessments));
            }
        }

        public IEnumerable<CourseStatus> CourseStatus
        {
            get
            {
                return Enum.GetValues(typeof(CourseStatus))
                    .Cast<CourseStatus>();
            }
        }

        public Course CurrentCourse
        {
            get => _currentCourse;
            set
            {
                SetProperty(ref _currentCourse, value);
                OnPropertyChanged(nameof(CurrentCourse));
                _ = ChangeCurrent();
            }
        }

        public Note CurrentNote
        {
            get => _currentNote;
            set
            {
                SetProperty(ref _currentNote, value);
                OnPropertyChanged(nameof(CurrentNote));
            }
        }

        public ViewType CurrentView
        {
            get => _currentView;
            set
            {
                SetProperty(ref _currentView, value);
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public Command DeleteCommand { get; }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                SetProperty(ref _endDate, value);
                OnPropertyChanged(nameof(EndDate));
            }
        }

        public DateTime EndDateMinimum
        {
            get => _endDateMinimum;
            set
            {
                SetProperty(ref _endDateMinimum, value);
                OnPropertyChanged(nameof(EndDateMinimum));
            }
        }

        public Command ModifyAssessmentCommand { get; }
        public Command ModifyCommand { get; }
        public Command NewAssessmentCommand { get; }
        public Command OkAssessmentSelectionCommand { get; }
        public Command OpenCourseSelectionCommand { get; }
        public Command RemoveAssessmentCommand { get; }
        public Command RemoveCourseCommand { get; }
        public Command RemoveObjectiveAssessmentCommand { get; }
        public Command RemovePerformanceAssessmentCommand { get; }
        public Command SaveCommand { get; }
        public Command SelectCourseCommand { get; }

        public Assessment SelectedAssessment
        {
            get => _selectedAssessment;
            set
            {
                SetProperty(ref _selectedAssessment, value);
                OnPropertyChanged(nameof(SelectedAssessment));
            }
        }

        public Assessment SelectedAttachAssessment
        {
            get => _selectedAttachAssessment;
            set
            {
                SetProperty(ref _selectedAttachAssessment, value);
                OnPropertyChanged(nameof(SelectedAttachAssessment));
            }
        }

        public Course SelectedCourse
        {
            get => _selectedCourse;
            set
            {
                SetProperty(ref _selectedCourse, value);
                OnPropertyChanged(nameof(SelectedCourse));
            }
        }

        public CourseStatus SelectedCourseStatus
        {
            get => _selectedCourseStatus;
            set
            {
                SetProperty(ref _selectedCourseStatus, value);
                OnPropertyChanged(nameof(SelectedCourseStatus));
            }
        }

        public Command ShareNoteCommand { get; }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                SetProperty(ref _startDate, value);
                OnPropertyChanged(nameof(StartDate));
                EndDateMinimum = value.AddDays(1);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Marking as static causes program to crash," +
            " presumably because the UI requires INotifyPropertyChanged")]
        public DateTime StartDateMinimum => DateTime.Today.AddDays(-60).Date;

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if (string.IsNullOrWhiteSpace(query["id"])) // Will be null for new Course requests
            {
                Task.Run(async () => await LoadNew());
            }
            else
            {
                int id = int.Parse(HttpUtility.UrlDecode(query["id"]));
                StartDate = DateTime.Parse(HttpUtility.UrlDecode(query["startDate"]));
                EndDate = DateTime.Parse(HttpUtility.UrlDecode(query["endDate"]));
                Task.Run(async () => await Load(id));
            }
        }

        public async Task OnAppearing()
        {
            CurrentView = ViewType.CourseModification;
        }

        public async Task Save()
        {
            IsBusy = true;
            Course course = CurrentCourse;

            course.StartDate = StartDate;
            course.EndDate = EndDate;

            if (!await ValidateCourse(course))
            {
                return;
            }

            if (CourseAssessments.Any())
            {
                var objectiveAssessment = CourseAssessments.Where(x => x.Style == AssessmentStyle.Objective);
                if (objectiveAssessment.Any())
                {
                    course.ObjectiveAssessmentId = objectiveAssessment.First().Id;
                }
                else
                {
                    course.ObjectiveAssessmentId = 0;
                }

                var performanceAssessment = CourseAssessments.Where(x => x.Style == AssessmentStyle.Performance);
                if (performanceAssessment.Any())
                {
                    course.PerformanceAssessmentId = performanceAssessment.First().Id;
                }
                else
                {
                    course.PerformanceAssessmentId = 0;
                }
            }
            else
            {
                course.ObjectiveAssessmentId = 0;
                course.PerformanceAssessmentId = 0;
            }

            if (course.ObjectiveAssessmentId == 0 && !await Shell.Current.DisplayAlert("Alert",
                "You do not have an Objective Assessment assigned for this course. Are you sure you want to continue?",
                "Continue", "Cancel"))
            {
                return;
            }

            if (course.PerformanceAssessmentId == 0 && !await Shell.Current.DisplayAlert("Alert",
                "You do not have an Performance Assessment assigned for this course. Are you sure you want to continue?",
                "Continue", "Cancel"))
            {
                return;
            }

            if (CurrentNote.Id == 0)
            {
                course.NoteId = await DBService.AddNote(CurrentNote.Contents);
            }
            else
            {
                course.NoteId = CurrentNote.Id;
                await DBService.EditNote(CurrentNote);
            }

            if (course.Id == 0)
            {
                await DBService.AddCourse(course);
            }
            else
            {
                await DBService.EditCourse(course);
            }
            IsBusy = false;
            await Shell.Current.GoToAsync("..");
        }

        public async Task ShareNote()
        {
            await Share.RequestAsync(
                new ShareTextRequest
                {
                    Text = CurrentNote.Contents,
                    Title = "Share Note"
                });
        }

        public async Task<bool> ValidateCourse(Course course)
        {
            // Null Checks
            if (string.IsNullOrWhiteSpace(course.Name))
            {
                await Shell.Current.DisplayAlert("Alert", "Unable to save, must specify a Course Name", "OK");
                return false;
            }

            if (string.IsNullOrWhiteSpace(course.InstructorName))
            {
                await Shell.Current.DisplayAlert("Alert", "Unable to save, Instructor name cannot be empty", "OK");
                return false;
            }

            if (string.IsNullOrWhiteSpace(course.InstructorEmail))
            {
                await Shell.Current.DisplayAlert("Alert", "Unable to save, Instructor email cannot be empty", "OK");
                return false;
            }

            if (string.IsNullOrWhiteSpace(course.InstructorPhone))
            {
                await Shell.Current.DisplayAlert("Alert", "Unable to save, Instructor phone cannot be empty", "OK");
                return false;
            }

            // Regex Checks
            Regex PhoneRegex = new Regex(@"\d{3}-\d{3}-\d{4}");
            Regex EmailRegex = new Regex(@"\w+@wgu\.edu");

            if (!PhoneRegex.IsMatch(course.InstructorPhone))
            {
                await Shell.Current.DisplayAlert("Alert",
                    "Unable to save, Instructor phone is invalid.\n" +
                    "Phone must match this pattern:\n" +
                    "123-456-7891", "OK");
                return false;
            }

            if (!EmailRegex.IsMatch(course.InstructorEmail))
            {
                await Shell.Current.DisplayAlert("Alert",
                    "Unable to save, Instructor email is invalid.\n" +
                    "Email must match this pattern:\n" +
                    "name@wgu.edu\n" +
                    "ex. 'ccald15@wgu.edu'",
                    "OK");
                return false;
            }

            // Date checks
            if (course.StartDate.Date >= course.EndDate.Date)
            {
                await Shell.Current.DisplayAlert("Alert", "Unable to save, End date must be after Start date", "OK");
                return false;
            }

            return true;
        }

        private async Task AttachAssessment()
        {
            if (CourseAssessments.Any(x => x.Style == SelectedAttachAssessment.Style))
            {
                Assessment assessment = CourseAssessments.First(x => x.Style == SelectedAttachAssessment.Style);

                // Breaking MVVM pattern here - but this is much trickier
                // because of not being allowed to use an MVVM library
                bool response = await Application.Current.MainPage.DisplayAlert(
                    "Alert",
                    "This will overwrite the existing attached course:\n" +
                    $"{assessment.Name}\n" +
                    $"{assessment.DueDate}\n",
                    "Continue", "Cancel"
                    );

                if (response)
                {
                    CourseAssessments.Remove(assessment);
                    CourseAssessments.Add(SelectedAttachAssessment);
                }
            }
            else
            {
                CourseAssessments.Add(SelectedAttachAssessment);
            }
        }

        private async Task CancelAssessmentSelection()
        {
            await SetCourseAssessments();
            CurrentView = ViewType.CourseModification;
        }

        private async Task ChangeCurrent()
        {
            CurrentNote = await DBService.GetNote(CurrentCourse.NoteId);

            await SetCourseAssessments();
        }

        private async Task Delete()
        {
            if (await Shell.Current.DisplayAlert("Confirm Deletion", $"Are you sure you want to delete {CurrentCourse.Name}?", "Delete", "Cancel"))
            {
                await DBService.RemoveCourse(CurrentCourse.Id);
                await Shell.Current.GoToAsync("..");
            }
        }

        private async Task Load(int Id)
        {
            if (CourseAssessments == null)
            {
                CourseAssessments = new ObservableCollection<Assessment>();
            }

            CurrentCourse = await DBService.GetCourse(Id);
        }

        private async Task LoadNew()
        {
            CurrentCourse = new Course();
            StartDate = DateTime.Today;
            EndDate = DateTime.Today.AddDays(1);
            CurrentNote = new Note();

            // Hardcode instructor name to me, as per odd item on rubric
            CurrentCourse.InstructorName = "Coby Caldwell";
            CurrentCourse.InstructorPhone = "808-690-7792";
            CurrentCourse.InstructorEmail = "ccald15@wgu.edu";
        }

        private async Task ModifyAssessment()
        {
            CurrentView = ViewType.AssessmentModification;

            List<Assessment> assessmentList = (List<Assessment>)await DBService.GetAllAssessment();
            assessmentList = assessmentList.Where(assessment => assessment.CourseId == 0).ToList();
            AssessmentSelectionList = new ObservableCollection<Assessment>(assessmentList);
        }

        private async Task NewAssessment()
        {
            CurrentView = ViewType.AssessmentModification;
        }

        private async Task OkAssessmentSelection()
        {
            CurrentView = ViewType.CourseModification;
        }

        private async Task RemoveAssessment()
        {
            CourseAssessments.Remove(SelectedAssessment);
        }

        private async Task SetCourseAssessments()
        {
            CourseAssessments.Clear();

            if (CurrentCourse.ObjectiveAssessmentId > 0)
            {
                Assessment objectiveAssessment = await DBService.GetAssessment(CurrentCourse.ObjectiveAssessmentId);
                if (objectiveAssessment.Style == AssessmentStyle.Objective)
                {
                    CourseAssessments.Add(objectiveAssessment);
                }
            }

            if (CurrentCourse.PerformanceAssessmentId > 0)
            {
                Assessment performanceAssessment = await DBService.GetAssessment(CurrentCourse.PerformanceAssessmentId);
                if (performanceAssessment.Style == AssessmentStyle.Performance)
                {
                    CourseAssessments.Add(performanceAssessment);
                }
            }
        }
    }
}