namespace AutomationFramework.Utilities
{
    /// <summary>
    /// Test data model
    /// </summary>
    public class TestData
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ExpectedResult { get; set; }

        public TestData(string username, string password, string expectedResult)
        {
            Username = username;
            Password = password;
            ExpectedResult = expectedResult;
        }
    }
}