using MriBase.Models.Services.Interfaces;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MriBase.App.Base.Icon
{
    [ContentProperty(nameof(Text))]
    public class IconExtension : IMarkupExtension<ImageSource>
    {
        public string Text { get; set; }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }

        public ImageSource ProvideValue(IServiceProvider serviceProvider)
        {
            var imageService = ViewModels.BaseViewModel.Container.Resolve<IImageRecourceService>();
            return ImageSource.FromStream(() => new MemoryStream(imageService.GetImage(Text + ".png")));
        }
    }
}
