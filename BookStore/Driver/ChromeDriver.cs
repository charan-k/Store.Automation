using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using AutomationFramework.Core;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;


namespace AutomationFramework.Driver
{
    /// <summary>
    /// Chrome WebDriver Implementation - Polymorphism
    /// OOPS: Concrete implementation of IDriver interface
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
                new DriverManager().SetUpDriver(new ChromeConfig());
                var options = new ChromeOptions();
                options.AddArgument("--no-sandbox");
                options.AddArgument("--disable-dev-shm-usage");

                _driver = new OpenQA.Selenium.Chrome.ChromeDriver(options);
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(implicitWaitSeconds);
                _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(implicitWaitSeconds));

                Logger.Instance.Info("Chrome WebDriver initialized");
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