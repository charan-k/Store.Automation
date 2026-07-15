using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;

namespace AutomationFramework.Core
{
    public sealed class Config
    {
        private static readonly Lazy<Config> _instance =
                new Lazy<Config>(() => new Config());
        private readonly Dictionary<string, object> _settings;
        private readonly object _lock = new object();

        public static Config Instance => _instance.Value;

        private Config()
        {
            _settings = new Dictionary<string, object>();
            LoadSettings();
        }

        private void LoadSettings()
        {
            try
            {
                string configFile = Path.Combine(
                       AppDomain.CurrentDomain.BaseDirectory,
                       "appsettings.json");

                if (!File.Exists(configFile))
                {
                    Logger.Instance.Warning(
                        $"Config file not found: {configFile}");
                    return;
                }

                string json = File.ReadAllText(configFile);
                using (JsonDocument doc = JsonDocument.Parse(json))
                {
                    foreach (var prop in
                             doc.RootElement.EnumerateObject())
                    {
                        if (prop.Value.ValueKind ==
                            JsonValueKind.String)
                        {
                            _settings[prop.Name] =
                                prop.Value.GetString();
                        }
                        else
                        {
                            _settings[prop.Name] =
                                prop.Value.GetRawText();
                        }
                    }
                }

                // ✅ NEW - Override with Jenkins
                // environment variables
                OverrideWithEnvironmentVariables();

                Logger.Instance.Info(
                    "Configuration loaded successfully");
            }
            catch (Exception ex)
            {
                Logger.Instance.Error(
                    "Failed to load configuration", ex);
            }
        }

        // ✅ NEW - Jenkins overrides these
        // via environment variables
        private void OverrideWithEnvironmentVariables()
        {
            // ✅ These match your appsettings.json keys
            var envVariables = new[]
            {
                "BrowserType",
                "BaseUrl",
                "BaseAPIUrl",
                "Headless",
                "ImplicitWaitSeconds",
                "ExplicitWaitSeconds",
                "PageLoadTimeout",
                "ScreenshotOnFailure"
            };

            foreach (var key in envVariables)
            {
                var envValue =
                    Environment.GetEnvironmentVariable(key);

                if (!string.IsNullOrEmpty(envValue))
                {
                    _settings[key] = envValue;
                    Logger.Instance.Info(
                        $"✅ Jenkins Override: " +
                        $"{key} = {envValue}");
                }
            }
        }

        public string Get(
               string key,
               string defaultValue = "")
        {
            lock (_lock)
            {
                return _settings.TryGetValue(
                       key, out var value)
                       ? value?.ToString() ?? defaultValue
                       : defaultValue;
            }
        }

        public int GetInt(
               string key,
               int defaultValue = 0)
        {
            lock (_lock)
            {
                if (_settings.TryGetValue(key, out var value))
                {
                    if (int.TryParse(
                        value?.ToString(), out int result))
                        return result;
                }
                return defaultValue;
            }
        }

        public bool GetBool(
               string key,
               bool defaultValue = false)
        {
            lock (_lock)
            {
                if (_settings.TryGetValue(key, out var value))
                {
                    if (bool.TryParse(
                        value?.ToString(), out bool result))
                        return result;
                }
                return defaultValue;
            }
        }

        // ✅ NEW - Strongly typed properties
        // matching your appsettings.json keys exactly
        public string BrowserType =>
               Get("BrowserType", "Chrome");

        public string BaseUrl =>
               Get("BaseUrl", "https://demoqa.com/login");

        public string BaseAPIUrl =>
               Get("BaseAPIUrl", "https://demoqa.com");

        // ✅ Headless is false locally
        // Jenkins overrides to true
        public bool Headless =>
               GetBool("Headless", false);

        public int ImplicitWaitSeconds =>
               GetInt("ImplicitWaitSeconds", 20);

        public int ExplicitWaitSeconds =>
               GetInt("ExplicitWaitSeconds", 30);

        public bool ScreenshotOnFailure =>
               GetBool("ScreenshotOnFailure", true);

        public int PageLoadTimeout =>
               GetInt("PageLoadTimeout", 60);
    }
}