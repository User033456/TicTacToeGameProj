using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Maui.Storage;

namespace TicTacToeGameProj.Services
{
    /// <summary>
    /// Класс для управления конфигом настроек
    /// </summary>
    public static class ConfigManager
    {
        private static readonly string ConfigPath =
            Path.Combine(FileSystem.AppDataDirectory, "config.txt");

        // дефолтные значения
        private static readonly Dictionary<string, string> DefaultConfig =
            new(StringComparer.OrdinalIgnoreCase)
            {
                { "theme", "BASIC" },
                { "language", "English" },
                { "music", "OFF" } // OFF или ON
            };

        /// <summary>
        /// Загружает конфиг. Если файла нет — создаёт с дефолтами и возвращает их.
        /// </summary>
        public static Dictionary<string, string> LoadConfig()
        {
            if (!File.Exists(ConfigPath))
            {
                // если файла нет — сохранить дефолт и отдать копию
                SaveConfig(DefaultConfig);
                return new Dictionary<string, string>(DefaultConfig, StringComparer.OrdinalIgnoreCase);
            }

            var config = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var line in File.ReadAllLines(ConfigPath))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                int idx = line.IndexOf('=');
                if (idx <= 0)
                    continue;

                string key = line[..idx].Trim();
                string value = line[(idx + 1)..].Trim();

                if (!string.IsNullOrEmpty(key))
                    config[key] = value;
            }

            // если каких-то ключей нет в файле — дополним дефолтами
            foreach (var pair in DefaultConfig)
            {
                if (!config.ContainsKey(pair.Key))
                    config[pair.Key] = pair.Value;
            }

            return config;
        }

        /// <summary>
        /// Сохранение конфига  (ключ=значение построчно).
        /// </summary>
        public static void SaveConfig(Dictionary<string, string> config)
        {
            var lines = new List<string>();

            foreach (var pair in config)
                lines.Add($"{pair.Key}={pair.Value}");

            File.WriteAllLines(ConfigPath, lines);
        }
    }
}
