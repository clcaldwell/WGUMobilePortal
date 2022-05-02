using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using WGUMobilePortal.Models;
using WGUMobilePortal.Services;

using Xamarin.Forms;

namespace WGUMobilePortal.ViewModels
{
    public class ModifyAssessmentsViewModel : BaseViewModel, IQueryAttributable
    {
        private Assessment _assessment;
        private DateTime _dueDate;
        private bool _dueDateShouldNotify;
        private int _id;
        private string _name;
        private AssessmentStyle _style;

        public ModifyAssessmentsViewModel()
        {
            Title = "Add/Modify Assessments Page";
            DeleteCommand = new Command(async () => await Delete());
            SaveCommand = new Command(async () => await Save());
        }

        public Command AddCommand { get; }

        public Assessment Assessment
        {
            get => _assessment;
            set
            {
                SetProperty(ref _assessment, value);
                OnPropertyChanged(nameof(Assessment));
            }
        }

        public IEnumerable<AssessmentStyle> AssessmentStyle
        {
            get
            {
                return Enum.GetValues(typeof(AssessmentStyle))
                    .Cast<AssessmentStyle>();
            }
        }

        public Command DeleteCommand { get; }

        public DateTime DueDate
        {
            get => _dueDate;
            set
            {
                SetProperty(ref _dueDate, value);
                OnPropertyChanged(nameof(DueDate));
            }
        }

        public bool DueDateShouldNotify
        {
            get => _dueDateShouldNotify;
            set
            {
                SetProperty(ref _dueDateShouldNotify, value);
                OnPropertyChanged(nameof(DueDateShouldNotify));
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

        public DateTime MinimumDueDate => DateTime.Today.AddMonths(-1);
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

        public Command SaveCommand { get; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Marking as static causes program to crash," +
            " presumably because the UI requires INotifyPropertyChanged")]
        public AssessmentStyle Style
        {
            get => _style;
            set
            {
                SetProperty(ref _style, value);
                OnPropertyChanged(nameof(Style));
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if (String.IsNullOrWhiteSpace(query["id"]))
            {
                Task.Run(async () => await LoadNew());
            }
            else
            {
                Id = int.Parse(HttpUtility.UrlDecode(query["id"]));

                Task.Run(async () => await Load(Id));
            }
        }

        public async Task Save()
        {
            Assessment.Name = Name;
            Assessment.DueDate = DueDate;
            Assessment.DueDateShouldNotify = DueDateShouldNotify;
            Assessment.Style = Style;

            if (!await ValidateAssessment(Assessment))
            {
                return;
            }

            if (Assessment.Id == 0)
            {
                await DBService.AddAssessment(Assessment);
            }
            else
            {
                await DBService.EditAssessment(Assessment);
            }

            await Shell.Current.Navigation.PopAsync();
        }

        public async Task<bool> ValidateAssessment(Assessment assessment)
        {
            // Null Checks
            if (string.IsNullOrWhiteSpace(assessment.Name))
            {
                await Shell.Current.DisplayAlert("Alert", "Unable to save, must specify a Course Name", "OK");
                return false;
            }

            // Date checks
            if (assessment.DueDate == null)
            {
                await Shell.Current.DisplayAlert("Alert", "Unable to save, Due Date must have a value", "OK");
                return false;
            }

            return true;
        }

        private async Task Delete()
        {
            if (await Shell.Current.DisplayAlert("Confirm Deletion", $"Are you sure you want to delete {Assessment.Name}?", "Delete", "Cancel"))
            {
                await DBService.RemoveAssessment(Assessment.Id);
                await Shell.Current.GoToAsync("..");
            }
        }

        private async Task Load(int Id)
        {
            Assessment = await DBService.GetAssessment(Id);
            Name = Assessment.Name;
            DueDate = Assessment.DueDate;
            DueDateShouldNotify = Assessment.DueDateShouldNotify;
            Style = Assessment.Style;
        }

        private async Task LoadNew()
        {
            Assessment = new Assessment();
            DueDate = DateTime.Today;
        }
    }
}