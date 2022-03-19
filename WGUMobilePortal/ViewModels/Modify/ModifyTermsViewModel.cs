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
        public Command AddCommand { get; }
        public Command ModifyCommand { get; }
        public Command<Term> SaveCommand { get; }
        public Command<Term> DeleteCommand { get; }
        public Command RemoveCourseCommand { get; }
        public Command OpenCourseSelectionCommand { get; }
        public Command CancelCourseSelectionCommand { get; }
        public Command SelectCourseCommand { get; }


        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
                OnPropertyChanged(nameof(Name));
            }
        }
        public int Id
        { 
            get => _id;
            set => SetProperty(ref _id, value);
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
        //public DateTime StartDate { get => _startDate; set =>  SetProperty(ref _startDate, value);
        //        OnPropertyChanged(nameof(StartDate));
        //    }
        //}
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        //public DateTime EndDate
        //{
        //    get => _endDate;
        //    set
        //    {
        //        SetProperty(ref _endDate, value);
        //        OnPropertyChanged(nameof(EndDate));
        //    }
        //}
        public DateTime StartDateMinimum => DateTime.Today.AddDays(-60).Date;
        public DateTime EndDateMinimum => StartDate.AddDays(1).Date;   
        //{
        //    get => _endDateMinimum;
        //    set
        //    {
        //        SetProperty(ref _endDateMinimum, value);
        //        OnPropertyChanged(nameof(EndDateMinimum));
        //    }
        //}
        public Term Term { get => _term; set => SetProperty(ref _term, value); }
        public Course SelectedCourse
        {
            get => _selectedCourse;
            set
            {
                SetProperty(ref _selectedCourse, value);
                OnPropertyChanged(nameof(SelectedCourse));
            }
        }
        public Course SelectedAttachCourse
        {
            get => _selectedAttachCourse;
            set
            {
                SetProperty(ref _selectedAttachCourse, value);
                OnPropertyChanged(nameof(SelectedAttachCourse));
            }
        }
        public ObservableCollection<Course> CourseSelectionList
        {
            get => _courseSelectionList;
            set
            {
                SetProperty(ref _courseSelectionList, value);
                OnPropertyChanged(nameof(CourseSelectionList));
            }
        }
        public ObservableCollection<Course> AttachedCourses
        {
            get => _attachedCourses;
            set
            {
                SetProperty(ref _attachedCourses, value);
                OnPropertyChanged(nameof(AttachedCourses));
            }
        }


        private string _name;
        private int _id;
        private bool _isCourseSelection;
        private bool _isModifyTerm;
        private DateTime _startDate;
        private DateTime _endDate;
        //private DateTime _endDateMinimum;  
        private Term _term;
        private Course _selectedAttachCourse;
        private Course _selectedCourse;
        private ObservableCollection<Course> _attachedCourses;
        private ObservableCollection<Course> _courseSelectionList;


        public ModifyTermsViewModel()
        {
            Title = "Add/Modify Terms Page";
            DeleteCommand = new Command<Term>(Delete);
            SaveCommand = new Command<Term>(Save);
            RemoveCourseCommand = new Command(async () => await RemoveCourse());
            OpenCourseSelectionCommand = new Command(async () => await OpenCourseSelection());
            CancelCourseSelectionCommand = new Command(async () => await CancelCourseSelection());
            SelectCourseCommand = new Command(async () => await SelectCourse());
            IsModifyTerm = true;
            IsCourseSelection = false;
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if (string.IsNullOrWhiteSpace(query["id"])) // Will be null for new Term requests
            {
                LoadNew();
            }
            else
            {
                Name = HttpUtility.UrlDecode(query["name"]);
                StartDate = DateTime.Parse(HttpUtility.UrlDecode(query["startDate"]));
                EndDate = DateTime.Parse(HttpUtility.UrlDecode(query["endDate"]));
                Id = int.Parse(HttpUtility.UrlDecode(query["id"]));
                //_ = StartDateMinimum;
                //_ = EndDateMinimum;

                Load(Id);
            }

        }

        public async void Save(Term term)
        {

            term.Name = Name;
            term.StartDate = StartDate;
            term.EndDate = EndDate;

            List<int> Courses = AttachedCourses.Select(x => x.Id).ToList();
            Courses.Sort();
            term.CourseId = Courses;

            if (term.Id == 0)
            {
                await DBService.AddTerm(term);
            }
            else
            {
                await DBService.EditTerm(term);
            }

            await Shell.Current.Navigation.PopAsync();
            //await Shell.Current.GoToAsync("..");
        }
        async void Delete(Term term)
        {
            if (await Shell.Current.DisplayAlert("Confirm Deletion", $"Are you sure you want to delete {term.Name}?", "Delete", "Cancel"))
            {
                await DBService.RemoveTerm(term.Id);
                await Shell.Current.GoToAsync("..");
            }
        }

        async Task SelectCourse()
        {
            AttachedCourses.Add(SelectedAttachCourse);
            CloseCourseSelection();
        }
        async Task RemoveCourse()
        {
            AttachedCourses.Remove(SelectedCourse);
        }
        async Task OpenCourseSelection()
        {
            if (AttachedCourses.Count >= 7)
            {
                return;
            }
            IsModifyTerm = false;
            IsCourseSelection = true;

            List<Course> courseList = (List<Course>)await DBService.GetAllCourse();

            //if (Term.CourseId != null)
            //{
            //Term.CourseId.ForEach(
            //currentTermCourseID => courseList.RemoveAt(
            //courseList.FindIndex(
            //course => course.Id == currentTermCourseID)));
            //}
            courseList = courseList.Where(course => course.TermId is null).ToList();

            CourseSelectionList = new ObservableCollection<Course>(courseList);
        }
        async Task CloseCourseSelection()
        {
            IsCourseSelection = false;
            IsModifyTerm = true;
            CourseSelectionList = null;
        }
        async Task CancelCourseSelection()
        {
            CloseCourseSelection();
        }
        async Task Load(int Id)
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
        async Task LoadNew()
        {
            Term = new Term();
            AttachedCourses = new ObservableCollection<Course>();
        }

    }
}