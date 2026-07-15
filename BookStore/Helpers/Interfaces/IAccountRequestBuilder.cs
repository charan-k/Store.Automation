using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationFramework.Helpers.Interfaces
{
    // ✅ I Principle - Interface specific
    // to Account requests only
    // Separate from Book requests
    public interface IAccountRequestBuilder
    {
        IAccountRequestBuilder WithEndpoint(string endpoint,Method method);
        IAccountRequestBuilder WithHeader(string key,string value);
        IAccountRequestBuilder WithToken(string token);
        IAccountRequestBuilder WithCredentials(string userName,string password);
        RestRequest Build();
    }
}
