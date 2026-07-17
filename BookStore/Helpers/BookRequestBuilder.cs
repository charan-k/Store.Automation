using AutomationFramework.Helpers.Interfaces;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationFramework.Helpers
{
    
    public class BookRequestBuilder : IBookRequestBuilder
    {
        private RestRequest _request;
        private string _userId;
        private List<string> _isbns = new List<string>();

        // ✅ Set endpoint and method
        public IBookRequestBuilder WithEndpoint(string endpoint,Method method)
        {
            _request = new RestRequest(endpoint, method);
            return this;
        }

        // ✅ Add any custom header
        public IBookRequestBuilder WithHeader(string key,string value)
        {
            _request.AddHeader(key, value);
            return this;
        }

        // ✅ Add Authorization token
        public IBookRequestBuilder WithToken(string token)
        {
            _request.AddHeader("Authorization", $"Bearer {token}");
            return this;
        }

        // ✅ Add userId
        // Specific to Book requests only
        public IBookRequestBuilder WithUserId(string userId)
        {
            _userId = userId;
            return this;
        }

        // ✅ Add single ISBN
        // Specific to Book requests only
        public IBookRequestBuilder WithIsbn(string isbn)
        {
            _isbns.Add(isbn);
            return this;
        }

        // ✅ Add multiple ISBNs
        // Specific to Book requests only
        public IBookRequestBuilder WithIsbns(List<string> isbns)
        {
            _isbns.AddRange(isbns);
            return this;
        }

        // ✅ Build the final request
        // Adds default headers and body
        public RestRequest Build()
        {
            // ✅ Add default headers
            _request.AddHeader("Content-Type", "application/json");
            _request.AddHeader("Accept", "application/json");

            // ✅ Step 2 - Add body AFTER headers
            // Only for POST/PUT requests
            if (_request.Method == Method.Post || _request.Method == Method.Put )
            {
                // ✅ Build exact body structure
                // that DemoQA API expects
                if (_userId != null && _isbns.Count > 0)
                {
                    var body = new
                    {
                        userId = _userId,
                        collectionOfIsbns = _isbns
                            .Select(isbn => new { isbn = isbn })
                            .ToList()
                    };

                    // ✅ Serialize body to JSON string
                    // and add as request body
                    _request.AddJsonBody(
                             Newtonsoft.Json.JsonConvert
                             .SerializeObject(body));

                    Console.WriteLine(
                        $"✅ Request Body: " +
                        $"{Newtonsoft.Json.JsonConvert
                        .SerializeObject(body)}");
                }
            }
            else if (_request.Method == Method.Delete)
            {
                // ✅ DELETE - uses isbn directly
                // NOT collectionOfIsbns!
                if (_userId != null && _isbns.Count > 0)
                {
                    var body = new
                    {
                        userId = _userId,
                        isbn = _isbns.FirstOrDefault()
                    };

                    _request.AddJsonBody(
                             Newtonsoft.Json.JsonConvert
                             .SerializeObject(body));

                    Console.WriteLine(
                        $"✅ DELETE Body: " +
                        $"{Newtonsoft.Json.JsonConvert
                        .SerializeObject(body)}");
                }
            }

                return _request;
        }
    }
}
