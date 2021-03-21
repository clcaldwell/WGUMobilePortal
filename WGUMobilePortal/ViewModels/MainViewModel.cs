using System.Windows.Input;

using Xamarin.Forms;

namespace WGUMobilePortal.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            Title = "Main";

            NavigateToTerms = new Command(async () => await AppShell.Current.GoToAsync("///TermsPage"));
            NavigateToCourses = new Command(async () => await AppShell.Current.GoToAsync("///CoursesPage"));
            NavigateToAssessments = new Command(async () => await AppShell.Current.GoToAsync("///AssessmentsPage"));
        }

        public ICommand NavigateToTerms { get; }
        public ICommand NavigateToCourses { get; }
        public ICommand NavigateToAssessments { get; }
    }
}