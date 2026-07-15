using AutomationFramework.Helpers;
using AutomationFramework.Helpers.Interfaces;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationFramework.Services
{
    public class BookStoreService
    {
        private readonly IApiClient _apiClient;

        public BookStoreService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        // ✅ Get All Books
        public RestResponse GetAllBooks()
        {
            var request = new BookRequestBuilder()
                              .WithEndpoint(
                               "/BookStore/v1/Books",
                               Method.Get)
                              .Build();

            return _apiClient.ExecuteRequest(request);
        }

        // ✅ Get Book by ISBN
        public RestResponse GetBookByIsbn(string isbn)
        {
            var request = new BookRequestBuilder()
                              .WithEndpoint(
                               $"/BookStore/v1/Book?ISBN={isbn}",
                               Method.Get)
                              .Build();

            return _apiClient.ExecuteRequest(request);
        }

        // ✅ Add Book
        public RestResponse AddBook(string userId,string isbn, string token)
        {
            var request = new BookRequestBuilder()
                              .WithEndpoint(
                               "/BookStore/v1/Books",
                               Method.Post)
                              .WithToken(token)
                              .WithUserId(userId)
                              .WithIsbn(isbn)
                              .Build();

            return _apiClient.ExecuteRequest(request);
        }

        // ✅ Delete Book
        public RestResponse DeleteBook(string userId,string isbn,string token)
        {
            var request = new BookRequestBuilder()
                              .WithEndpoint(
                               "/BookStore/v1/Book",
                               Method.Delete)
                              .WithToken(token)
                              .WithUserId(userId)
                              .WithIsbn(isbn)
                              .Build();

            return _apiClient.ExecuteRequest(request);
        }

        // ✅ Delete All Books
        public RestResponse DeleteAllBooks(
                            string userId,
                            string token)
        {
            var request = new BookRequestBuilder()
                              .WithEndpoint(
                               $"/BookStore/v1/Books?UserId={userId}",
                               Method.Delete)
                              .WithToken(token)
                              .Build();

            return _apiClient.ExecuteRequest(request);
        }
    }
}
