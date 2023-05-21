using MriBase.Models.Models;
using System.Collections.Generic;

namespace MriBase.App.Base.ViewModels
{
    public class FAQsViewModel
    {
        public List<FAQ> FAQs { get; set; }

        public FAQsViewModel()
        {
            this.FAQs = new List<FAQ>();

            FAQs.Add(new FAQ("Question1", "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam"));
            FAQs.Add(new FAQ("Question2", "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod"));
            FAQs.Add(new FAQ("Question3", "Lorem ipsum dolor sit amet, consetetur sadipscing elitr,"));
            FAQs.Add(new FAQ("Question4", "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At"));
            FAQs.Add(new FAQ("Question5", "Lorem ipsum dolor sit amet,"));
            FAQs.Add(new FAQ("Question6", "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore"));
            FAQs.Add(new FAQ("Question7", "Lorem ipsum dolor sit amet, consetetur sadipscing elitr,"));
            FAQs.Add(new FAQ("Question8", "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam"));
            FAQs.Add(new FAQ("Question9", "Lorem ipsum dolor sit amet, consetetur"));
        }
    }
}