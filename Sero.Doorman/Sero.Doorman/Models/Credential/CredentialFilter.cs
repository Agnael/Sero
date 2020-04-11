using Sero.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Abstractions;

namespace Sero.Doorman.Controller
{
    public class CredentialFilter : CollectionFilter<CredentialFilter, CredentialSorting>, IXunitSerializable
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
                .UseCriteria<RoleCodesCriteria>()
                .UseDefaultValue(new List<string>());

            For(x => x.Email)
                .UseCriteria<FreeTextCriteria>()
                .UseDefaultValue(null);

            For(x => x.BirthDateMin)
                .UseCriteria<FreeDateCriteria>()
                .UseDefaultValue(null);

            For(x => x.BirthDateMax)
                .UseCriteria<FreeDateCriteria>()
                .UseDefaultValue(null);

            For(x => x.CreationDateMin)
                .UseCriteria<FreeDateCriteria>()
                .UseDefaultValue(null);

            For(x => x.CreationDateMax)
                .UseCriteria<FreeDateCriteria>()
                .UseDefaultValue(null);
        }

        public override void XunitDeserialize(IXunitSerializationInfo info)
        {
            this.BirthDateMax = info.GetValue<DateTime?>(nameof(BirthDateMax));
            this.BirthDateMin = info.GetValue<DateTime?>(nameof(BirthDateMin));

            this.CreationDateMax = info.GetValue<DateTime?>(nameof(CreationDateMax));
            this.CreationDateMin = info.GetValue<DateTime?>(nameof(CreationDateMin));

            this.Email = info.GetValue<string>(nameof(Email));

            string[] roleCodeArr = info.GetValue<string[]>(nameof(RoleCodes));

            if (roleCodeArr == null || roleCodeArr.Length == 0)
                this.RoleCodes = new List<string>();
            else
                this.RoleCodes = roleCodeArr.ToList();
        }

        public override void XunitSerialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(BirthDateMax), this.BirthDateMax);
            info.AddValue(nameof(BirthDateMin), this.BirthDateMin);

            info.AddValue(nameof(CreationDateMax), this.CreationDateMax);
            info.AddValue(nameof(CreationDateMin), this.CreationDateMin);

            info.AddValue(nameof(Email), this.Email);

            string[] roleCodeArr = this.RoleCodes == null || this.RoleCodes.Count() == 0 ? null : this.RoleCodes.ToArray();
            info.AddValue(nameof(RoleCodes), roleCodeArr);
        }
    }
}
