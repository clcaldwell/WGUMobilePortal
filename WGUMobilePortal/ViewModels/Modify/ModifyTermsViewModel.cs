using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
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
        public Command<Models.Term> RemoveCommand { get; }
        public Command RemoveCourseCommand { get; }
        public Command OpenCourseSelectionCommand { get; }
        public Command CancelCourseSelectionCommand { get; }
        public Command SelectCourseCommand { get; }

        public Term Term { get => _term; set => SetProperty(ref _term, value); }
        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
                OnPropertyChanged(nameof(Name));
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

        public ObservableCollection<Course> CourseSelectionList
        {
            get => _courseSelectionList;
            set
            {
                SetProperty(ref _courseSelectionList, value);
                OnPropertyChanged(nameof(CourseSelectionList));
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

        public bool IsCourseSelection {
            get => _isCourseSelection;
            set
            {
                SetProperty(ref _isCourseSelection, value);
                OnPropertyChanged(nameof(IsCourseSelection));
            }
        }
        private bool _isCourseSelection;

        public bool IsModifyTerm
        {
            get => _isModifyTerm;
            set
            {
                SetProperty(ref _isModifyTerm, value);
                OnPropertyChanged(nameof(IsModifyTerm));
            }
        }
        private bool _isModifyTerm;

        private Course _selectedCourse;
        public DateTime StartDate { get => _startDate; set => SetProperty(ref _startDate, value); }
        public DateTime EndDate { get => _endDate; set => SetProperty(ref _endDate, value); }
       
        public int Id { get => _id; set => SetProperty(ref _id, value); }

        private Term _term;
        private string _name;
        private DateTime _startDate;
        private DateTime _endDate;
        private int _id;

        public ModifyTermsViewModel()
        {
            Title = "Add/Modify Terms Page";
            RemoveCommand = new Command<Models.Term>(Remove);
            SaveCommand = new Command<Models.Term>(Save);
            RemoveCourseCommand = new Command(RemoveCourse);
            OpenCourseSelectionCommand = new Command(OpenCourseSelection);
            CancelCourseSelectionCommand = new Command(CancelCourseSelection);
            SelectCourseCommand = new Command(SelectCourse);
            IsModifyTerm = true;
            IsCourseSelection = false;
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
        private ObservableCollection<Course> _attachedCourses;
        private Course _selectedAttachCourse;
        private ObservableCollection<Course> _courseSelectionList;

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {

            Name = HttpUtility.UrlDecode(query["name"]);
            StartDate = DateTime.Parse(HttpUtility.UrlDecode(query["startDate"]));
            EndDate = DateTime.Parse(HttpUtility.UrlDecode(query["endDate"]));
            Id = int.Parse(HttpUtility.UrlDecode(query["id"]));

            Load(Id);
        }

        async void Remove(Models.Term term)
        {
            if (await Shell.Current.DisplayAlert("Confirm Deletion", $"Are you sure you want to delete {term.Name}?", "Delete", "Cancel")) {
                await DBService.RemoveTerm(term.Id);
                await Shell.Current.GoToAsync("..");
            }
        }

        async void Save(Models.Term term)
        {
            //term.Id = Id;
            term.Name = Name;
            term.StartDate = StartDate;
            term.EndDate = EndDate;

            List<int> Courses = AttachedCourses.Select(x => x.Id).ToList();
            Courses.Sort();
            term.CourseId = Courses;
            
            await DBService.EditTerm(term);

            await Shell.Current.GoToAsync("..");
        }

        async void RemoveCourse()
        {
            AttachedCourses.Remove(SelectedCourse);
        }
        async void OpenCourseSelection()
        {
            IsModifyTerm = false;
            IsCourseSelection = true;

            List<Course> courseList = (List<Course>)await DBService.GetAllCourse();

            Term.CourseId.ForEach(
                currentTermCourseID => courseList.RemoveAt(
                    courseList.FindIndex(
                        course => course.Id == currentTermCourseID)));

            CourseSelectionList = new ObservableCollection<Course>(courseList);
        }

        async void CloseCourseSelection()
        {
            IsCourseSelection = false;
            IsModifyTerm = true;
            CourseSelectionList = null;
        }

        async void CancelCourseSelection()
        {
            CloseCourseSelection();
        }

        async void SelectCourse()
        {
            AttachedCourses.Add(SelectedAttachCourse);
            CloseCourseSelection();
        }

        //async void Remove(int Id)
        //{
        //if (await Shell.Current.DisplayAlert("Confirm Deletion", $"Are you sure you want to delete {Id}?", "Delete", "Cancel"))
        //{
        //await DBService.RemoveTerm(Id);
        //}
        //}

        async void Load(int Id)
        {

            //int.TryParse(strId, out intId);

            Term = await DBService.GetTerm(Id);

            //var newlist = new List<Course>();

            if (AttachedCourses == null)
            {
                AttachedCourses = new ObservableCollection<Course>();
            }

            foreach (int courseId in Term.CourseId)
            {
                AttachedCourses.Add(await DBService.GetCourse(courseId));
            }
            //var convert = Term.CourseId.ConvertAll(x => DBService.GetCourse(x)).AsEnumerable();
            //var list = (IEnumerable<Course>)convert;

            //AttachedCourses = new ObservableCollection<Course>(newlist);
            //AttachedCourses.Add

            //AttachedCourses = new ObservableCollection<Course>(
            // (IEnumerable<Course>)Term.CourseId.ConvertAll(async x => await DBService.GetCourse(x))
            //);

            //Id = term.Id;

        }



        //async void Modify(Models.Term term)
        //{
        //var termsModifyPage = new Views.ModifyTermsPage();
        // termsModifyPage.BindingContext = term;
        //await AppShell.Current.Navigation.PushAsync(termsModifyPage);
        //}
    }
}