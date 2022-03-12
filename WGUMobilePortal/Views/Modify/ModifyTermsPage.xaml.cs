using WGUMobilePortal.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WGUMobilePortal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModifyTermsPage : ContentPage
    {
        public ModifyTermsPage()
        {
            InitializeComponent();

            this.BindingContext = new ModifyTermsViewModel();
        }
    }
}