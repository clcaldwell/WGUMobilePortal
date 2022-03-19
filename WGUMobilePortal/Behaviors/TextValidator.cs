using Xamarin.Forms;

namespace WGUMobilePortal.Behaviors
{
    public class TextValidator : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            base.OnAttachedTo(entry);

            entry.TextChanged += Entry_TextChanged;
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool IsValid = !string.IsNullOrWhiteSpace(e.NewTextValue);
            ((Entry)sender).BackgroundColor = IsValid ? Color.Default : Color.Red;
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            base.OnDetachingFrom(entry);

            entry.TextChanged -= Entry_TextChanged;
        }
    }
}