using WGUMobilePortal.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WGUMobilePortal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModifyAssessmentsPage : ContentPage
    {
        public ModifyAssessmentsPage()
        {
            InitializeComponent();
            this.BindingContext = new ModifyAssessmentsViewModel();
        }
    }
}