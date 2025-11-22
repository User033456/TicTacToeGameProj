using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace TicTacToeGameProj
{
    /// <summary>
    /// Локализация на основе текстовых файлов lang_xx.txt (key=value).
    /// Работает с XAML через индексатор и TranslateExtension.
    /// </summary>
    public class LocalizationService : INotifyPropertyChanged
    {
        private static readonly Lazy<LocalizationService> _instance =
            new(() => new LocalizationService());

        public static LocalizationService Instance => _instance.Value;

        private readonly Dictionary<string, string> _strings = new();

        public string CurrentLanguageKey { get; private set; } = "en";

        public event PropertyChangedEventHandler? PropertyChanged;

        private LocalizationService()
        {
            
        }

        /// <summary>
        /// Индексатор для XAML: [{Key}]
        /// </summary>
        public string this[string key]
        {
            get
            {
                if (string.IsNullOrEmpty(key))
                    return string.Empty;

                return _strings.TryGetValue(key, out var value)
                    ? value
                    : key; // если нет — показываем сам ключ, но не падаем
            }
        }

        /// <summary>
        /// Загрузка lang_{languageKey}.txt из Resources/Raw.
        /// </summary>
        public async Task SetLanguageAsync(string languageKey)
        {
            if (string.IsNullOrWhiteSpace(languageKey))
                languageKey = "en";

            if (CurrentLanguageKey == languageKey && _strings.Count > 0)
                return;

            CurrentLanguageKey = languageKey;
            _strings.Clear();

            string filePath = $"Resources/Raw/lang_{languageKey}.txt";

            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync(filePath);
                using var reader = new StreamReader(stream);

                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    line = line.Trim();
                    if (line.StartsWith("#"))
                        continue;

                    int eqIndex = line.IndexOf('=');
                    if (eqIndex <= 0)
                        continue;

                    string key = line[..eqIndex].Trim();
                    string value = line[(eqIndex + 1)..].Trim();

                    if (!string.IsNullOrEmpty(key))
                        _strings[key] = value;
                }

                System.Diagnostics.Debug.WriteLine($"[Localization] Loaded {_strings.Count} entries from {filePath}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Localization] Error loading {filePath}: {ex.Message}");
            }

            // ВАЖНО: сообщаем, что изменилось "всё"
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
            // (можно ещё дополнительно так, для надёжности)
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));
        }

    }
}
