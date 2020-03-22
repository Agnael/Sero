using Sero.Core;

namespace Sero.Doorman
{
    public class Resource : Element
    {
        public string Code { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }

        public Resource()
            : base(Constants.ResourceCodes.Resources)
        {

        }

        public Resource(string code)
            : base(Constants.ResourceCodes.Resources)
        {
            this.Category = "uncategorized";
            this.Code = code;
        }

        public Resource(string category, string code)
            : base(Constants.ResourceCodes.Resources)
        {
            this.Category = category;
            this.Code = code;
        }

        public Resource(string category, string code, string description)
            : base(Constants.ResourceCodes.Resources)
        {
            this.Category = category;
            this.Code = code;
            this.Description = description;
        }
    }
}
