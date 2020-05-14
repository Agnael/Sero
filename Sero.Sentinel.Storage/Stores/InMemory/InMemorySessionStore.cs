using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sero.Core;

namespace Sero.Sentinel.Storage
{
    public class InMemorySessionStore : ISessionStore
    {
        public readonly IList<Session> Sessions;

        public InMemorySessionStore()
        {
            this.Sessions = new List<Session>();
        }

        public InMemorySessionStore(IList<Session> sessions)
        {
            this.Sessions = sessions;
        }

        public async Task Create(Session session)
        {
            this.Sessions.Add(session);
        }

        public async Task<Session> Get(
            string credentialId, 
            string deviceClass, 
            string deviceName, 
            string agentName, 
            string agentVersion)
        {
            var result = 
                Sessions
                .OrderByDescending(x => x.LoginDate)
                .FirstOrDefault(x => 
                    x.CredentialId == credentialId
                    && x.Device.Type == deviceClass
                    && x.Device.Name == deviceName
                    && x.Agent.Name == agentName
                    && x.Agent.Version == agentVersion
                    && x.ExpirationDate > DateTime.UtcNow);

            return result;
        }

        public async Task Update(Session session)
        {
            var found = 
                await Get(
                    session.CredentialId, 
                    session.Device.Type, 
                    session.Device.Name, 
                    session.Agent.Name, 
                    session.Agent.Version);

            found.LastActiveDate = session.LastActiveDate;
            found.ExpirationDate = session.ExpirationDate;
        }

        public async Task<Page<Session>> Get(SessionFilter filter)
        {
            IEnumerable<Session> query = Sessions;
            Func<Session, object> orderByPredicate = null;
            Func<Session, object> orderThenByPredicate = null;

            if (filter.SortBy == SessionSorting.AgentNameAndVersion)
            {
                orderByPredicate = x => x.Agent.Name;
                orderThenByPredicate = x => x.Agent.Version;
            }
            else if (filter.SortBy == SessionSorting.CredentialId)
            {
                orderByPredicate = x => x.CredentialId;
            }
            else if (filter.SortBy == SessionSorting.DeviceName)
            {
                orderByPredicate = x => x.Device.Name;
            }
            else if (filter.SortBy == SessionSorting.DeviceType)
            {
                orderByPredicate = x => x.Device.Type;
            }
            else if (filter.SortBy == SessionSorting.ExpirationDate)
            {
                orderByPredicate = x => x.ExpirationDate;
            }
            else if (filter.SortBy == SessionSorting.LastActiveDate)
            {
                orderByPredicate = x => x.LastActiveDate;
            }
            else if (filter.SortBy == SessionSorting.LoginDate)
            {
                orderByPredicate = x => x.LoginDate;
            }

            if (filter.OrderBy == Order.Desc)
            {
                if(orderThenByPredicate == null)
                {
                    query = query.OrderByDescending(orderByPredicate);
                }
                else
                {
                    query = query.OrderByDescending(orderByPredicate).ThenByDescending(orderThenByPredicate);
                }
            }
            else
            {
                if (orderThenByPredicate == null)
                {
                    query = query.OrderBy(orderByPredicate);
                }
                else
                {
                    query = query.OrderBy(orderByPredicate).OrderBy(orderThenByPredicate);
                }
            }

            if (!string.IsNullOrEmpty(filter.FreeText))
            {
                string target = filter.FreeText.ToLower();

                query =
                    query
                    .Where(x =>
                        x.CredentialId.ToLower().Contains(target)
                        // TODO: Cuando esté implementado con EF habría que ver si funciona llamar este método concatenador, seguro funciona pero tengo miedo de cómo se estaría armando la query
                        || x.Agent.GetFullName().Contains(target)
                        || x.Device.Type.Contains(target)
                        || x.Device.Name.Contains(target));
            }

            if (!string.IsNullOrEmpty(filter.CredentialId))
            {
                string target = filter.CredentialId.ToLower();
                query = query.Where(x => x.CredentialId.ToLower().Contains(target));
            }

            if (!string.IsNullOrEmpty(filter.Agent))
            {
                string target = filter.Agent.ToLower();
                query = query.Where(x => x.Agent.GetFullName().Contains(target));
            }

            if (!string.IsNullOrEmpty(filter.DeviceName))
            {
                string target = filter.DeviceName.ToLower();
                query = query.Where(x => x.Device.Name.Contains(target));
            }

            if (!string.IsNullOrEmpty(filter.DeviceType))
            {
                string target = filter.DeviceName.ToLower();
                query = query.Where(x => x.Device.Type.Contains(target));
            }


            if (filter.ExpirationDateMin.HasValue)
                query = query.Where(x => x.ExpirationDate >= filter.ExpirationDateMin.Value);

            if (filter.ExpirationDateMax.HasValue)
                query = query.Where(x => x.ExpirationDate <= filter.ExpirationDateMax.Value);


            if (filter.LastActiveDateMin.HasValue)
                query = query.Where(x => x.LastActiveDate >= filter.LastActiveDateMin.Value);

            if (filter.LastActiveDateMax.HasValue)
                query = query.Where(x => x.LastActiveDate <= filter.LastActiveDateMax.Value);


            if (filter.LoginDateMin.HasValue)
                query = query.Where(x => x.LoginDate >= filter.LoginDateMin.Value);

            if (filter.LoginDateMax.HasValue)
                query = query.Where(x => x.LoginDate <= filter.LoginDateMax.Value);

            if (filter.AllowSelfRenewal.HasValue)
                query = query.Where(x => x.AllowSelfRenewal == filter.AllowSelfRenewal.Value);

            var count = query.Count();
            var list = query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            return new Page<Session>(count, list);
        }
    }
}
