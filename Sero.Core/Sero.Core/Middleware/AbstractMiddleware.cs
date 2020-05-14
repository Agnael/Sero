using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Core.Middleware
{
    public abstract class AbstractMiddleware : IMiddleware
    {
        protected readonly RequestDelegate _next;

        public AbstractMiddleware()
        {

        }

        public virtual async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await OnBefore(context);
                await next(context);
                await OnAfter(context);
            }
            catch (Exception ex)
            {
                if (await OnErrorShouldRethrow(context, ex))
                    throw;
            }
        }

        protected abstract Task OnBefore(HttpContext context);
        protected abstract Task OnAfter(HttpContext context);
        protected abstract Task<bool> OnErrorShouldRethrow(HttpContext context, Exception ex);

        protected T GetService<T>(HttpContext context)
        {
            var service = (T)context.RequestServices.GetService(typeof(T));
            return service;
        }
    }
}
