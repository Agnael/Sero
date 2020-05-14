using Sero.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Abstractions;

namespace Sero.Sentinel.Storage
{
    public class CredentialFilter : BaseCollectionFilter<CredentialFilter, CredentialSorting>, IXunitSerializable
    {
        protected override CredentialFilter CurrentInstance => this;
        protected override CredentialSorting DefaultSortBy => CredentialSorting.CreationDate;

        public IEnumerable<string> RoleCodes { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDateMin { get; set; }
        public DateTime? BirthDateMax { get; set; }
        public DateTime? CreationDateMin { get; set; }
        public DateTime? CreationDateMax { get; set; }
        
        public CredentialFilter()
        {

        }

        public CredentialFilter(
            int page,
            int pageSize,
            CredentialSorting sortBy,
            Order orderBy,
            string textSearch,
            List<string> roleCodeList,
            string email,
            DateTime? birthDateMin,
            DateTime? birthDateMax,
            DateTime? creationDateMin,
            DateTime? creationDateMax) 
            : base(page, pageSize, sortBy, orderBy, textSearch)
        {
            this.RoleCodes = roleCodeList;
            this.Email = email;
            this.BirthDateMin = birthDateMin;
            this.BirthDateMax = birthDateMax;
            this.CreationDateMin = creationDateMin;
            this.CreationDateMax = creationDateMax;
        }

        protected override void OnConfiguring()
        {
            For(x => x.RoleCodes)
                .UseDefaultValue(new List<string>());

            For(x => x.Email)
                .UseDefaultValue(null);

            For(x => x.BirthDateMin)
                .UseTransformer(x => x.ToUrlFriendlyValue())
                .UseDefaultValue(null);

            For(x => x.BirthDateMax)
                .UseTransformer(x => x.ToUrlFriendlyValue())
                .UseDefaultValue(null);

            For(x => x.CreationDateMin)
                .UseTransformer(x => x.ToUrlFriendlyValue())
                .UseDefaultValue(null);

            For(x => x.CreationDateMax)
                .UseTransformer(x => x.ToUrlFriendlyValue())
                .UseDefaultValue(null);
        }
    }
}
