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
        private ObservableCollection<Assessment> _courseAssessments;

        private DateTime _courseEndDateMinimum;
        private DateTime _courseStartDate;
        private Course _currentCourse;

        private Instructor _currentInstructor;

        private Note _currentNote;

        private Assessment _currentObjectiveAssessment;

        private Assessment _currentPerformanceAssessment;

        private ViewType _currentView;

        private DateTime _endDate;

        private ObservableCollection<Instructor> _instructors;

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
            ModifyAssessmentCommand = new Command<Assessment>(ModifyAssessment);
            RemoveAssessmentCommand = new Command<Assessment>(RemoveAssessment);
            NewInstructorCommand = new Command(async () => await NewInstructor());
            ModifyInstructorCommand = new Command<Instructor>(ModifyInstructor);
            BackToMainModifyCommand = new Command(async () => await BackToModify());
            ChangeInstructorCommand = new Command(async (obj) => await ChangeInstructor(obj));
            SelectInstructorCommand = new Command(async () => await SelectInstructor());
            CancelSelectInstructorCommand = new Command(async () => await CancelSelectInstructor());

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

        public Command BackToMainModifyCommand { get; }

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

        public Assessment CurrentObjectiveAssessment
        {
            get => _currentObjectiveAssessment;
            set
            {
                SetProperty(ref _currentObjectiveAssessment, value);
                OnPropertyChanged(nameof(CurrentObjectiveAssessment));
            }
        }

        public Assessment CurrentPerformanceAssessment
        {
            get => _currentPerformanceAssessment;
            set
            {
                SetProperty(ref _currentPerformanceAssessment, value);
                OnPropertyChanged(nameof(CurrentPerformanceAssessment));
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

        public Command<Assessment> ModifyAssessmentCommand { get; }

        public Command ModifyCommand { get; }

        public Command<Instructor> ModifyInstructorCommand { get; }

        public Command NewAssessmentCommand { get; }

        public Command NewInstructorCommand { get; }

        public Command OpenCourseSelectionCommand { get; }

        public Command<Assessment> RemoveAssessmentCommand { get; }

        public Command RemoveCourseCommand { get; }

        public Command SaveCommand { get; }

        public Command SelectCourseCommand { get; }

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
                //Name = HttpUtility.UrlDecode(query["name"]);
                //StartDate = DateTime.Parse(HttpUtility.UrlDecode(query["startDate"]));
                //EndDate = DateTime.Parse(HttpUtility.UrlDecode(query["endDate"]));
                int id = int.Parse(HttpUtility.UrlDecode(query["id"]));

                Task.Run(async () => await Load(id));
            }
        }

        public async Task OnAppearing()
        {
            //Instructors = (ObservableCollection<Instructor>)await DBService.GetAllInstructor();
            CurrentView = ViewType.CourseModification;
        }

        public async Task Save()
        {
            Course course = CurrentCourse;

            course.InstructorId = CurrentInstructor.Id;
            course.NoteId = CurrentNote.Id;
            course.ObjectiveAssessmentId = CurrentObjectiveAssessment.Id;
            course.PerformanceAssessmentId = CurrentPerformanceAssessment.Id;

            await DBService.EditAssessment(CurrentPerformanceAssessment);
            await DBService.EditAssessment(CurrentObjectiveAssessment);
            await DBService.EditNote(CurrentNote);
            await DBService.EditCourse(course);

            if (course.Id == 0)
            {
                await DBService.AddCourse(course);
            }
            else
            {
                await DBService.EditCourse(course);
            }

        }

        private async Task BackToModify()
        {
            CurrentView = ViewType.CourseModification;
        }

        private async Task CancelSelectInstructor()
        {
            IsInstructorView = true;
            IsInstructorSelection = false;
        }

        private async void ChangeCurrent()
        {
            CurrentInstructor = await DBService.GetInstructor(CurrentCourse.InstructorId);
            CurrentNote = await DBService.GetNote(CurrentCourse.NoteId);

            StartDate = CurrentCourse.StartDate;
            EndDate = CurrentCourse.EndDate;

            CourseAssessments.Clear();

            if (CurrentCourse.ObjectiveAssessmentId > 0)
            {
                Assessment objectiveAssessment = await DBService.GetAssessment(CurrentCourse.ObjectiveAssessmentId);
                if (objectiveAssessment.Style == AssessmentStyle.Objective)
                {
                    CourseAssessments.Add(objectiveAssessment);
                    CurrentObjectiveAssessment = objectiveAssessment;
                }
            }

            if (CurrentCourse.PerformanceAssessmentId > 0)
            {
                Assessment performanceAssessment = await DBService.GetAssessment(CurrentCourse.PerformanceAssessmentId);
                if (performanceAssessment.Style == AssessmentStyle.Performance)
                {
                    CourseAssessments.Add(performanceAssessment);
                    CurrentPerformanceAssessment = performanceAssessment;
                }
            }
        }

        private async Task ChangeInstructor(Object obj)
        {
            var picker = obj as Picker;

            picker.IsEnabled = true;
            picker.IsVisible = true;
            Device.BeginInvokeOnMainThread(() => picker.Focus());
            //picker.Focus();
            _ = picker.SelectedItem;

            //SelectedInstructor = CurrentInstructor;
            //IsInstructorView = false;
            //IsInstructorSelection = true;
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
            CurrentCourse = await DBService.GetCourse(Id);
            foreach (Instructor instructor in await DBService.GetAllInstructor())
            {
                Instructors.Add(instructor);
            }

            if (CourseAssessments == null)
            {
                CourseAssessments = new ObservableCollection<Assessment>();
            }

            //CurrentInstructor = await DBService.GetInstructor(CurrentCourse.InstructorId);

            //if (CurrentCourse.ObjectiveAssessmentId != null)
            //{
            //    CourseAssessments.Insert(0,
            //        await DBService.GetAssessment((int)CurrentCourse.ObjectiveAssessmentId));
            //}
            //if (CurrentCourse.PerformanceAssessmentId != null)
            //{
            //    CourseAssessments.Insert(1,
            //        await DBService.GetAssessment((int)CurrentCourse.PerformanceAssessmentId));
            //}

            //if (CurrentCourse.NoteId != null)
            //{
            //    CurrentNote = await DBService.GetNote((int)CurrentCourse.NoteId);
            //}
            //else
            //{
            //    CurrentNote = new Note();
            //}
        }

        private async Task LoadNew()
        {
            CurrentCourse = new Course();
            //await Task.Run(() => AttachedCourses = new ObservableCollection<Course>());
        }

        private async void ModifyAssessment(Assessment assessment)
        {
            throw new NotImplementedException();
        }

        private async void ModifyInstructor(Instructor instructor)
        {
            throw new NotImplementedException();
        }

        private async Task NewAssessment()
        {
            CurrentView = ViewType.AssessmentModification;
        }

        private async Task NewInstructor()
        {
            CurrentView = ViewType.InstructorModification;
        }

        private async void RemoveAssessment(Assessment assessment)
        {
            throw new NotImplementedException();
        }

        private async Task SelectInstructor()
        {
            CurrentInstructor = SelectedInstructor;
            SelectedInstructor = CurrentInstructor;
            IsInstructorView = true;
            IsInstructorSelection = false;
        }
    }
}