using AutomationFramework.Helpers;
using AutomationFramework.Helpers.Interfaces;
using AutomationFramework.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationFramework.Services
{
    public class AccountService
    {
        private readonly IApiClient _apiClient;
        public string Token { get; private set; }
        public string UserId { get; private set; }

        public AccountService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        // ✅ Register User
        public RestResponse RegisterUser(string userName,string password)
        {
            var request = new AccountRequestBuilder()
                              .WithEndpoint(
                               "/Account/v1/User",
                               Method.Post)
                              .WithCredentials(userName, password)
                              .Build();

            var response = _apiClient.ExecuteRequest(request);

            if ((int)response.StatusCode == 201)
            {
                var userResponse = JsonConvert
                                   .DeserializeObject<UserResponse>
                                   (response.Content);
                UserId = userResponse?.userID;
                Console.WriteLine($"✅ Registered: {UserId}");
            }
            return response;
        }

        // ✅ Login - Checks Authorization
        // and Generates Token Together
        public RestResponse Login(string userName,string password)
        {
            // ✅ Step 1 - Check if Authorized
            var authRequest = new AccountRequestBuilder()
                                  .WithEndpoint(
                                   "/Account/v1/Authorized",
                                   Method.Post)
                                  .WithCredentials(userName, password)
                                  .Build();

            var authResponse = _apiClient.ExecuteRequest(authRequest);
         //   bool isAuthorized = JsonConvert.DeserializeObject<bool>(authResponse.IsSuccessful.ToString());

            if (authResponse.IsSuccessful)
            {
                // ✅ Step 2 - Generate Token on Login
                var tokenRequest = new AccountRequestBuilder()
                                       .WithEndpoint(
                                        "/Account/v1/GenerateToken",
                                        Method.Post)
                                       .WithCredentials(userName, password)
                                       .Build();

                var tokenResponse = _apiClient.ExecuteRequest(tokenRequest);
                var tokenData = JsonConvert.DeserializeObject<TokenResponse>(tokenResponse.Content);

                Token = tokenData?.token;
                Console.WriteLine($"✅ Login Successful!");
                Console.WriteLine($"✅ Token: {Token}");
                return tokenResponse;
            }

            Console.WriteLine($"❌ Login Failed!");
            return authResponse;
        }

        // ✅ Get User
        public RestResponse GetUser(string userId, string token)
        {
            var request = new AccountRequestBuilder()
                              .WithEndpoint(
                               $"/Account/v1/User/{userId}",
                               Method.Get)
                              .WithToken(token)
                              .Build();

            return _apiClient.ExecuteRequest(request);
        }

        // ✅ Delete User
        public RestResponse DeleteUser(string userId, string token)
        {
            var request = new AccountRequestBuilder()
                              .WithEndpoint(
                               $"/Account/v1/User/{userId}",
                               Method.Delete)
                              .WithToken(token)
                              .Build();

            return _apiClient.ExecuteRequest(request);
        }
    }
}
