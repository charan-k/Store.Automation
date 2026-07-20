using AutomationFramework.Core;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;


namespace AutomationFramework.Driver
{
    /// <summary>
    /// Chrome WebDriver Implementation
    /// </summary>
    public class ChromeDriver : IDriver
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        public ChromeDriver(int implicitWaitSeconds = 10)
        {
            Initialize(implicitWaitSeconds);
        }

        private void Initialize(int implicitWaitSeconds)
        {
            try
            {


                // ✅ FIX - Use MatchingBrowser strategy to auto-match installed Chrome version
                string driverPath = new DriverManager().SetUpDriver(
                    new ChromeConfig(),
                    VersionResolveStrategy.MatchingBrowser
                );
                // ✅ FIX - Null-safe directory extraction
                string driverDirectory = Path.GetDirectoryName(driverPath)
                    ?? throw new InvalidOperationException("Could not determine ChromeDriver directory.");

                Logger.Instance.Info($"Using ChromeDriver from: {driverPath}");

                var options = new ChromeOptions();
                options.AddArgument("--no-sandbox");
                options.AddArgument("--disable-dev-shm-usage");
                options.AddArgument("--disable-extensions");
                // Locally: Config reads false from appsettings.json
                // Jenkins: Config reads true from environment variable
                if (Config.Instance.Headless)
                {
                    options.AddArgument("--headless");
                    options.AddArgument("--disable-gpu");
                    options.AddArgument("--window-size=1920,1080");
                    Logger.Instance.Info("Chrome running in Headless mode");
                }

                // ✅ FIX - Pass driverDirectory so Selenium uses the correct ChromeDriver
                _driver = new OpenQA.Selenium.Chrome.ChromeDriver(driverDirectory, options);
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(implicitWaitSeconds);
                _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(implicitWaitSeconds));

                Logger.Instance.Info("Chrome WebDriver initialized successfully");

            }
            catch (Exception ex)
            {
                Logger.Instance.Error("Failed to initialize Chrome WebDriver", ex);
                throw;
            }
        }

        public void Navigate(string url)
        {
            try
            {
                _driver.Navigate().GoToUrl(url);
                Logger.Instance.Info($"Navigated to: {url}");
            }
            catch (Exception ex)
            {
                Logger.Instance.Error($"Failed to navigate to {url}", ex);
                throw;
            }
        }

        public IWebElement WaitForElement(string xpath, int timeoutSeconds = 30)
        {
            var wait = new WebDriverWait(
                _driver,
                TimeSpan.FromSeconds(timeoutSeconds)
            );
            return wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath(xpath)
            ));
        }
        public void Click(string xpath)
        {
            try
            {
                var element = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath(xpath)));
                element.Click();
                Logger.Instance.Info($"Clicked element: {xpath}");
            }
            catch (Exception ex)
            {
                Logger.Instance.Error($"Failed to click element: {xpath}", ex);
                throw;
            }
        }

        public void SendKeys(string xpath, string text)
        {
            try
            {
                var element = _driver.FindElement(By.XPath(xpath));
                element.SendKeys(text);
                Logger.Instance.Info($"Sent text to element: {xpath}");
            }
            catch (Exception ex)
            {
                Logger.Instance.Error($"Failed to send keys to element: {xpath}", ex);
                throw;
            }
        }

        public string GetText(string xpath)
        {
            try
            {
                var element = _driver.FindElement(By.XPath(xpath));
                return element.Text;
            }
            catch (Exception ex)
            {
                Logger.Instance.Error($"Failed to get text from element: {xpath}", ex);
                throw;
            }
        }

        public bool IsDisplayed(string xpath)
        {
            try
            {
                var element = _driver.FindElement(By.XPath(xpath));
                return element.Displayed;
            }
            catch
            {
                return false;
            }
        }

        public void Wait(int seconds)
        {
           Thread.Sleep(seconds * 1000);
        }

        public void Screenshot(string filename)
        {
            try
            {
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                screenshot.SaveAsFile(filename);
                Logger.Instance.Info($"Screenshot saved: {filename}");
            }
            catch (Exception ex)
            {
                Logger.Instance.Error($"Failed to take screenshot: {filename}", ex);
            }
        }

        public string GetUrl()
        {
            return _driver.Url;
        }

        public string GetTitle()
        {
            return _driver.Title;
        }

        public void Dispose()
        {
            try
            {
                _driver?.Quit();
                _driver?.Dispose();
                Logger.Instance.Info("Chrome WebDriver closed");
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("Failed to close WebDriver", ex);
            }
        }
    }
}