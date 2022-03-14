using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        public Command<Models.Term> SaveCommand { get; }
        public Command<Models.Term> DeleteCommand { get; }
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
        public int Id { get => _id; set => SetProperty(ref _id, value); }
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
        public DateTime StartDate { get => _startDate; set => SetProperty(ref _startDate, value); }
        public DateTime EndDate { get => _endDate; set => SetProperty(ref _endDate, value); }
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
        private Term _term;
        private Course _selectedAttachCourse;
        private Course _selectedCourse;
        private ObservableCollection<Course> _attachedCourses;
        private ObservableCollection<Course> _courseSelectionList;


        public ModifyTermsViewModel()
        {
            Title = "Add/Modify Terms Page";
            DeleteCommand = new Command<Models.Term>(Delete);
            SaveCommand = new Command<Models.Term>(Save);
            RemoveCourseCommand = new Command(RemoveCourse);
            OpenCourseSelectionCommand = new Command(OpenCourseSelection);
            CancelCourseSelectionCommand = new Command(CancelCourseSelection);
            SelectCourseCommand = new Command(SelectCourse);
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

                Load(Id);
            }

        }

        async void Save(Models.Term term)
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
        async void Delete(Models.Term term)
        {
            if (await Shell.Current.DisplayAlert("Confirm Deletion", $"Are you sure you want to delete {term.Name}?", "Delete", "Cancel"))
            {
                await DBService.RemoveTerm(term.Id);
                await Shell.Current.GoToAsync("..");
            }
        }

        void SelectCourse()
        {
            AttachedCourses.Add(SelectedAttachCourse);
            CloseCourseSelection();
        }
        void RemoveCourse()
        {
            AttachedCourses.Remove(SelectedCourse);
        }
        async void OpenCourseSelection()
        {
            IsModifyTerm = false;
            IsCourseSelection = true;

            List<Course> courseList = (List<Course>)await DBService.GetAllCourse();

            if (Term.CourseId != null)
            {
                Term.CourseId.ForEach(
                currentTermCourseID => courseList.RemoveAt(
                    courseList.FindIndex(
                        course => course.Id == currentTermCourseID)));
            }

            CourseSelectionList = new ObservableCollection<Course>(courseList);
        }
        void CloseCourseSelection()
        {
            IsCourseSelection = false;
            IsModifyTerm = true;
            CourseSelectionList = null;
        }
        void CancelCourseSelection()
        {
            CloseCourseSelection();
        }
        async void Load(int Id)
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
        void LoadNew()
        {
            Term = new Term();
            AttachedCourses = new ObservableCollection<Course>();
        }

    }
}