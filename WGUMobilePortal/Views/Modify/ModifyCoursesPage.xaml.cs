using System.Threading.Tasks;

using WGUMobilePortal.Models;
using WGUMobilePortal.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WGUMobilePortal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModifyCoursesPage : ContentPage
    {
        public ModifyCoursesPage()
        {
            InitializeComponent();
            this.BindingContext = new ModifyCoursesViewModel();
        }

        protected override async void OnAppearing()
        {
            if (BindingContext is ModifyCoursesViewModel viewModel)
            {
                await viewModel.OnAppearing();
            }
        }
    }
}