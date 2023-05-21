namespace MriBase.Models.Models
{
    public class FAQ
    {
        public string Question { get; set; }

        public string Answer { get; set; }

        public FAQ(string question, string answer)
        {
            Question = question;
            Answer = answer;
        }
    }
}