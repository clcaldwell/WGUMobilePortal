using System;
using Xamarin.Forms;

namespace WGUMobilePortal.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        async void Button_Clicked(object sender, System.EventArgs e)
        {
            //Routing.RegisterRoute

            await Shell.Current.GoToAsync(nameof(sender));
        }
    }
}