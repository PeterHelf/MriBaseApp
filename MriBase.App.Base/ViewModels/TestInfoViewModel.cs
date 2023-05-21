using Xamarin.Forms;

namespace MriBase.App.Base.ViewModels
{
    public class TestInfoViewModel
    {
        public TestInfoViewModel(string name, ContentPage page)
        {
            Name = name;
            Page = page;
        }

        public string Name { get; }

        public ContentPage Page { get; }
    }
}