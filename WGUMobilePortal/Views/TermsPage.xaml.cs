using System;
using System.Threading.Tasks;

using WGUMobilePortal.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WGUMobilePortal.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermsPage : ContentPage
    {
        public TermsPage()
        {
            InitializeComponent();
            this.BindingContext = new TermsViewModel();
        }

        protected override void OnAppearing()
        {
            if (BindingContext is TermsViewModel viewModel)
            {
                _ = viewModel.OnAppearing();
            }
        }

    }
}