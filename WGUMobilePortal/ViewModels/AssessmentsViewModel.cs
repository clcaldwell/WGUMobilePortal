using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using WGUMobilePortal.Services;
using WGUMobilePortal.Views;
using WGUMobilePortal.Models;

using Xamarin.Forms;

namespace WGUMobilePortal.ViewModels
{
    public class AssessmentsViewModel : BaseViewModel
    {
        public AssessmentsViewModel()
        {
            Title = "Assessments View";

            Assessments = new ObservableCollection<Assessment>();

            RefreshCommand = new Command(async () => await Refresh());
            AddCommand = new Command(async () => await Add());
            RemoveCommand = new Command<Assessment>(Remove);
            ModifyCommand = new Command<Assessment>(Modify);

            _ = Load();
        }

        public Command AddCommand { get; }
        public ObservableCollection<Assessment> Assessments { get; set; }
        public Command<Assessment> ModifyCommand { get; }
        public Command RefreshCommand { get; }
        public Command<Assessment> RemoveCommand { get; }

        public async Task OnAppearing()
        {
            await Load();
        }

        private async Task Add()
        {
            await AppShell.Current.GoToAsync($"{nameof(ModifyAssessmentsPage)}?id=0");
            //await Task.Run(() => Refresh());
        }

        private async Task Load()
        {
            IsBusy = true;
            Assessments.Clear();
            var assessments = await DBService.GetAllAssessment();
            foreach (Assessment assessment in assessments)
            {
                Assessments.Add(assessment);
            }
            IsBusy = false;
        }

        private async void Modify(Assessment assessment)
        {
            await AppShell.Current.GoToAsync($"{nameof(ModifyAssessmentsPage)}?id={assessment.Id}");
            //await AppShell.Current.GoToAsync($"{nameof(ModifyAssessmentsPage)}?id={assessment.Id}&name={assessment.Name}&startDate={assessment.StartDate}&endDate={assessment.EndDate}&style={assessment.Style}");
        }

        private async Task Refresh()
        {
            IsBusy = true;

            Assessments.Clear();

            var assessments = await DBService.GetAllAssessment();

            foreach (Assessment assessment in assessments)
            {
                Assessments.Add(assessment);
            }

            IsBusy = false;
        }

        private async void Remove(Assessment assessment)
        {
            await DBService.RemoveAssessment(assessment.Id);
            await Task.Run(() => Refresh());
        }
    }
}