
using System;

using WGUMobilePortal.Models;
using WGUMobilePortal.Services;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WGUMobilePortal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermsPage : ContentPage
    {
        public TermsPage()
        {
            InitializeComponent();
        }
        async void OnDeleteButtonClicked(object sender, EventArgs args)
        {

            Button clickedButton = (Button)sender;
            Term selectedTerm = (Term)clickedButton.CommandParameter;

            var result = await this.DisplayAlert("Alert!", $"Are you sure you want to delete {selectedTerm.Name}", "Yes", "No");
            if (result)
            {
                await DBService.RemoveTerm(selectedTerm.Id);
            }
                        
        }

    }
}