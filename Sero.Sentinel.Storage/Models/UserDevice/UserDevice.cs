using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Sentinel.Storage
{
    public class UserDevice
    {
        /// <summary>
        ///     e.g.: "Desktop", "Phone", "Tablet", etc
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///     Customized name of the device, e.g.: "Oleg's OnePlus"
        /// </summary>
        public string Name { get; set; }

        public UserDevice()
        {

        }

        public UserDevice(string type, string name)
        {
            this.Type = type;
            this.Name = name;
        }
    }
}
