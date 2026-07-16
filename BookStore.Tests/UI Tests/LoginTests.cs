using NUnit.Framework;
using AutomationFramework.Core;
using AutomationFramework.Pages;
using AutomationFramework.Utilities;

namespace AutomationFramework.Tests
{
    /// <summary>
    /// Login Tests - NUnit with Parallel Execution
    /// [Parallelizable] attribute enables parallel execution
    /// </summary>
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    [Category("Login")]
    public class LoginTests : BaseTest
    {
        private LoginPage _loginPage;
        private HomePage _homePage;

        [SetUp]
        public override void Setup()
        {
            base.Setup();

            // Navigate to login page
            string baseUrl = Config.Instance.Get("BaseUrl", "https://example.com");
            Driver.Navigate(baseUrl);

            // Initialize page objects
            _loginPage = new LoginPage(Driver);
            _homePage = new HomePage(Driver);

            Logger.Instance.Info("LoginTests Setup Complete");
        }

        [Test]
        [Order(1)]
        public void TestValidLogin()
        {
            Logger.Instance.Info("TEST: TestValidLogin Started");

            // Arrange
            var loginData = DataProvider.GetValidLoginData();

            // Act
            _loginPage.Login(loginData.Username, loginData.Password);
            _homePage.WaitForPageLoad();

            // Assert
            Assert.That(_homePage.IsUserProfileDisplayed(), Is.True, "User profile should be visible");
            Assert.That(_homePage.GetWelcomeMessage(), Is.Not.Empty, "Welcome message should be displayed");

            Logger.Instance.Info("TEST: TestValidLogin Passed");
        }

        [Test]
        [Order(2)]
        public void TestInvalidLogin()
        {
            Logger.Instance.Info("TEST: TestInvalidLogin Started");

            // Arrange
            var loginData = DataProvider.GetInvalidLoginData();

            // Act
            _loginPage.Login(loginData.Username, loginData.Password);
            Driver.Wait(1);

            // Assert
            Assert.That(_loginPage.IsErrorDisplayed(), Is.True, "Error message should be displayed");
            Assert.That(_loginPage.GetErrorMessage(), Is.Not.Empty, "Error message text should not be empty");

            Logger.Instance.Info("TEST: TestInvalidLogin Passed");
        }

        [Test]
        [Category("Smoke")]
        public void TestLoginPageTitle()
        {
            Logger.Instance.Info("TEST: TestLoginPageTitle Started");

            // Arrange & Act
            _loginPage.WaitForPageLoad();
            string pageTitle = _loginPage.GetPageTitle();

            // Assert
            Assert.That(pageTitle, Is.Not.Empty, "Page title should not be empty");

            Logger.Instance.Info("TEST: TestLoginPageTitle Passed");
        }

        [Test]
        [Ignore("This test is ignored for demonstration purposes")]
        [TestCaseSource(nameof(GetLoginTestCases))]
        public void TestLoginWithMultipleCredentials(TestData testData)
        {
            Logger.Instance.Info($"TEST: Testing login with {testData.Username}");

            // Act
            _loginPage.Login(testData.Username, testData.Password);
            Driver.Wait(1);

            // Assert
            if (testData.ExpectedResult == "Success")
            {
                Assert.That(_homePage.IsUserProfileDisplayed(), Is.True);
            }
            else
            {
                Assert.That(_loginPage.IsErrorDisplayed(), Is.True);
            }

            Logger.Instance.Info($"TEST: Login test for {testData.Username} passed");
        }

        private static System.Collections.IEnumerable GetLoginTestCases()
        {
            return DataProvider.GetMultipleLoginTestData();
        }
    }
}