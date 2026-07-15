using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationFramework.Helpers
{
    public class ApiHelper
    {
        private readonly RestClient _client;
        private const string BaseUrl = "https://demoqa.com";

        public ApiHelper()
        {
            _client = new RestClient(BaseUrl);
        }

        public RestResponse ExecuteRequest(RestRequest request)
        {
            return _client.Execute(request);
        }

        public RestRequest CreateRequest(string endpoint, Method method)
        {
            var request = new RestRequest(endpoint, method);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            return request;
        }

        public RestRequest CreateRequestWithToken(
                           string endpoint,
                           Method method,
                           string token)
        {
            var request = CreateRequest(endpoint, method);
            request.AddHeader("Authorization", $"Bearer {token}");
            return request;
        }
    }
}
