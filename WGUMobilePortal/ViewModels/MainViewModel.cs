﻿using System.Threading.Tasks;
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
            GenerateDummyData = new Command(async () => await GenerateData());
        }

        public ICommand GenerateDummyData { get; }

        public bool IsDataGenerated { get; set; } = false;
        public ICommand NavigateToAssessments { get; }

        public ICommand NavigateToCourses { get; }

        public ICommand NavigateToTerms { get; }

        public async Task GenerateData()
        {
            IsBusy = true;
            await DummyData.Main();
            IsBusy = false;
        }
    }
}