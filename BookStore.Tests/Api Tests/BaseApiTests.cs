using AutomationFramework.Core;
using AutomationFramework.Helpers;
using AutomationFramework.Helpers.Interfaces;
using AutomationFramework.Services;
using AutomationFramework.Utilities;

namespace AutomationFramework.Tests.Tests
{
    // ✅ Add Category HERE in Base class
    // So ALL API tests inherit this category
    // No need to add in every test class!
    [TestFixture]
    [Category("API")]
    public class BaseApiTests
    {
        protected IApiClient ApiClient;
        protected AccountService AccountService;
        protected BookStoreService BookStoreService;

        protected string UserName;
        protected string Password;
        protected string UserId;
        protected string Token;

        string BaseUrl = Config.Instance.Get("BaseAPIUrl", "https://demoqa.com");

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Logger.Instance.Info(
                "========== API Tests Started ==========");

            ApiClient = new ApiClient(BaseUrl);
            AccountService = new AccountService(ApiClient);
            BookStoreService = new BookStoreService(ApiClient);

            var loginData = DataProvider.PrepareValidLoginData();

            UserName = loginData.Username;
            Password = loginData.Password;

            AccountService.RegisterUser(UserName, Password);
            AccountService.Login(UserName, Password);

            UserId = AccountService.UserId;
            Token = AccountService.Token;
           
            Logger.Instance.Info($"✅ BaseApiTests Setup Done!");
            Logger.Instance.Info($"✅ UserName: {UserName}");
            Logger.Instance.Info($"✅ UserID: {UserId}");
            Logger.Instance.Info($"✅ Token: {Token}");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            if (UserId != null && Token != null)
            {
                AccountService.DeleteUser(UserId, Token);
                Logger.Instance.Info($"🗑️ User Deleted: {UserId}");
            }

            Logger.Instance.Info("========== API Tests Completed ==========");
        }
    }

}
