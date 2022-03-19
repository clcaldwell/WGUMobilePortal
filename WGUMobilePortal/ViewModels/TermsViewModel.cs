using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using WGUMobilePortal.Services;
using WGUMobilePortal.Views;

using Xamarin.Forms;

namespace WGUMobilePortal.ViewModels
{
    public class TermsViewModel : BaseViewModel
    {
        public TermsViewModel()
        {
            Title = "Terms View";

            Terms = new ObservableCollection<Models.Term>();

            RefreshCommand = new Command(async () => await Refresh());
            AddCommand = new Command(async () => await Add());
            RemoveCommand = new Command<Models.Term>(Remove);
            ModifyCommand = new Command<Models.Term>(Modify);

            IsBusy = true;
        }

        public async Task OnAppearing()
        {
            await Load();
        }

        public ObservableCollection<Models.Term> Terms { get; set; }
        public Command RefreshCommand { get; }
        public Command AddCommand { get; }
        public Command<Models.Term> RemoveCommand { get; }
        public Command ModifyCommand { get; }

        async Task Add()
        {
            await AppShell.Current.GoToAsync($"{nameof(ModifyTermsPage)}?id={null}");
            
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
        async void Modify(Models.Term term)
        {
            await AppShell.Current.GoToAsync($"{nameof(ModifyTermsPage)}?id={term.Id}&name={term.Name}&startDate={term.StartDate}&endDate={term.EndDate}");
        }
        public async Task Load()
        {
            await Task.Factory.StartNew(() => Refresh());
        }
        async Task Refresh()
        {
            IsBusy = true;
            Terms.Clear();
            var terms = await DBService.GetAllTerm();
            foreach (Models.Term term in terms)
            {
                Terms.Add(term);
            }
            IsBusy = false;
        }

    }
}