using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using WGUMobilePortal.Services;

using Xamarin.Forms;

namespace WGUMobilePortal.ViewModels
{
    public class AssessmentsViewModel : BaseViewModel
    {
        public AssessmentsViewModel()
        {
            Title = "Assessments View";

            Assessment = new ObservableCollection<Models.Assessment>();

            RefreshCommand = new Command(async () => await Refresh());
            AddCommand = new Command(async () => await Add());
            RemoveCommand = new Command<Models.Assessment>(Remove);
            //ModifyCommand = new Command<Models.Assessment>();

            _ = Load();
        }

        public Command AddCommand { get; }
        public ObservableCollection<Models.Assessment> Assessment { get; set; }
        public Command<Models.Assessment> ModifyCommand { get; }
        public Command RefreshCommand { get; }
        public Command<Models.Assessment> RemoveCommand { get; }

        public async Task OnAppearing()
        {
            await Load();
        }

        private async Task Add()
        {
            string name = await App.Current.MainPage.DisplayPromptAsync("Name", "Name of Assessment");

            DateTime startdate = new DateTime(2022, 06, 01);
            DateTime enddate = new DateTime(2022, 12, 30);
            const Models.AssessmentStyle style = Models.AssessmentStyle.Objective;

            await DBService.AddAssessment(name, startdate, enddate, style);
            await Task.Run(() => Refresh());
        }

        private async Task Load()
        {
            IsBusy = true;
            Assessment.Clear();
            var assessments = await DBService.GetAllAssessment();
            foreach (Models.Assessment assessment in assessments)
            {
                Assessment.Add(assessment);
            }
            IsBusy = false;
        }

        private async Task Refresh()
        {
            IsBusy = true;

            Assessment.Clear();

            var assessments = await DBService.GetAllAssessment();

            foreach (Models.Assessment assessment in assessments)
            {
                Assessment.Add(assessment);
            }

            IsBusy = false;
        }

        private async void Remove(Models.Assessment assessment)
        {
            await DBService.RemoveAssessment(assessment.Id);
            await Task.Run(() => Refresh());
        }
    }
}