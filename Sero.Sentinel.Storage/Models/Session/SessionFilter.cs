using Sero.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Abstractions;

namespace Sero.Sentinel.Storage
{
    public class SessionFilter : BaseCollectionFilter<SessionFilter, SessionSorting>, IXunitSerializable
    {
        protected override SessionFilter CurrentInstance => this;
        protected override SessionSorting DefaultSortBy => SessionSorting.LastActiveDate;

        public string CredentialId { get; set; }

        public string DeviceName { get; set; }
        public string DeviceType { get; set; }

        /// <summary>
        ///     Texto libre a buscarse dentro de la combinación "{Agent.Name} {Agent.Version}"
        /// </summary>
        public string Agent { get; set; }

        public DateTime? LoginDateMin { get; set; }
        public DateTime? LoginDateMax { get; set; }

        public DateTime? ExpirationDateMin { get; set; }
        public DateTime? ExpirationDateMax { get; set; }

        public DateTime? LastActiveDateMin { get; set; }
        public DateTime? LastActiveDateMax { get; set; }

        public bool? AllowSelfRenewal { get; set; }

        public SessionFilter()
        {

        }

        public SessionFilter(
            int page,
            int pageSize,
            SessionSorting sortBy,
            Order orderBy,
            string textSearch,            
            string deviceName,
            string deviceType,
            string agent,
            DateTime? loginDateMin,
            DateTime? loginDateMax,
            DateTime? expirationDateMin,
            DateTime? expirationDateMax,
            DateTime? lastActiveDateMin,
            DateTime? lastActiveDateMax,
            bool? allowSelfRenewal)
            : base(page, pageSize, sortBy, orderBy, textSearch)
        {
            this.DeviceName = deviceName;
            this.DeviceType = deviceType;
            this.Agent = agent;

            this.LoginDateMin = loginDateMin;
            this.LoginDateMax = loginDateMax;

            this.ExpirationDateMin = expirationDateMin;
            this.ExpirationDateMax = expirationDateMax;

            this.LastActiveDateMin = lastActiveDateMin;
            this.LastActiveDateMax = lastActiveDateMax;

            this.AllowSelfRenewal = allowSelfRenewal;
        }

        protected override void OnConfiguring()
        {
            For(x => x.CredentialId)
                .UseDefaultValue(null);

            For(x => x.DeviceName)
                .UseDefaultValue(null);

            For(x => x.DeviceType)
                .UseDefaultValue(null);

            For(x => x.Agent)
                .UseDefaultValue(null);


            For(x => x.LoginDateMin)
                .UseDefaultValue(null);

            For(x => x.LoginDateMax)
                .UseDefaultValue(null);


            For(x => x.ExpirationDateMin)
                .UseDefaultValue(null);

            For(x => x.ExpirationDateMax)
                .UseDefaultValue(null);


            For(x => x.LastActiveDateMin)
                .UseDefaultValue(null);

            For(x => x.LastActiveDateMax)
                .UseDefaultValue(null);


            For(x => x.AllowSelfRenewal)
                .UseDefaultValue(null);
        }
    }
}
