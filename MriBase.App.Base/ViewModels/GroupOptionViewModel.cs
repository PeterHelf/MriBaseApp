using MriBase.App.Base.Converter;
using MriBase.Models.Enums;

namespace MriBase.App.Base.ViewModels
{
    public class GroupOptionViewModel
    {
        public GroupOptionViewModel(GroupOption groupOption)
        {
            this.GroupOption = groupOption;
        }

        public GroupOption GroupOption { get; }

        public string Name { get => new EnumLanguageConverter().Convert(GroupOption, typeof(string)) as string; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
