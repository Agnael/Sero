using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sero.Loxy.Abstractions;
using Sero.Loxy.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestWeb.Models;

namespace TestWeb.Logging.Events
{
    public class UserCreatedEvent : JsonStateEvent<User>
    {
        public UserCreatedEvent(User user)
            : base(LogLevel.Information, "App", "A new User was created", user)
        {
        }
    }
}
