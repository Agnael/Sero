using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Sentinel.Storage
{
    public class UserAgent
    {
        /// <summary>
        ///     E.g.: "Chrome", "Mozilla", "Postman", etc
        /// </summary>
        public string Name { get; set; }

        public string Version { get; set; }

        public UserAgent()
        {

        }

        public UserAgent(string name, string version)
        {
            this.Name = name;
            this.Version = version;
        }

        public string GetFullName()
        {
            return string.Format("{0} {1}", Name, Version);
        }
    }
}
