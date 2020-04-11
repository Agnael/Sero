using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sero.Core;
using Sero.Doorman.Controller;

namespace Sero.Doorman
{
    public class InMemoryLoginAttemptStore : ILoginAttemptStore
    {
        public readonly IList<LoginAttempt> Attempts;
        
        public InMemoryLoginAttemptStore()
        {
            this.Attempts = new List<LoginAttempt>();
        }

        public InMemoryLoginAttemptStore(IList<LoginAttempt> loginAttempts)
        {
            this.Attempts = loginAttempts;
        }

        public async Task<int> Count(string fromIp, DateTime fromDate, DateTime toDate)
        {
            int count =
                Attempts
                .Count(x =>
                    x.FromIp == fromIp
                    && x.AttemptDate >= fromDate
                    && x.AttemptDate <= toDate);

            return count;
        }

        public async Task Create(LoginAttempt attempt)
        {
            this.Attempts.Add(attempt);
        }

        public async Task<LoginAttempt> GetLatest(string fromIp)
        {
            var result =
                Attempts
                .OrderByDescending(x => x.AttemptDate)
                .FirstOrDefault();

            return result;
        }
    }
}
