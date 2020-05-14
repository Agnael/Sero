using Microsoft.AspNetCore.Mvc.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public class ApiReturningNonApiResourceObject : Exception
    {
        public ApiReturningNonApiResourceObject(Type typeFound, string endpointName) 
            : base(string.Format(
                        "Attempted to return instance/s of type '{0}' at endpoint [Name='{1}'], " +
                        "The SERO API's can only return IApiResource inheritant instances as root elements.",
                    typeFound.FullName,
                    endpointName))
        {
        }
    }
}
