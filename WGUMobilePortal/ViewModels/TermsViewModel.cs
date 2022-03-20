using System.Collections.ObjectModel;
using System.Threading.Tasks;

using WGUMobilePortal.Models;
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

            Terms = new ObservableCollection<Term>();

            RefreshCommand = new Command(async () => await Refresh());
            AddCommand = new Command(async () => await Add());
            RemoveCommand = new Command<Term>(Remove);
            ModifyCommand = new Command<Term>(Modify);

            IsBusy = true;
        }

        public Command AddCommand { get; }

        public Command ModifyCommand { get; }

        public Command RefreshCommand { get; }

        public Command<Term> RemoveCommand { get; }

        public ObservableCollection<Term> Terms { get; set; }

        public async Task Load()
        {
            await Task.Run(() => Refresh());
        }

        public async Task OnAppearing()
        {
            await Load();
        }

        private async Task Add()
        {
            await AppShell.Current.GoToAsync($"{nameof(ModifyTermsPage)}?id={null}");

            await Refresh();
        }

        private async void Modify(Term term)
        {
            await AppShell.Current.GoToAsync($"{nameof(ModifyTermsPage)}?id={term.Id}&name={term.Name}&startDate={term.StartDate}&endDate={term.EndDate}");
        }

        private async Task Refresh()
        {
            IsBusy = true;
            Terms.Clear();
            var terms = await DBService.GetAllTerm();
            foreach (Term term in terms)
            {
                Terms.Add(term);
            }
            IsBusy = false;
        }

        private async void Remove(Term term)
        {
            if (await Shell.Current.DisplayAlert("Confirm Deletion", $"Are you sure you want to delete {term.Name}", "Delete", "Cancel"))
            {
                await DBService.RemoveTerm(term.Id);
                Terms.Remove(term);
            }
        }
    }
}