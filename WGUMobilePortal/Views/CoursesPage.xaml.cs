using System;

using WGUMobilePortal.Models;
using WGUMobilePortal.Services;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WGUMobilePortal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CoursesPage : ContentPage
    {
        public CoursesPage()
        {
            InitializeComponent();
        }

        async void OnDeleteButtonClicked(object sender, EventArgs args)
        {

            Button clickedButton = (Button)sender;
            Course selectedCourse = (Course)clickedButton.CommandParameter;

            var result = await this.DisplayAlert("Alert!", $"Are you sure you want to delete {selectedCourse.Name}", "Yes", "No");
            if (result)
            {
                await DBService.RemoveCourse(selectedCourse.Id);
            }

        }
    }
}