using WGUMobilePortal.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WGUMobilePortal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssessmentsPage : ContentPage
    {
        public AssessmentsPage()
        {
            InitializeComponent();

            this.BindingContext = new AssessmentsViewModel();
        }
    }
}