
using WGUMobilePortal.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WGUMobilePortal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddEditTermPage : ContentPage
    {
        public AddEditTermPage()
        {
            InitializeComponent();
            this.BindingContext = new TermsViewModel();
        }
    }
}