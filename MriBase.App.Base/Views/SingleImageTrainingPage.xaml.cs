using MriBase.App.Base.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MriBase.App.Base.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SingleImageTrainingPage : BaseTrainingPage
    {
        public SingleImageTrainingPage(SingleImageTrainingViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}