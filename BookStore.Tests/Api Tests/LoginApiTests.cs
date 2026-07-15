using AutomationFramework.Models;
using AutomationFramework.Services;
using AutomationFramework.Tests.Tests;
using Newtonsoft.Json;

namespace AutomationFramework.Tests
    {
        // ✅ S Principle - Only responsible
        // for Login and Token related test cases
        //
        // ✅ O Principle - Extends BaseTest
        // without modifying it
        [TestFixture]
        public class LoginApiTests : BaseApiTests
        {
            // =============================================
            // ✅ LOGIN TESTS
            // =============================================

            // ✅ TC001 - Login with Valid Credentials
            [Test, Order(1)]
            public void TC001_Login_ValidCredentials_ShouldReturn200()
            {
                var response = AccountService
                               .Login(UserName, Password);

                Assert.That((int)response.StatusCode, Is.EqualTo(200));
                Console.WriteLine($"✅ Login Status: {(int)response.StatusCode}");
            }

            // ✅ TC002 - Login Should Generate Token
            [Test, Order(2)]
            public void TC002_Login_ShouldGenerateToken()
            {
                AccountService.Login(UserName, Password);

                Assert.That(AccountService.Token, Is.Not.Null);
                Assert.That(AccountService.Token, Is.Not.Empty);
                Console.WriteLine($"✅ Token: {AccountService.Token}");
            }

            // ✅ TC003 - Login Token Status Should Be Authorized
            [Test, Order(3)]
            public void TC003_Login_TokenStatus_ShouldBeAuthorized()
            {
                var response = AccountService
                               .Login(UserName, Password);
                var tokenResponse = JsonConvert
                                    .DeserializeObject<TokenResponse>
                                    (response.Content);

                Assert.That(tokenResponse.status,
                            Is.EqualTo("Success"));
                Console.WriteLine($"✅ Status: {tokenResponse.status}");
            }

            // ✅ TC004 - Login Token Should Have Expiry Date
            [Test, Order(4)]
            public void TC004_Login_Token_ShouldHaveExpiryDate()
            {
                var response = AccountService
                               .Login(UserName, Password);
                var tokenResponse = JsonConvert
                                    .DeserializeObject<TokenResponse>
                                    (response.Content);

                Assert.That(tokenResponse.expires, Is.Not.Null);
                DateTime expiryDate = DateTime
                                      .Parse(tokenResponse.expires);
                Assert.That(expiryDate, Is.GreaterThan(DateTime.Now));
                Console.WriteLine($"✅ Expiry: {tokenResponse.expires}");
            }

            // ✅ TC005 - Login Token Result Message
            [Test, Order(5)]
            public void TC005_Login_Token_ShouldHaveResultMessage()
            {
                var response = AccountService
                               .Login(UserName, Password);
                var tokenResponse = JsonConvert
                                    .DeserializeObject<TokenResponse>
                                    (response.Content);

                Assert.That(tokenResponse.result,
                            Is.EqualTo("User authorized successfully."));
                Console.WriteLine($"✅ Result: {tokenResponse.result}");
            }

            // =============================================
            // ❌ NEGATIVE LOGIN TESTS
            // =============================================

            // ✅ TC006 - Login with Wrong Password
            [Test, Order(6)]
            public void TC006_Login_WrongPassword_ShouldFail()
            {
                var response = AccountService.Login(UserName, "WrongPassword@123");
                Assert.That(response.IsSuccessful, Is.False);
                Console.WriteLine($"✅ Login Failed as Expected!");
            }

            // ✅ TC007 - Login with Wrong Username
            [Test, Order(7)]
            public void TC007_Login_WrongUsername_ShouldFail()
            {
                var response = AccountService
                               .Login("WrongUser123", Password);
                Assert.That(response.IsSuccessful, Is.False);
                Console.WriteLine($"✅ Login Failed as Expected!");
            }

            // ✅ TC008 - Login with Empty Username
            [Test, Order(8)]
            public void TC008_Login_EmptyUsername_ShouldReturn400()
            {
                var response = AccountService
                               .Login("", Password);

                Assert.That((int)response.StatusCode, Is.EqualTo(400));
                Console.WriteLine($"✅ Empty Username Rejected!");
            }

            // ✅ TC009 - Login with Empty Password
            [Test, Order(9)]
            public void TC009_Login_EmptyPassword_ShouldReturn400()
            {
                var response = AccountService
                               .Login(UserName, "");

                Assert.That((int)response.StatusCode, Is.EqualTo(400));
                Console.WriteLine($"✅ Empty Password Rejected!");
            }

            // ✅ TC010 - Login with Empty Credentials
            [Test, Order(10)]
            public void TC010_Login_EmptyCredentials_ShouldReturn400()
            {
                var response = AccountService.Login("", "");

                Assert.That((int)response.StatusCode, Is.EqualTo(400));
                Console.WriteLine($"✅ Empty Credentials Rejected!");
            }
        }
    }

