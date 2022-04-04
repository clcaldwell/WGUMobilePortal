using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using WGUMobilePortal.Models;
using WGUMobilePortal.Services;

using Xamarin.Forms;

namespace WGUMobilePortal.ViewModels
{
    public class ModifyCoursesViewModel : BaseViewModel, IQueryAttributable
    {
        private ObservableCollection<Assessment> _assessmentSelectionList;
        private ObservableCollection<Assessment> _courseAssessments;
        private DateTime _courseEndDateMinimum;
        private DateTime _courseStartDate;
        private Course _currentCourse;
        private Instructor _currentInstructor;
        private Note _currentNote;
        private ViewType _currentView;
        private DateTime _endDate;
        private ObservableCollection<Instructor> _instructors;
        private Assessment _selectedAssessment;
        private Assessment _selectedAttachAssessment;
        private Course _selectedCourse;
        private CourseStatus _selectedCourseStatus;
        private Instructor _selectedInstructor;
        private DateTime _startDate;

        public ModifyCoursesViewModel()
        {
            Title = "Add/Modify Courses Page";
            DeleteCommand = new Command<Course>(Delete);
            SaveCommand = new Command(async () => await Save());
            NewAssessmentCommand = new Command(async () => await NewAssessment());
            ModifyAssessmentCommand = new Command(async () => await ModifyAssessment());
            RemoveAssessmentCommand = new Command(async () => await RemoveAssessment());
            AttachAssessmentCommand = new Command(async () => await AttachAssessment());
            ChangeInstructorCommand = new Command(async (obj) => await ChangeInstructor(obj));
            SelectInstructorCommand = new Command(async () => await SelectInstructor());
            OkAssessmentSelectionCommand = new Command(async () => await OkAssessmentSelection());
            CancelAssessmentSelectionCommand = new Command(async () => await CancelAssessmentSelection());

            if (CourseAssessments == null)
            {
                CourseAssessments = new ObservableCollection<Assessment>();
            }
            if (Instructors == null)
            {
                Instructors = new ObservableCollection<Instructor>();
            }
        }

        public enum ViewType
        {
            CourseModification,
            InstructorModification,
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

        public Command CancelSelectInstructorCommand { get; }

        public Command ChangeInstructorCommand { get; }

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
                ChangeCurrent();
            }
        }

        public Instructor CurrentInstructor
        {
            get => _currentInstructor;
            set
            {
                SetProperty(ref _currentInstructor, value);
                OnPropertyChanged(nameof(CurrentInstructor));
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

        public Command<Course> DeleteCommand { get; }

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
            get => _courseEndDateMinimum;
            set
            {
                SetProperty(ref _courseEndDateMinimum, value);
                OnPropertyChanged(nameof(EndDateMinimum));
            }
        }

        public ObservableCollection<Instructor> Instructors
        {
            get => _instructors;
            set
            {
                SetProperty(ref _instructors, value);
                OnPropertyChanged(nameof(Instructors));
            }
        }

        public bool IsInstructorSelection { get; set; }

        public bool IsInstructorView { get; set; }

        public Command ModifyAssessmentCommand { get; }

        public Command ModifyCommand { get; }

        public Command<Instructor> ModifyInstructorCommand { get; }

        public Command NewAssessmentCommand { get; }

        public Command NewInstructorCommand { get; }

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

        public Instructor SelectedInstructor
        {
            get => _selectedInstructor;
            set
            {
                SetProperty(ref _selectedInstructor, value);
                OnPropertyChanged(nameof(_selectedInstructor));
            }
        }

        public Command SelectInstructorCommand { get; }

        public DateTime StartDate
        {
            get => _courseStartDate;
            set
            {
                SetProperty(ref _courseStartDate, value);
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

            course.InstructorId = CurrentInstructor.Id;

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

            if (await DBService.GetNote(CurrentNote.Id) == null)
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
                    $"{assessment.StartDate}\n" +
                    $"{assessment.EndDate}\n",
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
            CurrentInstructor = await DBService.GetInstructor(CurrentCourse.InstructorId);
            CurrentNote = await DBService.GetNote(CurrentCourse.NoteId);

            StartDate = CurrentCourse.StartDate;
            EndDate = CurrentCourse.EndDate;

            await SetCourseAssessments();
        }

        private async Task ChangeInstructor(Object obj)
        {
            var picker = obj as Picker;

            picker.IsEnabled = true;
            picker.IsVisible = true;
            Device.BeginInvokeOnMainThread(() => picker.Focus());
            _ = picker.SelectedItem;
        }

        private async void Delete(Course course)
        {
            if (await Shell.Current.DisplayAlert("Confirm Deletion", $"Are you sure you want to delete {course.Name}?", "Delete", "Cancel"))
            {
                await DBService.RemoveCourse(course.Id);
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

            foreach (Instructor instructor in await DBService.GetAllInstructor())
            {
                Instructors.Add(instructor);
            }
        }

        private async Task LoadNew()
        {
            CurrentCourse = new Course();

            foreach (Instructor instructor in await DBService.GetAllInstructor())
            {
                Instructors.Add(instructor);
            }
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

        private async Task SelectInstructor()
        {
            CurrentInstructor = SelectedInstructor;
            SelectedInstructor = CurrentInstructor;
            IsInstructorView = true;
            IsInstructorSelection = false;
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