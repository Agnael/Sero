using Sero.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Abstractions;

namespace Sero.Sentinel.Storage
{
    public class CredentialRoleFilter : BaseCollectionFilter<CredentialRoleFilter, CredentialRoleSorting>, IXunitSerializable
    {
        protected override CredentialRoleFilter CurrentInstance => this;
        protected override CredentialRoleSorting DefaultSortBy => CredentialRoleSorting.Code;

        public string Code { get; set; }
        public string Description { get; set; }
        
        public CredentialRoleFilter()
        {

        }

        public CredentialRoleFilter(
            int page,
            int pageSize,
            CredentialRoleSorting sortBy,
            Order orderBy,
            string textSearch,
            string code,
            string description) 
            : base(page, pageSize, sortBy, orderBy, textSearch)
        {
            this.Code = code;
            this.Description = description;
        }

        protected override void OnConfiguring()
        {
            For(x => x.Code)
                .UseDefaultValue(null);

            For(x => x.Description)
                .UseDefaultValue(null);
        }
    }
}
