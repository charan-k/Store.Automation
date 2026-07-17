using System;
using AutomationFramework.Core;
using AutomationFramework.Driver;

namespace AutomationFramework.Pages
{
    /// <summary>
    /// Base Page Object 
    /// </summary>
    public abstract class BasePage
    {
        protected IDriver Driver { get; }

        protected BasePage(IDriver driver)
        {
            Driver = driver ?? throw new ArgumentNullException(nameof(driver));
        }

        /// <summary>
        /// Wait for page to load - Virtual for overriding
        /// </summary>
        public virtual void WaitForPageLoad()
        {
            Logger.Instance.Info($"Waiting for page: {GetType().Name}");
        }

        /// <summary>
        /// Get page title
        /// </summary>
        public string GetPageTitle()
        {
            return Driver.GetTitle();
        }

        /// <summary>
        /// Wait for specific seconds
        /// </summary>
        protected void Wait(int seconds)
        {
            Driver.Wait(seconds);
        }
    }
}