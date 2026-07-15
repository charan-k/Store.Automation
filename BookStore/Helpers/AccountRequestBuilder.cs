using AutomationFramework.Helpers.Interfaces;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationFramework.Helpers
{
    // ✅ S Principle - Only ONE job
    // which is to build Account requests
    //
    // ✅ O Principle - Open for extension
    // if new account request types needed
    // just extend this class
    // Closed for modification
    public class AccountRequestBuilder : IAccountRequestBuilder
    {
        private RestRequest _request;
        private string _userName;
        private string _password;

        // ✅ Set endpoint and method
        public IAccountRequestBuilder WithEndpoint(string endpoint,Method method)
        {
            _request = new RestRequest(endpoint, method);
            return this;
        }

        // ✅ Add any custom header
        public IAccountRequestBuilder WithHeader(string key,string value)
        {
            _request.AddHeader(key, value);
            return this;
        }

        // ✅ Add Authorization token
        public IAccountRequestBuilder WithToken(string token)
        {
            _request.AddHeader("Authorization", $"Bearer {token}");
            return this;
        }

        // ✅ Add user credentials
        // Specific to Account requests only
        public IAccountRequestBuilder WithCredentials(string userName,string password)
        {
            _userName = userName;
            _password = password;
            return this;
        }

        // ✅ Build the final request
        // Adds default headers and body
        public RestRequest Build()
        {
            // ✅ Add default headers
            _request.AddHeader("Content-Type", "application/json");
            _request.AddHeader("Accept", "application/json");

            // ✅ Add credentials to body if provided
            if (_userName != null && _password != null)
            {
                _request.AddJsonBody(new
                {
                    userName = _userName,
                    password = _password
                });
            }

            return _request;
        }
    }
}
