using System;

using WGUMobilePortal.Models;
using WGUMobilePortal.Services;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WGUMobilePortal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssessmentsPage : ContentPage
    {
        public AssessmentsPage()
        {
            InitializeComponent();
        }

        async void OnDeleteButtonClicked(object sender, EventArgs args)
        {

            Button clickedButton = (Button)sender;
            Assessment selectedAssessment = (Assessment)clickedButton.CommandParameter;

            var result = await this.DisplayAlert("Alert!", $"Are you sure you want to delete {selectedAssessment.Name}", "Yes", "No");
            if (result)
            {
                await DBService.RemoveAssessment(selectedAssessment.Id);
            }

        }
    }
}