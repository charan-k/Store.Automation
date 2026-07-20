using AutomationFramework.Core;
using AutomationFramework.Driver;
using AutomationFramework.Factory;
using AutomationFramework.Utilities;
using NUnit.Framework.Interfaces;

namespace AutomationFramework.Tests
{
    // ✅ Add Category HERE in Base class
    // So ALL UI tests inherit this category
    // No need to add in every test class!
    [TestFixture]
    [Category("UI")]  // 👈 ADD THIS
    public abstract class BaseTest
    {
        protected IDriver Driver { get; private set; }
        protected ScreenshotHelper ScreenshotHelper { get; private set; }

        [SetUp]
        public virtual void Setup()
        {
            try
            {
                string browserConfig = Config.Instance.Get("BrowserType","Chrome");
                BrowserType browser = (BrowserType)Enum.Parse(typeof(BrowserType),browserConfig);
                int waitTime = Config.Instance.GetInt("ImplicitWaitSeconds");

                Driver = DriverFactory.CreateDriver(browser);
                ScreenshotHelper =new ScreenshotHelper(Driver);

                Logger.Instance.Info($"✅ UI Setup: {GetType().Name}");
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("Setup failed", ex);
                throw;
            }
        }

        [TearDown]
        public virtual void Teardown()
        {
            try
            {
                var testName = TestContext.CurrentContext.Test.Name;
                var status = TestContext.CurrentContext.Result.Outcome.Status;

                if (status == TestStatus.Failed)
                {
                    ScreenshotHelper?.TakeScreenshot($"{testName}_Failed");

                    Logger.Instance.Warning($"❌ Test Failed: {testName}"); 

                }
                else
                {
                    Logger.Instance.Info($"✅ Test Passed: {testName}");
                    ScreenshotHelper?.TakeScreenshot($"{testName}_Passed");
                }

                Driver?.Dispose();
                Logger.Instance.Info( $"✅ UI Teardown: {GetType().Name}");
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("Teardown failed", ex);
            }
        }
    }
}