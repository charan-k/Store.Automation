using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace AutomationFramework.Core
{
    /// <summary>
    /// Singleton Configuration Manager
    /// OOPS: Encapsulation, Single Responsibility Principle
    /// </summary>
    public sealed class Config
    {
        private static readonly Lazy<Config> _instance = new Lazy<Config>(() => new Config());
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
                string configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");

                if (!File.Exists(configFile))
                {
                    Logger.Instance.Warning($"Config file not found: {configFile}");
                    return;
                }

                string json = File.ReadAllText(configFile);
                using (JsonDocument doc = JsonDocument.Parse(json))
                {
                    foreach (var prop in doc.RootElement.EnumerateObject())
                    {
                        if (prop.Value.ValueKind == JsonValueKind.String)
                        {
                            _settings[prop.Name] = prop.Value.GetString();
                        }
                        else
                        {
                            _settings[prop.Name] = prop.Value.GetRawText();
                        }
                    }
                }

                Logger.Instance.Info("Configuration loaded successfully");
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("Failed to load configuration", ex);
            }
        }

        /// <summary>
        /// Get string setting
        /// </summary>
        public string Get(string key, string defaultValue = "")
        {
            lock (_lock)
            {
                return _settings.TryGetValue(key, out var value) ? value?.ToString() ?? defaultValue : defaultValue;
            }
        }

        /// <summary>
        /// Get integer setting
        /// </summary>
        public int GetInt(string key, int defaultValue = 0)
        {
            lock (_lock)
            {
                if (_settings.TryGetValue(key, out var value))
                {
                    if (int.TryParse(value?.ToString(), out int result))
                        return result;
                }
                return defaultValue;
            }
        }

        /// <summary>
        /// Get boolean setting
        /// </summary>
        public bool GetBool(string key, bool defaultValue = false)
        {
            lock (_lock)
            {
                if (_settings.TryGetValue(key, out var value))
                {
                    if (bool.TryParse(value?.ToString(), out bool result))
                        return result;
                }
                return defaultValue;
            }
        }
    }
}