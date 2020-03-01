using Sero.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman.Controller
{
    public class CredentialsFilter : CollectionFilter
    {
        public List<string> RoleCodes { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDateMin { get; set; }
        public DateTime? BirthDateMax { get; set; }
        public DateTime? CreationDateMin { get; set; }
        public DateTime? CreationDateMax { get; set; }

        public CredentialsFilter()
        {
            RoleCodes = new List<string>();
        }

        public CredentialsFilter(CredentialsFilter filter) 
            : base(filter)
        {
            this.RoleCodes = filter.RoleCodes;
            this.Email = filter.Email;
            this.BirthDateMin = filter.BirthDateMin;
            this.BirthDateMax = filter.BirthDateMax;
            this.CreationDateMin = filter.CreationDateMin;
            this.CreationDateMax = filter.CreationDateMax;
        }

        public CredentialsFilter(
            List<string> roleCodeList,
            string email,
            DateTime? birthDateMin,
            DateTime? birthDateMax,
            DateTime? creationDateMin,
            DateTime? creationDateMax,
            string textSearch, 
            int page, 
            int pageSize, 
            string sortBy, 
            string orderBy) 
            : base(textSearch, page, pageSize, sortBy, orderBy)
        {
            this.RoleCodes = roleCodeList;
            this.Email = email;
            this.BirthDateMin = birthDateMin;
            this.BirthDateMax = birthDateMax;
            this.CreationDateMin = creationDateMin;
            this.CreationDateMax = creationDateMax;
        }

        public override CollectionFilter Copy()
        {
            return new RolesFilter(this);
        }

        public override string GetDefaultSortByValue()
        {
            return nameof(Credential.CreationDate);
        }
    }
}
