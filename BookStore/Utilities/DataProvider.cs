using System.Collections.Generic;

namespace AutomationFramework.Utilities
{
    /// <summary>
    /// Data Provider - Provides test data
    /// OOPS: Single Responsibility Principle
    /// </summary>
    public static class DataProvider
    {

        /// <summary>
        /// Get valid login credentials
        /// </summary>
        public static TestData PrepareValidLoginData()
        {
            return new TestData("TestUser_" + DateTime.Now.ToString("yyyyMMddHHmmss"), "Password@123", "Success");
        }

        /// <summary>
        /// Get valid login credentials
        /// </summary>
        public static TestData GetValidLoginData()
        {
            return new TestData("charanInduri", "Password@123", "Success");
        }

        /// <summary>
        /// Get invalid login credentials
        /// </summary>
        public static TestData GetInvalidLoginData()
        {
            return new TestData("invalid", "wrongpass", "Failure");
        }

        /// <summary>
        /// Get multiple test data sets
        /// </summary>
        public static IEnumerable<TestData> GetMultipleLoginTestData()
        {
            yield return new TestData("user1", "pass1", "Success");
            yield return new TestData("user2", "pass2", "Success");
            yield return new TestData("user3", "pass3", "Success");
            yield return new TestData("invalid", "wrongpass", "Failure");
        }
    }
}