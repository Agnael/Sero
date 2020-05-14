using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sero.Core;

namespace Sero.Sentinel.Storage
{
    public class InMemoryCredentialPenaltyStore : ICredentialPenaltyStore
    {
        public readonly IList<CredentialPenalty> Penalties;

        public InMemoryCredentialPenaltyStore()
        {
            this.Penalties = new List<CredentialPenalty>();
        }

        public InMemoryCredentialPenaltyStore(IList<CredentialPenalty> penalties)
        {
            this.Penalties = penalties;
        }

        public async Task CreatePenalty(CredentialPenalty penalty)
        {
            this.Penalties.Add(penalty);
        }

        public async Task<CredentialPenalty> Get(string credentialId, DateTime targetDate)
        {
            var result =
                Penalties
                .FirstOrDefault(x =>
                    x.CredentialId == credentialId
                    && targetDate >= x.StartDate
                    && targetDate <= x.EndDate);

            return result;
        }
    }
}
