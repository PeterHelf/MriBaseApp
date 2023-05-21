namespace MriBase.Models.Models
{
    public class Link
    {
        private string linkAlias;

        public Link(string uri, string linkName = null)
        {
            Uri = uri;
            LinkAlias = linkName;
        }

        public string Uri { get; set; }

        public string LinkAlias
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(linkAlias))
                {
                    return linkAlias;
                }
                else
                {
                    return this.Uri;
                }

            }

            set => linkAlias = value;
        }
    }
}
