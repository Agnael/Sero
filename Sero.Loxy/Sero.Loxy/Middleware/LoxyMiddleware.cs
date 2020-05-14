using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sero.Core.Middleware;
using Sero.Loxy.Abstractions;
using Sero.Loxy.Events;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Loxy
{
    public class LoxyMiddleware : AbstractMiddleware
    {
        private ILoxy _loxy;

        public LoxyMiddleware() 
        {

        }

        protected override async Task OnBefore(HttpContext context)
        {
            _loxy = (ILoxy)context.RequestServices.GetService(typeof(ILoxy));

            if (_loxy == null) 
                throw new LoxyNotFoundException();

            if (_loxy.Sinks == null || _loxy.Sinks.Count == 0)
                throw new NoSinksRegisteredException();
        }

        protected override async Task OnAfter(HttpContext context)
        {
            await _loxy.PersistAsync();
        }

        protected override async Task<bool> OnErrorShouldRethrow(HttpContext context, Exception ex)
        {
            // When it's a Loxy configuration error, we should actually throw it so the developer is aware of it, 
            // because it probably won't get to be logged into any sink for him to see.
            if (ex is NoSinksRegisteredException)
                throw ex;

            _loxy.Raise(new Event(LogLevel.Critical, "Loxy", "An unexpected exception was catched in the middleware phase end.", ex));
            await _loxy.PersistAsync();
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            throw new Exception("LOXY DEV MODE EXCEPTION (SEE INNER EX)", ex);

            return false;
        }
    }
}
