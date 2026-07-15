using NUnit.Framework;
using AutomationFramework.Core;
using AutomationFramework.Pages;
using AutomationFramework.Utilities;

namespace AutomationFramework.Tests
{
    /// <summary>
    /// Home Page Tests - NUnit with Parallel Execution
    /// [Parallelizable] attribute enables parallel execution
    /// </summary>
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    [Category("HomePage")]
    public class HomePageTests : BaseTest
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

            // Login first
            var loginData = DataProvider.GetValidLoginData();
            _loginPage.Login(loginData.Username, loginData.Password);
            _homePage.WaitForPageLoad();

            Logger.Instance.Info("HomePageTests Setup Complete");
        }

        [Test]
        [Order(1)]
        public void TestWelcomeMessageDisplayed()
        {
            Logger.Instance.Info("TEST: TestWelcomeMessageDisplayed Started");

            // Arrange & Act
            string welcomeMessage = _homePage.GetWelcomeMessage();

            // Assert
            Assert.That(welcomeMessage, Is.Not.Empty, "Welcome message should be displayed");

            Logger.Instance.Info("TEST: TestWelcomeMessageDisplayed Passed");
        }

        [Test]
        [Order(2)]
        public void TestUserProfileVisible()
        {
            Logger.Instance.Info("TEST: TestUserProfileVisible Started");

            // Arrange & Act
            bool isProfileVisible = _homePage.IsUserProfileDisplayed();

            // Assert
            Assert.That(isProfileVisible, Is.True, "User profile should be visible");

            Logger.Instance.Info("TEST: TestUserProfileVisible Passed");
        }

        [Test]
        public void TestHomePageUrl()
        {
            Logger.Instance.Info("TEST: TestHomePageUrl Started");

            // Arrange & Act
            string currentUrl = Driver.GetUrl();

            // Assert
            Assert.That(currentUrl, Is.Not.Empty, "Current URL should not be empty");

            Logger.Instance.Info("TEST: TestHomePageUrl Passed");
        }
    }
}