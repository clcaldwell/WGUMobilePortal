using System;

using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;

using WGUMobilePortal.ViewModels;

using Xamarin.Forms;

namespace WGUMobilePortal.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            this.BindingContext = new MainViewModel();
        }

        async void Button_Clicked(object sender, System.EventArgs e)
        {
            //Routing.RegisterRoute

            await Shell.Current.GoToAsync(nameof(sender));
        }

        private async void OnPopupTask(object sender, EventArgs e)
        {
            TermTaskView page = new TermTaskView();
            //StartPopup(page);
            await Navigation.PushPopupAsync(page, true);
            //PopupNavigation.PushAsync(new TermTaskView());
        }

        private async void StartPopup(PopupPage page)
        {
            // Open a PopupPage
            await Navigation.PushPopupAsync(page, true);

            // Close the last PopupPage int the PopupStack
            //await Navigation.PopPopupAsync();

            // Close all PopupPages in the PopupStack
            //await Navigation.PopAllPopupAsync();

            // Close an one PopupPage in the PopupStack even if the page is not the last
            //await Navigation.RemovePopupPageAsync(page);
        }
    }
}