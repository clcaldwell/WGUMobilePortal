using System;
using System.Threading.Tasks;

using MvvmHelpers;
using MvvmHelpers.Commands;

using WGUMobilePortal.Services;

namespace WGUMobilePortal.ViewModels
{
    public class AssessmentsViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Models.Assessment> Assessment { get; set; }
        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand AddCommand { get; }
        public AsyncCommand<Models.Assessment> RemoveCommand { get; }

        public AssessmentsViewModel()
        {

            Title = "Assessments View";

            Assessment = new ObservableRangeCollection<Models.Assessment>();

            RefreshCommand = new AsyncCommand(Refresh);
            AddCommand = new AsyncCommand(Add);
            RemoveCommand = new AsyncCommand<Models.Assessment>(Remove);

            Load();
        }

        async Task Add()
        {

            string name = await App.Current.MainPage.DisplayPromptAsync("Name", "Name of Assessment");

            DateTime startdate = new DateTime(2020, 01, 01);
            DateTime enddate = new DateTime(2020, 06, 30);

            await DBService.AddAssessment(name, startdate, enddate);
            await Refresh();
        }

        async Task Remove(Models.Assessment term)
        {
            await DBService.RemoveAssessment(term.Id);
            await Refresh();
        }

        async Task Refresh()
        {
            IsBusy = true;

            Assessment.Clear();

            var assessments = await DBService.GetAllAssessment();

            Assessment.AddRange(assessments);

            IsBusy = false;
        }

        async void Load()
        {
            IsBusy = true;
            Assessment.Clear();
            var assessments = await DBService.GetAllAssessment();
            Assessment.AddRange(assessments);
            IsBusy = false;
        }
    }
}