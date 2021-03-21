using System.Windows.Input;

namespace WGUMobilePortal.ViewModels
{
    public class AssessmentsViewModel : BaseViewModel
    {
        public AssessmentsViewModel()
        {
            Title = "Assessments";
        }

        public ICommand OpenWebCommand { get; }
    }
}