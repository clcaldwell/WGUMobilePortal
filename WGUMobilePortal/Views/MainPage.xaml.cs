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
            await Shell.Current.GoToAsync(nameof(sender));
        }

    }
}