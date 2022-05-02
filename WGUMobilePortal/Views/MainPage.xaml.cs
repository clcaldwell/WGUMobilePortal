using System.Threading.Tasks;

using WGUMobilePortal.Services;
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

            var Notify = new Notifier();
            Task.Run(async () => await Notify.OnStartNotifications());
        }

        private async void Button_Clicked(object sender, System.EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(sender));
        }
    }
}