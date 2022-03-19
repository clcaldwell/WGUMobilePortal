using WGUMobilePortal.Views;

using Xamarin.Forms;

namespace WGUMobilePortal
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(AssessmentsPage), typeof(AssessmentsPage));
            Routing.RegisterRoute(nameof(CoursesPage), typeof(CoursesPage));
            Routing.RegisterRoute(nameof(TermsPage), typeof(TermsPage));
            Routing.RegisterRoute(nameof(ModifyTermsPage), typeof(ModifyTermsPage));
        }
    }
}