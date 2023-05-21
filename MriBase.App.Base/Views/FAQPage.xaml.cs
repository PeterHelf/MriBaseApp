﻿using MriBase.App.Base.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MriBase.App.Base.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FAQPage : ContentPage
    {
        public FAQPage(FAQsViewModel viewModel)
        {
            InitializeComponent();

            this.BindingContext = viewModel;
        }
    }
}