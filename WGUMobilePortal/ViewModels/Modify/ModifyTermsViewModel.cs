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
    public class ModifyTermsViewModel : BaseViewModel, IQueryAttributable
    {
        private ObservableCollection<Course> _attachedCourses;
        private ObservableCollection<Course> _courseSelectionList;
        private DateTime _endDate;
        private DateTime _endDateMinimum;
        private int _id;
        private bool _isCourseSelection;
        private bool _isModifyTerm;
        private string _name;
        private Course _selectedAttachCourse;
        private Course _selectedCourse;
        private DateTime _startDate;
        private Term _term;

        public ModifyTermsViewModel()
        {
            Title = "Add/Modify Terms Page";
            DeleteCommand = new Command<Term>(Delete);
            SaveCommand = new Command(async () => await Save());
            RemoveCourseCommand = new Command(async () => await RemoveCourse());
            OpenCourseSelectionCommand = new Command(async () => await OpenCourseSelection());
            CancelCourseSelectionCommand = new Command(async () => await CancelCourseSelection());
            SelectCourseCommand = new Command(async () => await SelectCourse());
        }

        public Command AddCommand { get; }

        public ObservableCollection<Course> AttachedCourses
        {
            get => _attachedCourses;
            set
            {
                SetProperty(ref _attachedCourses, value);
                OnPropertyChanged(nameof(AttachedCourses));
            }
        }

        public Command CancelCourseSelectionCommand { get; }

        public ObservableCollection<Course> CourseSelectionList
        {
            get => _courseSelectionList;
            set
            {
                SetProperty(ref _courseSelectionList, value);
                OnPropertyChanged(nameof(CourseSelectionList));
            }
        }

        public Command<Term> DeleteCommand { get; }

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

        public int Id
        {
            get => _id;
            set
            {
                SetProperty(ref _id, value);
                OnPropertyChanged(nameof(Id));
            }
        }

        public bool IsCourseSelection
        {
            get => _isCourseSelection;
            set
            {
                SetProperty(ref _isCourseSelection, value);
                OnPropertyChanged(nameof(IsCourseSelection));
            }
        }

        public bool IsModifyTerm
        {
            get => _isModifyTerm;
            set
            {
                SetProperty(ref _isModifyTerm, value);
                OnPropertyChanged(nameof(IsModifyTerm));
            }
        }

        public Command ModifyCommand { get; }

        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
                OnPropertyChanged(nameof(Name));
            }
        }

        public Command OpenCourseSelectionCommand { get; }
        public Command RemoveCourseCommand { get; }
        public Command SaveCommand { get; }
        public Command SelectCourseCommand { get; }

        public Course SelectedAttachCourse
        {
            get => _selectedAttachCourse;
            set
            {
                SetProperty(ref _selectedAttachCourse, value);
                OnPropertyChanged(nameof(SelectedAttachCourse));
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

        public DateTime StartDate
        {
            get => _startDate; set
            {
                SetProperty(ref _startDate, value);
                OnPropertyChanged(nameof(StartDate));
                EndDateMinimum = value.AddDays(1);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Marking as static causes program to crash," +
            " presumably because the UI requires INotifyPropertyChanged")]
        public DateTime StartDateMinimum => DateTime.Today.AddDays(-60).Date;

        public Term Term
        {
            get => _term;
            set
            {
                SetProperty(ref _term, value);
                OnPropertyChanged(nameof(Term));
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if (string.IsNullOrWhiteSpace(query["id"])) // Will be null for new Term requests
            {
                Task.Run(async () => await LoadNew());
            }
            else
            {
                Name = HttpUtility.UrlDecode(query["name"]);
                StartDate = DateTime.Parse(HttpUtility.UrlDecode(query["startDate"]));
                EndDate = DateTime.Parse(HttpUtility.UrlDecode(query["endDate"]));
                Id = int.Parse(HttpUtility.UrlDecode(query["id"]));

                Task.Run(async () => await Load(Id));
            }
        }

        public async Task OnAppearing()
        {
            IsModifyTerm = true;
            IsCourseSelection = false;
        }

        public async Task Save()
        {
            Term.Name = Name;
            Term.StartDate = StartDate;
            Term.EndDate = EndDate;

            List<int> Courses = AttachedCourses.Select(x => x.Id).ToList();
            Courses.Sort();
            Term.CourseId = Courses;

            if (!await ValidateTerm(Term))
            {
                return;
            }

            if (Term.Id == 0)
            {
                await DBService.AddTerm(Term);
            }
            else
            {
                await DBService.EditTerm(Term);
            }

            await Shell.Current.Navigation.PopAsync();
        }

        public async Task<bool> ValidateTerm(Term term)
        {
            // Null Checks
            if (string.IsNullOrWhiteSpace(term.Name))
            {
                await Shell.Current.DisplayAlert("Alert", "Unable to save, must specify a Course Name", "OK");
                return false;
            }

            // Date checks
            if (term.StartDate.Date >= term.EndDate.Date)
            {
                await Shell.Current.DisplayAlert("Alert", "Unable to save, End date must be after Start date", "OK");
                return false;
            }

            // Course Count check
            if (term.CourseId.Count > 6)
            {
                await Shell.Current.DisplayAlert("Alert", "Unable to save, Term can only hold 6 courses", "OK");
                return false;
            }

            return true;
        }

        private async Task CancelCourseSelection()
        {
            await CloseCourseSelection();
        }

        private async Task CloseCourseSelection()
        {
            IsCourseSelection = false;
            IsModifyTerm = true;
            CourseSelectionList = null;
        }

        private async void Delete(Term term)
        {
            if (await Shell.Current.DisplayAlert("Confirm Deletion", $"Are you sure you want to delete {term.Name}?", "Delete", "Cancel"))
            {
                await DBService.RemoveTerm(term.Id);
                await Shell.Current.GoToAsync("..");
            }
        }

        private async Task Load(int Id)
        {
            Term = await DBService.GetTerm(Id);

            if (AttachedCourses == null)
            {
                AttachedCourses = new ObservableCollection<Course>();
            }

            if (Term.CourseId != null)
            {
                foreach (int courseId in Term.CourseId)
                {
                    AttachedCourses.Add(await DBService.GetCourse(courseId));
                }
            }
        }

        private async Task LoadNew()
        {
            Term = new Term();
            await Task.Run(() => AttachedCourses = new ObservableCollection<Course>());
            StartDate = DateTime.Today;
            EndDate = DateTime.Today.AddDays(1);
        }

        private async Task OpenCourseSelection()
        {
            if (AttachedCourses.Count >= 7)
            {
                return;
            }
            IsModifyTerm = false;
            IsCourseSelection = true;

            List<Course> courseList = (List<Course>)await DBService.GetAllCourse();
            courseList = courseList.Where(course => course.TermId == 0).ToList();
            CourseSelectionList = new ObservableCollection<Course>(courseList);
        }

        private async Task RemoveCourse()
        {
            await Task.Run(() => AttachedCourses.Remove(SelectedCourse));
        }

        private async Task SelectCourse()
        {
            AttachedCourses.Add(SelectedAttachCourse);
            await CloseCourseSelection();
        }
    }
}