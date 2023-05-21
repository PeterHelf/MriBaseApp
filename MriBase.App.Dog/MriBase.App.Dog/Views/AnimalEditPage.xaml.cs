using MriBase.App.Base.ViewModels;
using MriBase.App.Base.Views;
using MriBase.Models.Enums;
using MriBase.Models.Resources;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MriBase.App.Dog.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnimalEditPage : AnimalEditPageBase
    {
        private readonly AnimalEditViewModel viewModel;

        public AnimalEditPage(AnimalEditViewModel viewModel)
        {
            InitializeComponent();

            this.BindingContext = this.viewModel = viewModel;

            this.BreedSelector.Text = this.viewModel.Breeds.FirstOrDefault(b =>
                b.Breed.Equals(this.viewModel.SelectedBreed)).BreedName;

            if (this.viewModel.SelectedSex == Gender.Female)
            {
                this.FemaleButton.BackgroundColor = Color.Green;
                this.MaleButton.BackgroundColor = Color.LightGray;
                this.viewModel.SelectedSex = Gender.Female;
            }
            else
            {
                this.MaleButton.BackgroundColor = Color.Green;
                this.FemaleButton.BackgroundColor = Color.LightGray;
                this.viewModel.SelectedSex = Gender.Male;
            }
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            if (sender as Button == this.FemaleButton)
            {
                this.FemaleButton.BackgroundColor = Color.Green;
                this.MaleButton.BackgroundColor = Color.LightGray;
                this.viewModel.SelectedSex = Gender.Female;
            }
            else
            {
                this.MaleButton.BackgroundColor = Color.Green;
                this.FemaleButton.BackgroundColor = Color.LightGray;
                this.viewModel.SelectedSex = Gender.Male;
            }
        }

        private void BreedSelectorOnUnfocused(object sender, FocusEventArgs e)
        {
            var selectedBreed = this.viewModel.Breeds.FirstOrDefault(b =>
                b.BreedName.Equals(this.BreedSelector.Text, StringComparison.CurrentCultureIgnoreCase));

            if (selectedBreed is null)
            {
                this.BreedSelector.ErrorText = ResViewAnimalRegistration.InvaldBreedName;
                this.viewModel.SelectedBreed = null;
            }
            else
            {
                this.BreedSelector.ErrorText = string.Empty;
                this.viewModel.SelectedBreed = selectedBreed.Breed;
            }
        }

        private void BreedSelectorItemSelected(object sender, Xfx.XfxSelectedItemChangedEventArgs e)
        {
            var selectedBreed = this.viewModel.Breeds.FirstOrDefault(b =>
               b.BreedName.Equals(e.SelectedItem.ToString(), StringComparison.CurrentCultureIgnoreCase));

            if (selectedBreed is null)
            {
                this.BreedSelector.ErrorText = ResViewAnimalRegistration.InvaldBreedName;
                this.viewModel.SelectedBreed = null;
            }
            else
            {
                this.BreedSelector.ErrorText = string.Empty;
                this.BreedSelector.Text = selectedBreed.BreedName;
                this.viewModel.SelectedBreed = selectedBreed.Breed;
            }
        }
    }
}