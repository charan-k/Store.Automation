using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationFramework.Helpers.Interfaces
{
    // ✅ I Principle - Interface specific
    // to Book requests only
    // Separate from Account requests
    public interface IBookRequestBuilder
    {
        IBookRequestBuilder WithEndpoint(string endpoint, Method method);
        IBookRequestBuilder WithHeader(string key,string value);
        IBookRequestBuilder WithToken(string token);
        IBookRequestBuilder WithUserId(string userId);
        IBookRequestBuilder WithIsbn(string isbn);
        IBookRequestBuilder WithIsbns(List<string> isbns);
        RestRequest Build();
    }
}
