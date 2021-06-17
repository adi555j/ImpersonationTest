using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace ImpersonationTest
{
    public interface IRestAdapter
    {
        Task<string> GetJsonAsyncUsingAuthentication(string url);
    }

    public class RestAdapter : IRestAdapter
    {
        HttpClient _httpClient;
        ILogger<RestAdapter> _logger;
        private IHttpContextAccessor _httpContextAccessor;

        public RestAdapter(HttpClient httpClient, ILogger<RestAdapter> logger, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;           
        }
        public async Task<string> GetJsonAsyncUsingAuthentication(string url)
        {
            var response = "";
           
            var currentIdentity = WindowsIdentity.GetCurrent();
            _logger.LogInformation(string.Format("Current User:WindowsIdentity: {0}", currentIdentity.Name));

            WindowsIdentity contextIdentity = null;
            if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User != null)
            {
                var user = _httpContextAccessor.HttpContext.User;
                _logger.LogInformation(string.Format("Current User:_httpContextAccessor: {0}", user.Identity.Name));
                foreach (var claimsIdentity in _httpContextAccessor.HttpContext.User.Identities)
                {
                    if (contextIdentity == null)
                    {
                        contextIdentity = claimsIdentity as WindowsIdentity;
                    }
                }
            }
            WindowsIdentity userToImpersonate = WindowsIdentity.GetCurrent();
            if (contextIdentity != null)
            {
                _logger.LogInformation(string.Format("Current User:_contextIdentity: {0}", contextIdentity.Name));
                userToImpersonate = contextIdentity;
            }
            response = await WindowsIdentity.RunImpersonated(userToImpersonate.AccessToken, async () => await GetJsonAsyncWithCredentials(url));           

            return response;
        }
        internal async Task<string> GetJsonAsyncWithCredentials(string url)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            httpRequestMessage.Headers.Add("Accept", "application/json");           
            var response = await _httpClient.SendAsync(httpRequestMessage);

            if (!response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                throw new WebException(response.StatusCode.ToString(), new Exception(responseBody));
            }
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }

    }
}
