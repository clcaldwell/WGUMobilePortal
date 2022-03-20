using System.Windows.Input;

using WGUMobilePortal.Services;
using WGUMobilePortal.Views;

using Xamarin.Forms;

namespace WGUMobilePortal.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            Title = "Main";

            NavigateToTerms = new Command(async () => await AppShell.Current.GoToAsync(nameof(TermsPage)));
            NavigateToCourses = new Command(async () => await AppShell.Current.GoToAsync(nameof(CoursesPage)));
            NavigateToAssessments = new Command(async () => await AppShell.Current.GoToAsync(nameof(AssessmentsPage)));
            //NavigateToTerms = new Command(async () => await AppShell.Current.GoToAsync("///TermsPage"));
            //NavigateToCourses = new Command(async () => await AppShell.Current.GoToAsync("///CoursesPage"));
            //NavigateToAssessments = new Command(async () => await AppShell.Current.GoToAsync("///AssessmentsPage"));
            GenerateDummyData = new Command(async () => await DummyData.Main());
        }

        public ICommand GenerateDummyData { get; }
        public ICommand NavigateToAssessments { get; }
        public ICommand NavigateToCourses { get; }
        public ICommand NavigateToTerms { get; }
    }
}