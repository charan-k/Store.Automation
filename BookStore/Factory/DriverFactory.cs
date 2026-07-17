using System;
using AutomationFramework.Core;
using AutomationFramework.Driver;

namespace AutomationFramework.Factory
{
    /// <summary>
    /// Creates WebDriver instances
    /// </summary>
    public class DriverFactory
    {
        /// <summary>
        /// Create WebDriver based on browser type
        /// </summary>
        public static IDriver CreateDriver(BrowserType browserType, int implicitWaitSeconds = 10)
        {
            try
            {
                IDriver driver = browserType switch
                {
                    BrowserType.Chrome => new ChromeDriver(implicitWaitSeconds),
                    BrowserType.Firefox => new FirefoxDriver(implicitWaitSeconds),
                    _ => throw new ArgumentException($"Unsupported browser: {browserType}")
                };

                Logger.Instance.Info($"WebDriver created for: {browserType}");
                return driver;
            }
            catch (Exception ex)
            {
                Logger.Instance.Error($"Failed to create WebDriver for {browserType}", ex);
                throw;
            }
        }
    }
}