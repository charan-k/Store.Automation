using AutomationFramework.Driver;

namespace AutomationFramework.Pages
{
    /// <summary>
    /// Login Page - Concrete Page Object
    /// OOPS: Inheritance from BasePage
    /// </summary>
    public class LoginPage : BasePage
    {
        // Page Locators - Encapsulation
        private const string UsernameXPath = "//input[@id='userName']";
        private const string PasswordXPath = "//input[@id='password']";
        private const string LoginButtonXPath = "//button[@id='login']";
        private const string ErrorMessageXPath = "//div[@class='col-md-12 col-sm-12']";

        //private const string UsernameXPath = "//input[@id='Input_Email']";
        //private const string PasswordXPath = "//input[@id='Input_Password']";
        //private const string LoginButtonXPath = "//button[@id='login-submit']";
        //private const string ErrorMessageXPath = "//div[@class='text-danger validation-summary-errors']";
        public LoginPage(IDriver driver) : base(driver)
        {
        }

        public override void WaitForPageLoad()
        {
            base.WaitForPageLoad();
            // Wait for username field to be visible
            Driver.Wait(1);
        }

        /// <summary>
        /// Enter username
        /// </summary>
        public void EnterUsername(string username)
        {
            Driver.SendKeys(UsernameXPath, username);
        }

        /// <summary>
        /// Enter password
        /// </summary>
        public void EnterPassword(string password)
        {
            Driver.SendKeys(PasswordXPath, password);
        }

        /// <summary>
        /// Click login button
        /// </summary>
        public void ClickLogin()
        {
            Driver.Click(LoginButtonXPath);
        }

        /// <summary>
        /// Get error message
        /// </summary>
        public string GetErrorMessage()
        {
            return Driver.GetText(ErrorMessageXPath);
        }

        /// <summary>
        /// Check if error is displayed
        /// </summary>
        public bool IsErrorDisplayed()
        {
            return Driver.IsDisplayed(ErrorMessageXPath);
        }

        /// <summary>
        /// Login with credentials
        /// </summary>
        public void Login(string username, string password)
        {
            WaitForPageLoad();
            EnterUsername(username);
            EnterPassword(password);
            ClickLogin();
            Driver.Wait(2);
        }
    }
}