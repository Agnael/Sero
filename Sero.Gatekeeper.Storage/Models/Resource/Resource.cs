using Sero.Core;

namespace Sero.Gatekeeper.Storage
{
    public class Resource : IApiResource
    {
        public string ApiResourceCode => GtkResourceCodes.Resources;

        public string OwnerId { get; set; }
        public string Code { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }

        public Resource()
        {

        }

        public Resource(string code)
        {
            this.Category = "uncategorized";
            this.Code = code;
        }

        public Resource(string category, string code)
        {
            this.Category = category;
            this.Code = code;
        }

        public Resource(string category, string code, string description)
        {
            this.Category = category;
            this.Code = code;
            this.Description = description;
        }
    }
}
