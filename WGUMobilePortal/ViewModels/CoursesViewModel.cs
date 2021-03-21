using System.Windows.Input;

namespace WGUMobilePortal.ViewModels
{
    public class CoursesViewModel : BaseViewModel
    {
        public CoursesViewModel()
        {
            Title = "Courses";
        }

        public ICommand OpenWebCommand { get; }
    }
}