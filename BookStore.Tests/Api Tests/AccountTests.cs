using AutomationFramework.Core;
using AutomationFramework.Models;
using AutomationFramework.Tests.Tests;
using Newtonsoft.Json;

namespace AutomationFramework.Tests
{
    // ✅ S Principle - Only responsible
    // for Account related test cases
    //
    // ✅ O Principle - Extends BaseTest
    // without modifying it
    [TestFixture]
    public class AccountTests : BaseApiTests
    {
        // =============================================
        // ✅ REGISTER USER TESTS
        // =============================================

        // ✅ TC001 - Verify User Registered Successfully
        [Test, Order(1)]
        public void TC001_RegisterUser_ShouldHaveValidUserId()
        {
            // ✅ User already registered in BaseTest
            // Just verify UserId is not null
            Assert.That(UserId, Is.Not.Null);
            Assert.That(UserId, Is.Not.Empty);
            Logger.Instance.Info($"✅ UserId Verified: {UserId}");
        }

        // ✅ TC002 - Register User with Existing Username
        [Test, Order(2)]
        public void TC002_RegisterUser_ExistingUsername_ShouldReturn406()
        {
            // ✅ Try registering same user again
            var response = AccountService
                           .RegisterUser(UserName, Password);

            Assert.That((int)response.StatusCode, Is.EqualTo(406));
            Logger.Instance.Info($"✅ Duplicate User Rejected!");
            Logger.Instance.Info($"✅ Response: {response.Content}");
        }

        // ✅ TC003 - Register User with Weak Password
        [Test, Order(3)]
        public void TC003_RegisterUser_WeakPassword_ShouldReturn400()
        {
            var response = AccountService
                           .RegisterUser(
                            "NewWeakUser123",
                            "weakpassword");

            Assert.That((int)response.StatusCode, Is.EqualTo(400));
            Logger.Instance.Info($"✅ Weak Password Rejected!");
            Logger.Instance.Info($"✅ Response: {response.Content}");
        }

        // ✅ TC004 - Register User with Empty Username
        [Test, Order(4)]
        public void TC004_RegisterUser_EmptyUsername_ShouldReturn400()
        {
            var response = AccountService
                           .RegisterUser("", Password);

            Assert.That((int)response.StatusCode, Is.EqualTo(400));
            Console.WriteLine($"✅ Empty Username Rejected!");
            Console.WriteLine($"✅ Response: {response.Content}");
        }

        // ✅ TC005 - Register User with Empty Password
        [Test, Order(5)]
        public void TC005_RegisterUser_EmptyPassword_ShouldReturn400()
        {
            var response = AccountService
                           .RegisterUser(UserName, "");

            Assert.That((int)response.StatusCode, Is.EqualTo(400));
            Console.WriteLine($"✅ Empty Password Rejected!");
            Console.WriteLine($"✅ Response: {response.Content}");
        }

        // =============================================
        // ✅ GET USER TESTS
        // =============================================

        // ✅ TC006 - Get User with Valid Token
        [Test, Order(6)]
        public void TC006_GetUser_ValidToken_ShouldReturn200()
        {
            var response = AccountService
                           .GetUser(UserId, Token);
            var userResponse = JsonConvert
                               .DeserializeObject<UserResponse>
                               (response.Content);

            Assert.That((int)response.StatusCode, Is.EqualTo(200));
            Assert.That(userResponse.userID, Is.EqualTo(UserId));
            Assert.That(userResponse.username, Is.EqualTo(UserName));
            Logger.Instance.Info($"✅ Got User: {userResponse.username}");
        }

        // ✅ TC007 - Get User with Invalid Token
        [Test, Order(7)]
        public void TC007_GetUser_InvalidToken_ShouldReturn401()
        {
            var response = AccountService
                           .GetUser(UserId, "InvalidToken123");

            Assert.That((int)response.StatusCode, Is.EqualTo(401));
            Logger.Instance.Info($"✅ Unauthorized as Expected!");
            Logger.Instance.Info($"✅ Response: {response.Content}");
        }

        // ✅ TC008 - Get User with Invalid UserId
        [Test, Order(8)]
        public void TC008_GetUser_InvalidUserId_ShouldReturn401()
        {
            var response = AccountService
                           .GetUser("InvalidUserId123", Token);

            Assert.That((int)response.StatusCode, Is.EqualTo(401));
            Console.WriteLine($"✅ Invalid UserId Rejected!");
            Console.WriteLine($"✅ Response: {response.Content}");
        }

        // ✅ TC009 - Get User with Empty Token
        [Test, Order(9)]
        public void TC009_GetUser_EmptyToken_ShouldReturn401()
        {
            var response = AccountService
                           .GetUser(UserId, "");

            Assert.That((int)response.StatusCode, Is.EqualTo(401));
            Console.WriteLine($"✅ Empty Token Rejected!");
            Console.WriteLine($"✅ Response: {response.Content}");
        }
    }
}