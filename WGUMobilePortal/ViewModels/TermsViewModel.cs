using System;
using System.Threading.Tasks;

using MvvmHelpers;
using MvvmHelpers.Commands;

using WGUMobilePortal.Services;

namespace WGUMobilePortal.ViewModels
{
    public class TermsViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Models.Term> Term { get; set; }
        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand AddCommand { get; }
        public AsyncCommand<Models.Term> RemoveCommand { get; }


        public TermsViewModel()
        {

            Title = "Terms View";

            Term = new ObservableRangeCollection<Models.Term>();

            RefreshCommand = new AsyncCommand(Refresh);
            AddCommand = new AsyncCommand(Add);
            RemoveCommand = new AsyncCommand<Models.Term>(Remove);

            Load();
        }

        async Task Add()
        {
            //string name = "Term 1";
            string name = await App.Current.MainPage.DisplayPromptAsync("Name", "Name of Term");
            //var startdate = await App.Current.MainPage.DisplayPromptAsync("StartDate", "First Day of Term");
            //var enddate = await App.Current.MainPage.DisplayPromptAsync("EndDate", "Last Day of Term");

            DateTime startdate = new DateTime(2020, 01, 01);
            DateTime enddate = new DateTime(2020, 06, 30);

            await DBService.AddTerm(name, startdate, enddate);
            await Refresh();
        }

        async Task Remove(Models.Term term)
        {
            await DBService.RemoveTerm(term.Id);
            await Refresh();
        }

        async Task Refresh()
        {
            IsBusy = true;

            //await Task.Delay(100);

            Term.Clear();

            var terms = await DBService.GetAllTerm();

            Term.AddRange(terms);

            IsBusy = false;
        }

        async void Load()
        {
            IsBusy = true;
            Term.Clear();
            var terms = await DBService.GetAllTerm();
            Term.AddRange(terms);
            IsBusy = false;
        }
    }
}