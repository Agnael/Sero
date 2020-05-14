using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Sentinel.Storage
{
    public interface ILoginAttemptStore
    {
        Task<LoginAttempt> GetLatest(string fromIp);
        Task<int> Count(string fromIp, DateTime fromDate, DateTime toDate);

        Task Create(LoginAttempt attempt);
    }
}
