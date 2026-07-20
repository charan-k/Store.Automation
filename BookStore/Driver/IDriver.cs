using OpenQA.Selenium;
using System;

namespace AutomationFramework.Driver
{
    /// <summary>
    /// Interface for WebDriver
    /// </summary>
    public interface IDriver : IDisposable
    {
        void Navigate(string url);
        void Click(string xpath);
        void SendKeys(string xpath, string text);
        string GetText(string xpath);
        bool IsDisplayed(string xpath);
        void Wait(int seconds);
        void Screenshot(string filename);
        string GetUrl();
        string GetTitle();

        IWebElement WaitForElement(string xpath, int timeoutSeconds = 60);
    }
}