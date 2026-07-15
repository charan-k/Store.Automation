using AutomationFramework.Helpers.Interfaces;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationFramework.Helpers
{
    // ✅ S Principle - Only ONE job
    // which is to execute requests
    // Nothing else!
    public class ApiClient : IApiClient
    {
        private readonly RestClient _client;

        public ApiClient(string baseUrl)
        {
            _client = new RestClient(baseUrl);
        }

        // ✅ Only responsible for
        // executing the request
        public RestResponse ExecuteRequest(RestRequest request)
        {
            return _client.Execute(request);
        }
    }
}
