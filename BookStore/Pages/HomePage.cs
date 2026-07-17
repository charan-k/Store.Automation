using AutomationFramework.Core;
using AutomationFramework.Driver;
using OpenQA.Selenium;

namespace AutomationFramework.Pages
{
    /// <summary>
    /// Home Page - Concrete Page Object
    /// </summary>
    public class HomePage : BasePage
    {
        // Page Locators - Encapsulation
        private const string WelcomeMessageXPath = "//div[contains(@class, 'ms-auto') and contains(@class, 'text-end')]";
        private const string UserProfileXPath = "//div[contains(@class, 'profile-wrapper')]";
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
            try
            {
                var element = Driver.WaitForElement(UserProfileXPath);
                bool isDisplayed = element.Displayed;
                Logger.Instance.Info($"User profile displayed: {isDisplayed}");
                return isDisplayed;
            }
            catch (WebDriverTimeoutException)
            {
                Logger.Instance.Error("User profile not visible within timeout");
                return false;
            }
            catch (NoSuchElementException)
            {
                Logger.Instance.Error("User profile element not found in DOM");
                return false;
            }
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