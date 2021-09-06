using System;
using System.Threading.Tasks;

using MvvmHelpers;
using MvvmHelpers.Commands;

using WGUMobilePortal.Models;
using WGUMobilePortal.Services;
using WGUMobilePortal.Views;

using Xamarin.Forms;

namespace WGUMobilePortal.ViewModels
{
    public class TermsViewModel : BaseViewModel
    {
        private Term _newTerm;
        private string _newTermName;
        private DateTime _newTermStart = DateTime.Today;
        private DateTime _newTermEnd = DateTime.Today.AddMonths(6);

        public ObservableRangeCollection<Term> Term { get; set; }
        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand AddCommand { get; }

        public AsyncCommand<Term> EditCommand { get; }
        public AsyncCommand<Term> RemoveCommand { get; }

        public AsyncCommand AddNewCommand { get; }
        public AsyncCommand CancelCommand { get; }

        public Term NewTerm
        {
            get => _newTerm;
            set
            {
                if (value != _newTerm)
                {
                    SetProperty(ref _newTerm, value);
                    OnPropertyChanged();
                }
            }
        }

        public string NewTermName
        {
            get => _newTermName;
            set
            {
                if (value != _newTermName)
                {
                    SetProperty(ref _newTermName, value);
                    OnPropertyChanged();
                }
            }
        }

        public DateTime NewTermStart
        {
            get => _newTermStart;
            set
            {
                if (value != _newTermStart)
                {
                    SetProperty(ref _newTermStart, value);
                    OnPropertyChanged();
                }
            }
        }

        public DateTime NewTermEnd
        {
            get => _newTermEnd;
            set
            {
                if (value != _newTermEnd)
                {
                    SetProperty(ref _newTermEnd, value);
                    OnPropertyChanged();
                }
            }
        }

        public TermsViewModel()
        {

            Title = "Terms View";

            Term = new ObservableRangeCollection<Term>();

            RefreshCommand = new AsyncCommand(Refresh);
            AddCommand = new AsyncCommand(Add);
            EditCommand = new AsyncCommand<Term>(Edit);
            RemoveCommand = new AsyncCommand<Term>(Remove);
            AddNewCommand = new AsyncCommand(SaveNewTerm);
            CancelCommand = new AsyncCommand(Cancel);

            Load();
        }

        async Task Add()
        {

            //string name = await App.Current.MainPage.DisplayPromptAsync("Name", "Name of Term");
            await App.Current.MainPage.Navigation.PushModalAsync(AddEditTermPage());
            //var startdate = await App.Current.MainPage.DisplayPromptAsync("StartDate", "First Day of Term");
            //var enddate = await App.Current.MainPage.DisplayPromptAsync("EndDate", "Last Day of Term");

            //DateTime startdate = new DateTime(2020, 01, 01);
            //DateTime enddate = new DateTime(2020, 06, 30);

            //await DBService.AddTerm(name, startdate, enddate);
            await Refresh();
        }

        async Task Edit(Term term)
        {
            await DBService.EditTerm(term);
            await Refresh();
        }

        async Task Remove(Term term)
        {
            await DBService.RemoveTerm(term.Id);
            await Refresh();
        }

        public async Task Refresh()
        {
            IsBusy = true;

            //await Task.Delay(100);

            Term.Clear();

            System.Collections.Generic.IEnumerable<Term> terms = await DBService.GetAllTerms();

            Term.AddRange(terms);

            IsBusy = false;
        }

        async void Load()
        {
            IsBusy = true;
            Term.Clear();
            System.Collections.Generic.IEnumerable<Term> terms = await DBService.GetAllTerms();
            Term.AddRange(terms);
            IsBusy = false;
        }

        public static Page AddEditTermPage()
        {
            return new NavigationPage(new AddEditTermPage());
        }

        async Task SaveNewTerm()
        {
            await DBService.AddTerm(
                new Term
                {
                    Name = NewTermName,
                    StartDate = NewTermStart,
                    EndDate = NewTermEnd
                }
            );
            //await DBService.AddTerm(NewTermName, NewTermStart, NewTermEnd);
            await Refresh();
            NewTerm = null;
            await App.Current.MainPage.Navigation.PopModalAsync();
            await Refresh();
        }

        async Task Cancel()
        {
            await App.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}