using System;
using System.Collections.ObjectModel;

using WGUMobilePortal.Services;
using WGUMobilePortal.Views;

using Xamarin.Forms;

namespace WGUMobilePortal.ViewModels
{
    public class TermsViewModel : BaseViewModel
    {
        public ObservableCollection<Models.Term> Term { get; set; }
        //public Models.Term SelectedTerm
        //{
            //get => _selectedTerm;
            //set
            //{
                //if (value != _selectedTerm)
                //{
                    //SetProperty(ref _selectedTerm, value);
                    //OnPropertyChanged();
                //}
            //}
        //}
        public Command RefreshCommand { get; }
        public Command AddCommand { get; }
        public Command<Models.Term> RemoveCommand { get; }
        public Command ModifyCommand { get; }

        //private Models.Term _selectedTerm;


        public TermsViewModel()
        {

            Title = "Terms View";

            Term = new ObservableCollection<Models.Term>();

            RefreshCommand = new Command(Refresh);
            AddCommand = new Command(Add);
            RemoveCommand = new Command<Models.Term>(Remove);
            ModifyCommand = new Command<Models.Term>(Modify);

            Load();
        }


            async void Add()
        {
            //string name = "Term 1";
            string name = await App.Current.MainPage.DisplayPromptAsync("Name", "Name of Term");
            //var startdate = await App.Current.MainPage.DisplayPromptAsync("StartDate", "First Day of Term");
            //var enddate = await App.Current.MainPage.DisplayPromptAsync("EndDate", "Last Day of Term");

            DateTime startdate = new DateTime(2020, 01, 01);
            DateTime enddate = new DateTime(2020, 06, 30);

            await DBService.AddTerm(name, startdate, enddate);
            Refresh();
        }

        async void Remove(Models.Term term)
        {
            if (await Shell.Current.DisplayAlert("Confirm Deletion", $"Are you sure you want to delete {term.Name}", "Delete", "Cancel"))
            {
                await DBService.RemoveTerm(term.Id);
                Refresh();
            }
        }

        async void Refresh()
        {
            IsBusy = true;

            //await Task.Delay(100);

            Term.Clear();

            var terms = await DBService.GetAllTerm();

            foreach (Models.Term term in terms)
            {
                Term.Add(term);
            }

            IsBusy = false;
        }

        public async void Load()
        {
            IsBusy = true;
            Term.Clear();
            var terms = await DBService.GetAllTerm();
            foreach (Models.Term term in terms)
            {
                Term.Add(term);
            }
            IsBusy = false;
        }

        async void Modify(Models.Term term)
        {
            //SelectedTerm = term;
            //var termsModifyPage = new Views.ModifyTermsPage();
            //termsModifyPage.BindingContext = term;
            await AppShell.Current.GoToAsync($"{nameof(ModifyTermsPage)}?id={term.Id}&name={term.Name}&startDate={term.StartDate}&endDate={term.EndDate}");
                //($"///ModifyTermsPage"); //?Id={term.Id}");
            //await AppShell.Current.Navigation.PushAsync(termsModifyPage);
        }
    }
}