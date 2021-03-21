using System;

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

        async void OnButtonClicked(object sender, EventArgs args)
        {
            //await label.RelRotateTo(360, 1000);
        }
    }
}