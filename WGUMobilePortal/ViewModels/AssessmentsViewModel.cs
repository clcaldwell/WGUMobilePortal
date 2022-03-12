using System;
using System.Collections.ObjectModel;

using WGUMobilePortal.Services;

using Xamarin.Forms;

namespace WGUMobilePortal.ViewModels
{
    public class AssessmentsViewModel : BaseViewModel
    {
        public ObservableCollection<Models.Assessment> Assessment { get; set; }
        public Command RefreshCommand { get; }
        public Command AddCommand { get; }
        public Command<Models.Assessment> RemoveCommand { get; }
        public Command<Models.Assessment> ModifyCommand { get; }

        public AssessmentsViewModel()
        {

            Title = "Assessments View";

            Assessment = new ObservableCollection<Models.Assessment>();

            RefreshCommand = new Command(Refresh);
            AddCommand = new Command(Add);
            RemoveCommand = new Command<Models.Assessment>(Remove);
            //ModifyCommand = new Command<Models.Assessment>();

            Load();
        }

        async void Add()
        {

            string name = await App.Current.MainPage.DisplayPromptAsync("Name", "Name of Assessment");

            DateTime startdate = new DateTime(2020, 01, 01);
            DateTime enddate = new DateTime(2020, 06, 30);
            Models.AssessmentStyle style = Models.AssessmentStyle.Objective;

            await DBService.AddAssessment(name, startdate, enddate, style);
            Refresh();
        }

        async void Remove(Models.Assessment assessment)
        {
            await DBService.RemoveAssessment(assessment.Id);
            Refresh();
        }

        //aysnc async void Modify(Models.Assessment assessment)
        //{
            //NavigateToTerms = new Command(async () =>
            //await var termsPage = new Views.TermsPage();
            //await AppShell.Current.Navigation.PushAsync(new Views.TermsPage()));

        //}

        async void Refresh()
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

        async void Load()
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
    }
}