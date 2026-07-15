using System;
using System.IO;
using AutomationFramework.Core;
using AutomationFramework.Driver;

namespace AutomationFramework.Utilities
{
    /// <summary>
    /// Screenshot Helper - Manages screenshots
    /// OOPS: Single Responsibility Principle
    /// </summary>
    public class ScreenshotHelper
    {
        private readonly IDriver _driver;
        private readonly string _screenshotDir;

        public ScreenshotHelper(IDriver driver)
        {
            _driver = driver ?? throw new ArgumentNullException(nameof(driver));
            _screenshotDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");

            if (!Directory.Exists(_screenshotDir))
                Directory.CreateDirectory(_screenshotDir);
        }

        /// <summary>
        /// Take screenshot
        /// </summary>
        public void TakeScreenshot(string testName)
        {
            try
            {
                string filename = Path.Combine(_screenshotDir, $"{testName}_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.png");
                _driver.Screenshot(filename);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("Failed to take screenshot", ex);
            }
        }
    }
}