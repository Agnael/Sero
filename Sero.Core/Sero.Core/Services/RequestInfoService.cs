using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Core.Services
{
    public class RequestInfoService : IRequestInfoService
    {
        private readonly HttpContext _httpContext;

        private string _requestedUrl;
        private string _requestQueryString;
        private string _requestBody;
        private string _requestedVerb;
        private string _acceptLanguageHeader;
        private string _requestId;
        private DateTime _requestStartDateUtc;
        private long _requestStartUnixTimestamp;
        private int _idProcess;
        private string _ipRemote;

        public RequestInfoService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;

            if(_httpContext != null)
            {
                _requestedUrl = _httpContext.Request.Path;
                _requestQueryString = _httpContext.Request.QueryString.HasValue ? _httpContext.Request.QueryString.Value : null;

                string body = GetRequestBody(_httpContext.Request).Result;
                _requestBody = string.IsNullOrEmpty(body) ? null : body;

                _requestedVerb = _httpContext.Request.Method;
                _acceptLanguageHeader = _httpContext.Request.Headers["Accept-Language"].ToString();

                _requestId = _httpContext.TraceIdentifier;
                _requestStartDateUtc = DateTime.UtcNow;
                _idProcess = Process.GetCurrentProcess().Id;

                _requestStartUnixTimestamp = ((DateTimeOffset)_requestStartDateUtc).ToUnixTimeSeconds();

                if (_httpContext.Connection.RemoteIpAddress != null)
                    _ipRemote = _httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                else
                    _ipRemote = Constants.UNKNOWN_IP;
            }
        }

        public async Task<string> GetRequestBody(HttpRequest request)
        {
            StreamReader bodyReader = new StreamReader(request.Body);
            string bodyStr = await bodyReader.ReadToEndAsync();

            byte[] requestData = Encoding.UTF8.GetBytes(bodyStr);
            request.Body = new MemoryStream(requestData);
            
            return bodyStr;
        }

        string IRequestInfoService.AcceptLanguageHeader => _acceptLanguageHeader;

        DateTime IRequestInfoService.StartDateUtc => _requestStartDateUtc;

        int IRequestInfoService.IdProcess => _idProcess;

        string IRequestInfoService.QueryString => _requestQueryString;

        string IRequestInfoService.RequestBody => _requestBody;

        string IRequestInfoService.RemoteIp => _ipRemote;

        string IRequestInfoService.RequestId => _requestId;

        long IRequestInfoService.StartUnixTimestamp => _requestStartUnixTimestamp;

        string IRequestInfoService.Url => _requestedUrl;

        string IRequestInfoService.Verb => _requestedVerb;
    }
}
