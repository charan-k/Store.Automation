using AutomationFramework.Core;
using AutomationFramework.Driver;
using OpenQA.Selenium;

namespace AutomationFramework.Pages
{
    /// <summary>
    /// Home Page - Concrete Page Object
    /// OOPS: Inheritance from BasePage
    /// </summary>
    public class HomePage : BasePage
    {
        // Page Locators - Encapsulation
        private const string WelcomeMessageXPath = "//div[@class='ms-auto text-end col-md-4 col-sm-12']";
        private const string UserProfileXPath = "//div[@class='profile-wrapper']";
        private const string LogoutButtonXPath = "//button[text()='Logout']";

        public HomePage(IDriver driver) : base(driver)
        {
        }

        public override void WaitForPageLoad()
        {
            base.WaitForPageLoad();
            Driver.Wait(3);
        }

        /// <summary>
        /// Get welcome message
        /// </summary>
        public string GetWelcomeMessage()
        {
            try
            {
                // ✅ Clean - no _wait needed in BasePage
                var element = Driver.WaitForElement(WelcomeMessageXPath);
                string message = element.Text.Trim();
                Logger.Instance.Info($"Welcome message found: '{message}'");
                return message;
            }
            catch (WebDriverTimeoutException)
            {
                Logger.Instance.Error("Welcome message not found within timeout");
                throw;
            }
        }

        /// <summary>
        /// Check if user profile is displayed
        /// </summary>
        public bool IsUserProfileDisplayed()
        {
            return Driver.IsDisplayed(UserProfileXPath);
        }

        /// <summary>
        /// Click logout button
        /// </summary>
        public void Logout()
        {
            Driver.Click(LogoutButtonXPath);
        }
    }
}