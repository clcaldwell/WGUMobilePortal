using WGUMobilePortal.ViewModels;

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
            this.BindingContext = new CoursesViewModel();
        }

        protected override async void OnAppearing()
        {
            if (BindingContext is CoursesViewModel viewModel)
            {
                await viewModel.OnAppearing();
            }
        }
    }
}