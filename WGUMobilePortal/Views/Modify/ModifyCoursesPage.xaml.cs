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

        //ChangeInstructorCommand = new Command(async () => await ChangeInstructor());
        //DateSelectionCommand = new Command((arg)=> {
        //    var picker = arg as DatePicker;

        //    picker.IsEnabled = true;
        //    picker.IsVisible = true;
        //    picker.Focus();

        //});
    }
}