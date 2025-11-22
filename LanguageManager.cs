using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToeGameProj
{
    public class LanguageInfo
    {
        public string Key { get; }
        public string DisplayName { get; }

        public LanguageInfo(string key, string displayName)
        {
            Key = key;
            DisplayName = displayName;
        }
    }

    public static class LanguageManager
    {
        // сюда можно добавлять любые новые языки
        private static readonly List<LanguageInfo> _languages = new()
        {
           new("en", "English"),
    new("ru", "Русский"),
    new("ja", "日本語"),
    new("es", "Español"),
    new("pt", "Português"),
    new("he", "עברית"),
    new("de", "Deutsch"),
    new("fr", "Français"),
    new("it", "Italiano"),
    new("hi", "हिन्दी"),
    new("zh", "中文")
        };

        public static IReadOnlyList<LanguageInfo> Languages => _languages;
        public static LanguageInfo CurrentLanguage { get; private set; } = _languages[0];

        /// <summary>
        /// Инициализация текущего языка по конфигу.
        /// Поддерживает и ключи ("en"), и полные названия ("English").
        /// </summary>
        public static void Initialize(Dictionary<string, string> config)
        {
            LanguageInfo defaultLang = _languages[0];
            LanguageInfo? selected = null;

            if (config.TryGetValue("language", out var value))
            {
                selected = _languages.FirstOrDefault(l =>
                    string.Equals(l.Key, value, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(l.DisplayName, value, StringComparison.OrdinalIgnoreCase));
            }

            CurrentLanguage = selected ?? defaultLang;

            // нормализуем значение в конфиге
            config["language"] = CurrentLanguage.DisplayName;
        }

        public static void SetLanguage(LanguageInfo lang)
        {
            if (lang != null)
            {
                CurrentLanguage = lang;
            }
        }
    }
}
